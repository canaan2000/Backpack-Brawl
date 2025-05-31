// MergeFlowEditor.cs
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class MergeFlowEditor : EditorWindow
{
    private MergeDictionary targetMergeDictionary; // Reference to the script we're inspecting
    private Vector2 scrollPosition; // For scrolling if the content is larger than the window

    // Dictionaries to manage node positions and sizes
    private Dictionary<string, Rect> nodeRects = new Dictionary<string, Rect>();
    private Dictionary<string, string> tagOrPrefabToNodeId = new Dictionary<string, string>(); // Maps a tag/prefab name to a unique node ID
    private Dictionary<string, List<string>> nodeOutputs = new Dictionary<string, List<string>>(); // Node ID to list of node IDs it produces (ResultPrefab nodes)
    private Dictionary<string, List<string>> nodeInputs = new Dictionary<string, List<string>>(); // Node ID to list of node IDs that produce it (Obj1/Obj2 nodes)

    // --- Node Drawing Parameters ---
    private const float NodeWidth = 150;
    private const float NodeHeight = 50;
    private const float NodePadding = 20;
    private const float LayerXOffset = 200; // Horizontal spacing between layers
    private const float LayerYOffset = 80;  // Vertical spacing within layers

    [MenuItem("Window/Merge Flow Editor")]
    public static void ShowWindow()
    {
        GetWindow<MergeFlowEditor>("Merge Flow Editor").Show();
    }

    private void OnGUI()
    {
        // 1. Select the MergeDictionary instance
        EditorGUILayout.LabelField("Select MergeDictionary:", EditorStyles.boldLabel);
        MergeDictionary previousTarget = targetMergeDictionary; // Keep track to detect change
        targetMergeDictionary = (MergeDictionary)EditorGUILayout.ObjectField(
            targetMergeDictionary, typeof(MergeDictionary), true);

        if (targetMergeDictionary == null)
        {
            EditorGUILayout.HelpBox("Drag a MergeDictionary component here to visualize its flow.", MessageType.Info);
            nodeRects.Clear(); // Clear old data if no target is selected
            tagOrPrefabToNodeId.Clear();
            nodeOutputs.Clear();
            nodeInputs.Clear();
            return;
        }

        // --- Important: Rebuild graph data only when necessary ---
        // (e.g., target changed, list size changed, or forced refresh)
        if (targetMergeDictionary != previousTarget ||
            targetMergeDictionary.MergeDataList.Count != nodeOutputs.Keys.Count + nodeInputs.Keys.Count && targetMergeDictionary.MergeDataList.Count > 0) // Simple change detection
        {
            UpdateGraphData(true); // Force re-initialization of positions
        }
        else if (Event.current.type == EventType.Repaint)
        {
            // If just a repaint, ensure data is consistent without resetting positions
            UpdateGraphData(false);
        }

        // --- 2. Begin Drawing the Graph Area ---
        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        // Use a fixed size for the scroll view content to ensure lines draw correctly
        // and scroll bars appear when needed.
        float contentWidth = Mathf.Max(position.width, nodeRects.Values.Any() ? nodeRects.Values.Max(r => r.xMax) + NodePadding : position.width);
        float contentHeight = Mathf.Max(position.height, nodeRects.Values.Any() ? nodeRects.Values.Max(r => r.yMax) + NodePadding : position.height);

        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Width(position.width), GUILayout.Height(position.height - EditorGUIUtility.singleLineHeight * 2 - 20)); // Adjust height to fit object field

        GUILayoutUtility.GetRect(contentWidth, contentHeight); // Reserve space for drawing

        // --- 3. Draw Connections (Lines/Arrows) ---
        DrawConnections();

        // --- 4. Draw Nodes ---
        DrawNodes();

        EditorGUILayout.EndScrollView();
        EditorGUILayout.EndVertical();

        // 5. Handle user input (dragging nodes)
        ProcessEvents(Event.current);

        // Request repaint to ensure smooth dragging and updates
        if (GUI.changed) Repaint();
    }

    private void UpdateGraphData(bool forceInitializePositions)
    {
        // Clear node relationships but keep existing positions unless forced
        nodeOutputs.Clear();
        nodeInputs.Clear();
        tagOrPrefabToNodeId.Clear();

        // Step 1: Collect all unique tags and result prefabs as potential nodes
        HashSet<string> allUniqueTags = new HashSet<string>();
        HashSet<string> allUniquePrefabNames = new HashSet<string>();
        HashSet<string> allResultPrefabNames = new HashSet<string>(); // Keep track of only actual result nodes

        foreach (var data in targetMergeDictionary.MergeDataList)
        {
            if (!string.IsNullOrEmpty(data.Obj1Tag)) allUniqueTags.Add(data.Obj1Tag);
            if (!string.IsNullOrEmpty(data.Obj2Tag)) allUniqueTags.Add(data.Obj2Tag);
            if (data.ResultPrefab != null)
            {
                allUniquePrefabNames.Add(data.ResultPrefab.name);
                allResultPrefabNames.Add(data.ResultPrefab.name);
            }
        }

        // Map tags/prefab names to unique node IDs and initialize positions if needed
        int nodeIdCounter = 0;
        foreach (string tag in allUniqueTags.Concat(allUniquePrefabNames).Distinct()) // Combine all potential node names
        {
            string nodeId = "Node_" + tag.GetHashCode().ToString(); // Use hash for unique ID
            tagOrPrefabToNodeId[tag] = nodeId;

            // Only initialize position if it doesn't exist or if forcing a re-layout
            if (forceInitializePositions || !nodeRects.ContainsKey(nodeId))
            {
                nodeRects[nodeId] = new Rect(0, 0, NodeWidth, NodeHeight); // Temporary rect, will be updated by layout
            }
        }

        // Step 2: Build the connections (nodeOutputs and nodeInputs)
        foreach (var data in targetMergeDictionary.MergeDataList)
        {
            string obj1Tag = data.Obj1Tag;
            string obj2Tag = data.Obj2Tag;
            string resultPrefabName = (data.ResultPrefab != null) ? data.ResultPrefab.name : null;

            string obj1NodeId = string.IsNullOrEmpty(obj1Tag) ? null : tagOrPrefabToNodeId.ContainsKey(obj1Tag) ? tagOrPrefabToNodeId[obj1Tag] : null;
            string obj2NodeId = string.IsNullOrEmpty(obj2Tag) ? null : tagOrPrefabToNodeId.ContainsKey(obj2Tag) ? tagOrPrefabToNodeId[obj2Tag] : null;
            string resultNodeId = string.IsNullOrEmpty(resultPrefabName) ? null : tagOrPrefabToNodeId.ContainsKey(resultPrefabName) ? tagOrPrefabToNodeId[resultPrefabName] : null;

            if (resultNodeId != null)
            {
                // Source nodes
                if (obj1NodeId != null)
                {
                    if (!nodeOutputs.ContainsKey(obj1NodeId)) nodeOutputs[obj1NodeId] = new List<string>();
                    nodeOutputs[obj1NodeId].Add(resultNodeId);
                    if (!nodeInputs.ContainsKey(resultNodeId)) nodeInputs[resultNodeId] = new List<string>();
                    nodeInputs[resultNodeId].Add(obj1NodeId);
                }
                if (obj2NodeId != null)
                {
                    if (!nodeOutputs.ContainsKey(obj2NodeId)) nodeOutputs[obj2NodeId] = new List<string>();
                    nodeOutputs[obj2NodeId].Add(resultNodeId);
                    if (!nodeInputs.ContainsKey(resultNodeId)) nodeInputs[resultNodeId] = new List<string>();
                    nodeInputs[resultNodeId].Add(obj2NodeId);
                }
            }
        }

        // Step 3: Simple Layered Layout (only if forceInitializePositions is true)
        if (forceInitializePositions)
        {
            ApplyLayeredLayout(allUniqueTags, allUniquePrefabNames, allResultPrefabNames);
        }
    }

    private void ApplyLayeredLayout(HashSet<string> allUniqueTags, HashSet<string> allUniquePrefabNames, HashSet<string> allResultPrefabNames)
    {
        // Define "layers" of nodes
        // Layer 0: Initial tags (that are not results of any merge)
        // Layer 1: Result prefabs
        // Layer 2: Tags that are results and also inputs to other merges (optional)

        HashSet<string> sourceTags = new HashSet<string>();
        foreach (string tag in allUniqueTags)
        {
            if (!allResultPrefabNames.Contains(tag)) // If a tag is not a result prefab, consider it a source
            {
                sourceTags.Add(tag);
            }
        }

        // Collect result prefabs
        HashSet<string> resultPrefabs = new HashSet<string>(allResultPrefabNames);

        // Position nodes
        float currentX = NodePadding;
        float currentY = NodePadding;

        // Layer 0: Source Tags
        int yCount = 0;
        foreach (string tag in sourceTags)
        {
            string nodeId = tagOrPrefabToNodeId[tag];
            nodeRects[nodeId] = new Rect(currentX, currentY + yCount * (NodeHeight + NodePadding), NodeWidth, NodeHeight);
            yCount++;
        }
        currentX += LayerXOffset;
        currentY = NodePadding;
        yCount = 0;

        // Layer 1: Result Prefabs
        foreach (string prefabName in resultPrefabs)
        {
            string nodeId = tagOrPrefabToNodeId[prefabName];
            nodeRects[nodeId] = new Rect(currentX, currentY + yCount * (NodeHeight + NodePadding), NodeWidth, NodeHeight);
            yCount++;
        }
        // You might want to add more layers for complex dependencies if a result can also be a source.
        // For simplicity, we're assuming a two-layer system for now.
    }


    private void DrawNodes()
    {
        BeginWindows(); // Required to make GUI.Window movable

        // Draw each node
        foreach (var entry in tagOrPrefabToNodeId)
        {
            string tagOrPrefabName = entry.Key; // The actual tag or prefab name
            string nodeId = entry.Value;        // The unique ID (Node_hashcode)

            // Ensure the rect exists before trying to draw
            if (nodeRects.ContainsKey(nodeId))
            {
                Rect nodeRect = nodeRects[nodeId];
                // GUI.Window handles dragging if GUI.DragWindow() is called in the callback
                // It returns the new position of the window after dragging.
                nodeRect = GUI.Window(nodeId.GetHashCode(), nodeRect, DrawNodeWindow, tagOrPrefabName, "window");
                nodeRects[nodeId] = nodeRect; // Update the stored position
            }
        }

        EndWindows();
    }

    // This method defines the content of each individual node window
    private void DrawNodeWindow(int id)
    {
        // Reverse lookup the actual node ID from the hash code
        string nodeId = tagOrPrefabToNodeId.FirstOrDefault(x => x.Value.GetHashCode() == id).Value;
        string tagOrPrefabName = tagOrPrefabToNodeId.FirstOrDefault(x => x.Value.GetHashCode() == id).Key;

        // Display the name centered
        GUI.Label(new Rect(5, 20, NodeWidth - 10, 20), tagOrPrefabName, EditorStyles.boldLabel);

        // Allow dragging the window itself
        GUI.DragWindow();
    }

    private void DrawConnections()
    {
        if (targetMergeDictionary == null || nodeOutputs.Count == 0) return;

        Handles.BeginGUI(); // Required for drawing lines in IMGUI

        foreach (var data in targetMergeDictionary.MergeDataList)
        {
            string obj1Tag = data.Obj1Tag;
            string obj2Tag = data.Obj2Tag;
            string resultPrefabName = (data.ResultPrefab != null) ? data.ResultPrefab.name : null;

            string obj1NodeId = string.IsNullOrEmpty(obj1Tag) ? null : tagOrPrefabToNodeId.ContainsKey(obj1Tag) ? tagOrPrefabToNodeId[obj1Tag] : null;
            string obj2NodeId = string.IsNullOrEmpty(obj2Tag) ? null : tagOrPrefabToNodeId.ContainsKey(obj2Tag) ? tagOrPrefabToNodeId[obj2Tag] : null;
            string resultNodeId = string.IsNullOrEmpty(resultPrefabName) ? null : tagOrPrefabToNodeId.ContainsKey(resultPrefabName) ? tagOrPrefabToNodeId[resultPrefabName] : null;

            Rect resultRect;
            if (resultNodeId != null && nodeRects.TryGetValue(resultNodeId, out resultRect))
            {
                // Draw connection from Obj1Tag to ResultPrefab
                if (obj1NodeId != null && nodeRects.ContainsKey(obj1NodeId))
                {
                    DrawNodeConnection(nodeRects[obj1NodeId], resultRect, Color.blue);
                }
                // Draw connection from Obj2Tag to ResultPrefab
                if (obj2NodeId != null && nodeRects.ContainsKey(obj2NodeId))
                {
                    DrawNodeConnection(nodeRects[obj2NodeId], resultRect, Color.cyan);
                }
            }
        }

        Handles.EndGUI();
    }

    // Draws a line connection between two node rectangles
    private void DrawNodeConnection(Rect startRect, Rect endRect, Color color)
    {
        Vector3 startPos = new Vector3(startRect.xMax, startRect.center.y, 0); // Right side of start node
        Vector3 endPos = new Vector3(endRect.xMin, endRect.center.y, 0);       // Left side of end node

        // Optional: Add Bezier curves for smoother connections
        Vector3 startTangent = startPos + Vector3.right * 50;
        Vector3 endTangent = endPos + Vector3.left * 50;

        Handles.color = color;
        Handles.DrawBezier(startPos, endPos, startTangent, endTangent, color, null, 3); // Thicker line
    }

    // This method handles mouse events for the entire editor window, primarily for repainting
    private void ProcessEvents(Event e)
    {
        // If a node is being dragged (GUI.DragWindow active), GUI.changed will be true automatically.
        // We just need to repaint.

        // If the mouse is being dragged and not over a GUI element (like a node)
        // this is where you'd implement panning the entire canvas.
        // This is a simple example for dragging the whole canvas if needed.
        if (e.type == EventType.MouseDrag && e.button == 0 && GUIUtility.hotControl == 0) // GUIUtility.hotControl == 0 means no GUI control is active (e.g., a node isn't being dragged)
        {
            scrollPosition -= e.delta; // Pan the scroll view
            GUI.changed = true;
            e.Use();
        }
        else if (e.type == EventType.MouseDown || e.type == EventType.MouseUp)
        {
            GUI.changed = true; // Force repaint on clicks, good for responsiveness
            e.Use();
        }
    }
}
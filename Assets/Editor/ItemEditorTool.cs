using UnityEditor;
using UnityEngine;
using System.Collections.Generic; // Required for List

public class ItemEditorTool : EditorWindow
{
    private List<GameObject> itemPrefabs;
    private List<string> itemPrefabNames; // To store names for the popup
    private int currentIndex = 0;
    private GameObject currentPrefab;
    private NewItemScript currentItemScript;

    // Add an Editor reference for the current prefab's inspector
    private Editor prefabEditor;

    private Vector2 scrollPosition; // For the scroll view

    [MenuItem("Tools/Item Editor")]
    public static void ShowWindow()
    {
        GetWindow<ItemEditorTool>("Item Editor");
    }

    private void OnEnable()
    {
        LoadItemPrefabsFromResources();
        // Initialize currentPrefab and currentItemScript if prefabs are found
        if (itemPrefabs != null && itemPrefabs.Count > 0)
        {
            LoadCurrentPrefab();
        }
    }

    private void OnGUI()
    {
        GUILayout.Label("Item Prefab Editor", EditorStyles.boldLabel);

        if (itemPrefabs == null || itemPrefabs.Count == 0)
        {
            EditorGUILayout.HelpBox("No item prefabs found in Resources/Objects. Please ensure your item prefabs are in a subfolder named 'Objects' within any 'Resources' folder, and have a 'NewItemScript' component.", MessageType.Warning);
            return;
        }

        // Add a Refresh button to reload prefabs from Resources
        if (GUILayout.Button("Refresh Prefabs"))
        {
            LoadItemPrefabsFromResources();
            if (itemPrefabs.Count > 0)
            {
                currentIndex = 0; // Reset to the first item after refresh
                LoadCurrentPrefab();
            }
            else
            {
                currentPrefab = null;
                currentItemScript = null;
                if (prefabEditor != null) DestroyImmediate(prefabEditor);
            }
        }

        EditorGUILayout.Space();

        // Dropdown for selecting prefabs by name
        int selectedIndex = EditorGUILayout.Popup("Select Item", currentIndex, itemPrefabNames.ToArray());
        if (selectedIndex != currentIndex)
        {
            currentIndex = selectedIndex;
            LoadCurrentPrefab();
        }

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Previous"))
        {
            currentIndex--;
            if (currentIndex < 0)
            {
                currentIndex = itemPrefabs.Count - 1;
            }
            LoadCurrentPrefab();
        }

        if (GUILayout.Button("Next"))
        {
            currentIndex++;
            if (currentIndex >= itemPrefabs.Count)
            {
                currentIndex = 0;
            }
            LoadCurrentPrefab();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        // Start a scroll view for the prefab inspector
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);

        if (currentPrefab != null && currentItemScript != null)
        {
            GUI.enabled = false; // Disable editing of the GameObject field
            EditorGUILayout.ObjectField("Current Prefab", currentPrefab, typeof(GameObject), false);
            GUI.enabled = true;

            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Item Data", EditorStyles.boldLabel);

            // Using SerializedObject and SerializedProperty for proper Undo/Redo and prefab saving
            SerializedObject serializedObject = new SerializedObject(currentItemScript);
            SerializedProperty itemDataProperty = serializedObject.FindProperty("itemData");

            EditorGUILayout.PropertyField(itemDataProperty, true); // True to draw children

            serializedObject.ApplyModifiedProperties(); // Apply changes to the script

            // If you want to show the full Inspector for the prefab
            if (prefabEditor == null)
            {
                prefabEditor = Editor.CreateEditor(currentPrefab);
            }
            if (prefabEditor != null)
            {
                EditorGUILayout.Space();
                GUILayout.Label("Prefab Inspector", EditorStyles.boldLabel);
                // Draw the default inspector for the selected prefab
                prefabEditor.OnInspectorGUI();
            }

            // Apply changes to the prefab asset
            if (GUI.changed)
            {
                EditorUtility.SetDirty(currentItemScript);
                PrefabUtility.RecordPrefabInstancePropertyModifications(currentItemScript); // Essential for prefab changes
                AssetDatabase.SaveAssets(); // Ensure assets are saved
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Select a prefab to edit.", MessageType.Info);
        }

        EditorGUILayout.EndScrollView(); // End the scroll view
    }

    private void LoadItemPrefabsFromResources()
    {
        itemPrefabs = new List<GameObject>();
        itemPrefabNames = new List<string>(); // Initialize the list of names

        // Load all GameObjects from the "Objects" folder within any Resources folder
        // Note: If you want to go deeper, e.g., "Objects/Items", change the path here.
        Object[] loadedObjects = Resources.LoadAll("Objects", typeof(GameObject));

        foreach (Object obj in loadedObjects)
        {
            GameObject prefab = obj as GameObject;
            if (prefab != null && prefab.GetComponent<NewItemScript>() != null)
            {
                itemPrefabs.Add(prefab);
                itemPrefabNames.Add(prefab.name); // Add the prefab's name to the list
            }
            else if (prefab != null)
            {
                Debug.LogWarning($"Prefab '{prefab.name}' in Resources/Objects does not have a NewItemScript and will not be displayed in the editor tool.");
            }
        }

        // Sort items by name for consistent display in the dropdown
        // This requires sorting both lists in parallel to maintain correspondence
        SortItemPrefabsByName();

        if (itemPrefabs.Count > 0)
        {
            // Ensure currentIndex is valid after loading/sorting
            currentIndex = Mathf.Clamp(currentIndex, 0, itemPrefabs.Count - 1);
        }
        else
        {
            currentIndex = 0; // No items found, reset index
        }
    }

    private void SortItemPrefabsByName()
    {
        // Create a list of anonymous objects with both prefab and name
        List<System.Tuple<string, GameObject>> combinedList = new List<System.Tuple<string, GameObject>>();
        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            combinedList.Add(System.Tuple.Create(itemPrefabNames[i], itemPrefabs[i]));
        }

        // Sort the combined list by name
        combinedList.Sort((a, b) => a.Item1.CompareTo(b.Item1));

        // Re-populate the original lists
        itemPrefabs.Clear();
        itemPrefabNames.Clear();
        foreach (var tuple in combinedList)
        {
            itemPrefabNames.Add(tuple.Item1);
            itemPrefabs.Add(tuple.Item2);
        }
    }


    private void LoadCurrentPrefab()
    {
        if (itemPrefabs != null && itemPrefabs.Count > 0)
        {
            currentPrefab = itemPrefabs[currentIndex];
            currentItemScript = currentPrefab.GetComponent<NewItemScript>();

            // Destroy the old editor if it exists
            if (prefabEditor != null)
            {
                DestroyImmediate(prefabEditor);
            }
            // Create a new editor for the newly selected prefab
            prefabEditor = Editor.CreateEditor(currentPrefab);
        }
        else
        {
            currentPrefab = null;
            currentItemScript = null;
            if (prefabEditor != null)
            {
                DestroyImmediate(prefabEditor);
            }
        }
        Repaint(); // Redraw the window to show the new prefab's data
    }

    private void OnDisable()
    {
        // Clean up the created Editor instance
        if (prefabEditor != null)
        {
            DestroyImmediate(prefabEditor);
        }
    }
}

            

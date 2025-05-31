// MergeableItemDataDrawer.cs
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq; // For .Distinct() and .OrderBy()
using System; // For StringSplitOptions

[CustomPropertyDrawer(typeof(MergeableItemData))]
public class MergeableItemDataDrawer : PropertyDrawer
{
    // Caching serialized properties for performance
    private SerializedProperty obj1TagProperty;
    private SerializedProperty obj2TagProperty;
    private SerializedProperty resultPrefabProperty;

    // Height of a single line in the Inspector
    private static readonly float SingleLineHeight = EditorGUIUtility.singleLineHeight;
    private const float Padding = 2f;

    // Store a reference to the parent MergeDictionary's serialized object
    // and its MergeDataList to check for global duplicates.
    private static SerializedProperty parentMergeDataListProperty = null;
    private static Dictionary<GameObject, int> globalPrefabCounts = new Dictionary<GameObject, int>();

    // This method is called to draw the custom GUI for the MergeableItemData struct
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the properties of MergeableItemData
        obj1TagProperty = property.FindPropertyRelative("Obj1Tag");
        obj2TagProperty = property.FindPropertyRelative("Obj2Tag");
        resultPrefabProperty = property.FindPropertyRelative("ResultPrefab");

        // Set the current Y position for drawing elements
        float currentY = position.y;

        // Use BeginProperty / EndProperty to ensure prefab modifications are properly recorded
        EditorGUI.BeginProperty(position, label, property);

        // --- CUSTOM LABEL FOR THE DROPDOWN ---
        // Create a new label based on the ResultPrefab's name.
        // If no prefab is assigned, fall back to a default "New Merge Rule" or the original label.
        GUIContent customLabel;
        if (resultPrefabProperty.objectReferenceValue != null)
        {
            customLabel = new GUIContent(resultPrefabProperty.objectReferenceValue.name);
        }
        else
        {
            // If the prefab isn't assigned yet, show a more informative label.
            // You can also fall back to the original label if preferred: `label`
            customLabel = new GUIContent("New Merge Rule (No Prefab)");
        }

        // Draw a foldout for the list element itself, making it collapsible
        // Use our customLabel here
        property.isExpanded = EditorGUI.Foldout(new Rect(position.x, currentY, position.width, SingleLineHeight), property.isExpanded, customLabel);
        currentY += SingleLineHeight + Padding;

        if (property.isExpanded)
        {
            // Indent all the fields inside the foldout
            EditorGUI.indentLevel++;

            // --- Multi-string Input for Obj1Tag and Obj2Tag ---
            // Draw a clear label for the multi-string input area
            EditorGUI.LabelField(new Rect(position.x, currentY, position.width, SingleLineHeight), new GUIContent("Input Tags (e.g., TagA, TagB)"));
            currentY += SingleLineHeight + Padding;

            Rect tagsTextAreaRect = new Rect(position.x, currentY, position.width, SingleLineHeight * 2f); // Taller for TextArea
            string currentTagsCombined = $"{obj1TagProperty.stringValue}, {obj2TagProperty.stringValue}".Trim().TrimStart(','); // Combine existing, clean up leading comma if empty
            if (string.IsNullOrEmpty(obj2TagProperty.stringValue)) // If only one tag, remove the trailing comma
            {
                currentTagsCombined = obj1TagProperty.stringValue;
            }

            string newTagsInput = EditorGUI.TextArea(tagsTextAreaRect, currentTagsCombined);

            // If the text has changed, parse and update the properties
            if (newTagsInput != currentTagsCombined)
            {
                string[] parsedTags = newTagsInput.Split(new char[] { ',', '\n' }, StringSplitOptions.RemoveEmptyEntries)
                                                 .Select(s => s.Trim())
                                                 .ToArray();

                if (parsedTags.Length > 0)
                {
                    obj1TagProperty.stringValue = parsedTags[0];
                    if (parsedTags.Length > 1)
                    {
                        obj2TagProperty.stringValue = parsedTags[1];
                    }
                    else
                    {
                        obj2TagProperty.stringValue = ""; // Clear Obj2Tag if only one tag entered
                    }
                }
                else
                {
                    obj1TagProperty.stringValue = "";
                    obj2TagProperty.stringValue = "";
                }
            }
            currentY += SingleLineHeight * 2f + Padding; // Adjust height for TextArea

            // Display current tags directly below for clarity
            EditorGUI.LabelField(new Rect(position.x, currentY, position.width, SingleLineHeight), $"Obj1 Tag: {obj1TagProperty.stringValue}");
            currentY += SingleLineHeight + Padding;
            EditorGUI.LabelField(new Rect(position.x, currentY, position.width, SingleLineHeight), $"Obj2 Tag: {obj2TagProperty.stringValue}");
            currentY += SingleLineHeight + Padding;


            // --- Result Prefab Field with Duplicate Highlighting ---
            Color originalGUIColor = GUI.color; // Store original GUI color

            // Ensure parentMergeDataListProperty is correctly referenced.
            // This needs to be done once per OnGUI call to ensure it's always valid for the current SerializedObject.
            if (parentMergeDataListProperty == null || parentMergeDataListProperty.serializedObject != property.serializedObject || !parentMergeDataListProperty.isArray)
            {
                parentMergeDataListProperty = property.serializedObject.FindProperty("MergeDataList");
            }

            // Recalculate counts if the list has changed (important for visual updates)
            UpdateGlobalCounts();

            GameObject currentPrefab = resultPrefabProperty.objectReferenceValue as GameObject;
            bool isDuplicatePrefab = currentPrefab != null && globalPrefabCounts.ContainsKey(currentPrefab) && globalPrefabCounts[currentPrefab] > 1;

            if (isDuplicatePrefab)
            {
                GUI.color = Color.red; // Highlight duplicate prefabs in red
            }

            // Draw the Result Prefab field
            EditorGUI.PropertyField(new Rect(position.x, currentY, position.width, SingleLineHeight), resultPrefabProperty);
            currentY += SingleLineHeight + Padding;

            GUI.color = originalGUIColor; // Reset GUI color

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    // This method is called to calculate the total height of the custom GUI
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Initial height for the foldout itself
        float totalHeight = SingleLineHeight + Padding;

        if (property.isExpanded)
        {
            // Add height for the "Input Tags" label
            totalHeight += SingleLineHeight + Padding;
            // Add height for the multi-string input area (TextArea)
            totalHeight += SingleLineHeight * 2f + Padding;
            // Add height for displaying Obj1 Tag and Obj2 Tag labels
            totalHeight += (SingleLineHeight + Padding) * 2;
            // Add height for the Result Prefab field
            totalHeight += SingleLineHeight + Padding;
        }
        return totalHeight;
    }

    // Helper method to update global counts of prefabs (no longer counting keys here)
    private void UpdateGlobalCounts()
    {
        globalPrefabCounts.Clear();

        if (parentMergeDataListProperty != null && parentMergeDataListProperty.isArray)
        {
            for (int i = 0; i < parentMergeDataListProperty.arraySize; i++)
            {
                SerializedProperty element = parentMergeDataListProperty.GetArrayElementAtIndex(i);
                SerializedProperty prefab = element.FindPropertyRelative("ResultPrefab");

                // Count prefabs for duplicate highlighting
                GameObject currentPrefab = prefab.objectReferenceValue as GameObject;
                if (currentPrefab != null)
                {
                    if (globalPrefabCounts.ContainsKey(currentPrefab))
                    {
                        globalPrefabCounts[currentPrefab]++;
                    }
                    else
                    {
                        globalPrefabCounts.Add(currentPrefab, 1);
                    }
                }
            }
        }
    }
}
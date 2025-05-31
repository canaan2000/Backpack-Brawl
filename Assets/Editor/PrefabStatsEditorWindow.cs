using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq; // For LINQ operations

public class PrefabStatsEditorWindow : EditorWindow
{
    private List<GameObject> loadedPrefabs = new List<GameObject>();
    private Vector2 scrollPosition;
    private string searchString = "";
    private GameObject selectedPrefab; // For single prefab detailed view
    private List<GameObject> multiSelectedPrefabs = new List<GameObject>(); // For batch editing

    // Define the type of your prefab's stat component (now NewItemScript)
    private System.Type statComponentType = typeof(NewItemScript);

    [MenuItem("Window/Item Stats Editor")]
    public static void ShowWindow()
    {
        GetWindow<PrefabStatsEditorWindow>("Item Stats Editor");
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();

        // --- Prefab Loading/Selection ---
        EditorGUILayout.LabelField("Load Item Prefabs", EditorStyles.boldLabel);

        // Drag and Drop Area
        Event currentEvent = Event.current;
        Rect dropArea = GUILayoutUtility.GetRect(0.0f, 50.0f, GUILayout.ExpandWidth(true));
        GUI.Box(dropArea, "Drag & Drop Item Prefabs Here");

        if (currentEvent.type == EventType.DragUpdated && dropArea.Contains(currentEvent.mousePosition))
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            currentEvent.Use();
        }
        else if (currentEvent.type == EventType.DragPerform && dropArea.Contains(currentEvent.mousePosition))
        {
            DragAndDrop.AcceptDrag();
            foreach (Object draggedObject in DragAndDrop.objectReferences)
            {
                if (draggedObject is GameObject go && PrefabUtility.IsPartOfPrefabAsset(go))
                {
                    // Ensure the prefab has the NewItemScript component
                    if (go.GetComponent<NewItemScript>() != null)
                    {
                        if (!loadedPrefabs.Contains(go))
                        {
                            loadedPrefabs.Add(go);
                        }
                    }
                    else
                    {
                        Debug.LogWarning($"Prefab '{go.name}' skipped: No 'NewItemScript' component found.");
                    }
                }
            }
            currentEvent.Use();
            Repaint(); // Redraw the window
        }

        // Search Bar
        EditorGUILayout.Space();
        searchString = EditorGUILayout.TextField("Search Items:", searchString);

        EditorGUILayout.Space();

        // --- Prefab List ---
        EditorGUILayout.LabelField("Loaded Item Prefabs", EditorStyles.boldLabel);
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(200));

        List<GameObject> filteredPrefabs = loadedPrefabs
            .Where(p => string.IsNullOrEmpty(searchString) || p.name.ToLower().Contains(searchString.ToLower()))
            .ToList();

        foreach (GameObject prefab in filteredPrefabs)
        {
            EditorGUILayout.BeginHorizontal();
            bool isSelected = multiSelectedPrefabs.Contains(prefab);

            // Allow multi-selection using toggles
            bool newSelection = EditorGUILayout.Toggle(isSelected, GUILayout.Width(20));
            if (newSelection != isSelected)
            {
                if (newSelection)
                {
                    multiSelectedPrefabs.Add(prefab);
                }
                else
                {
                    multiSelectedPrefabs.Remove(prefab);
                }
            }

            // Button to select single item for detailed view
            if (GUILayout.Button(prefab.name, EditorStyles.label))
            {
                selectedPrefab = prefab; // Set for single-item detail view
                Selection.activeObject = prefab; // Select in Project window
            }

            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space();
        EditorGUILayout.HelpBox("Select one or more prefabs above to edit. Use the checkboxes for batch editing, or click a name for detailed single-item editing.", MessageType.Info);
        EditorGUILayout.Space();

        // --- Batch Editing Section ---
        if (multiSelectedPrefabs.Any())
        {
            EditorGUILayout.LabelField("Batch Edit Selected Items (" + multiSelectedPrefabs.Count + ")", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");

            // Example: Batch edit 'damage' for all selected items
            float currentBatchDamage = 0f;
            bool mixedDamage = false;

            // Get initial value and check for mixed values
            if (multiSelectedPrefabs.Count > 0 && multiSelectedPrefabs[0].TryGetComponent<NewItemScript>(out NewItemScript firstScript))
            {
                currentBatchDamage = firstScript.itemData.damage;
                for (int i = 1; i < multiSelectedPrefabs.Count; i++)
                {
                    if (multiSelectedPrefabs[i].TryGetComponent<NewItemScript>(out NewItemScript otherScript))
                    {
                        if (otherScript.itemData.damage != currentBatchDamage)
                        {
                            mixedDamage = true;
                            break;
                        }
                    }
                }
            }

            EditorGUI.BeginChangeCheck();
            float newBatchDamage;
            if (mixedDamage)
            {
                // Display a mixed value indicator
                EditorGUI.showMixedValue = true;
                newBatchDamage = EditorGUILayout.FloatField("Damage (Mixed)", 0f); // Default value doesn't matter much when mixed
            }
            else
            {
                newBatchDamage = EditorGUILayout.FloatField("Damage", currentBatchDamage);
            }
            EditorGUI.showMixedValue = false; // Reset mixed value flag

            if (EditorGUI.EndChangeCheck())
            {
                // Apply batch damage
                foreach (GameObject prefabGO in multiSelectedPrefabs)
                {
                    if (prefabGO.TryGetComponent<NewItemScript>(out NewItemScript itemScript))
                    {
                        Undo.RecordObject(itemScript, "Change Item Damage (Batch)"); // For Undo/Redo
                        itemScript.itemData.damage = newBatchDamage;
                        EditorUtility.SetDirty(itemScript); // Mark prefab dirty to save changes
                        AssetDatabase.SaveAssets();
                    }
                }
                Debug.Log($"Batch damage set to {newBatchDamage} for {multiSelectedPrefabs.Count} items.");
            }

            // Example: Batch edit 'rarity' for all selected items
            NewItemScript.ItemClass.Rarity currentBatchRarity = NewItemScript.ItemClass.Rarity.Common;
            bool mixedRarity = false;

            if (multiSelectedPrefabs.Count > 0 && multiSelectedPrefabs[0].TryGetComponent<NewItemScript>(out firstScript))
            {
                currentBatchRarity = firstScript.itemData.rarity;
                for (int i = 1; i < multiSelectedPrefabs.Count; i++)
                {
                    if (multiSelectedPrefabs[i].TryGetComponent<NewItemScript>(out NewItemScript otherScript))
                    {
                        if (otherScript.itemData.rarity != currentBatchRarity)
                        {
                            mixedRarity = true;
                            break;
                        }
                    }
                }
            }

            EditorGUI.BeginChangeCheck();
            NewItemScript.ItemClass.Rarity newBatchRarity;
            if (mixedRarity)
            {
                EditorGUI.showMixedValue = true;
                newBatchRarity = (NewItemScript.ItemClass.Rarity)EditorGUILayout.EnumPopup("Rarity (Mixed)", NewItemScript.ItemClass.Rarity.Common);
            }
            else
            {
                newBatchRarity = (NewItemScript.ItemClass.Rarity)EditorGUILayout.EnumPopup("Rarity", currentBatchRarity);
            }
            EditorGUI.showMixedValue = false;

            if (EditorGUI.EndChangeCheck())
            {
                foreach (GameObject prefabGO in multiSelectedPrefabs)
                {
                    if (prefabGO.TryGetComponent<NewItemScript>(out NewItemScript itemScript))
                    {
                        Undo.RecordObject(itemScript, "Change Item Rarity (Batch)");
                        itemScript.itemData.rarity = newBatchRarity;
                        EditorUtility.SetDirty(itemScript);
                        AssetDatabase.SaveAssets();
                    }
                }
                Debug.Log($"Batch rarity set to {newBatchRarity} for {multiSelectedPrefabs.Count} items.");
            }

            // You can add more batch editable properties here following the same pattern.
            // For a more comprehensive batch editor, you'd iterate through properties of itemData
            // and show common ones. This is more involved and might require a custom SerializedProperty drawer.

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.Space();

        // --- Single Prefab Detail View ---
        if (selectedPrefab != null)
        {
            EditorGUILayout.LabelField("Selected Item Details: " + selectedPrefab.name, EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box"); // Outer box for the entire details section

            if (selectedPrefab.TryGetComponent<NewItemScript>(out NewItemScript itemScript))
            {
                SerializedObject so = new SerializedObject(itemScript);
                SerializedProperty itemDataProp = so.FindProperty("itemData");

                if (itemDataProp != null)
                {
                    EditorGUI.BeginChangeCheck();

                    // --- Layout for ItemClass properties ---
                    EditorGUILayout.LabelField("Item Data:", EditorStyles.boldLabel);

                    // Name and Description
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("name"));
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("description"));


                    // Grouping float stats (2 per row)
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("damage"));
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("armor"));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("poison"));
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("thorns"));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("autoManaGain"));
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("autoStaminaUsage"));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("staminaUsage"));
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("clickManaUsage"));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("clickHealing"));
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("clickHunger"));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("clickArmor"));
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("clickDamage"));
                    EditorGUILayout.EndHorizontal();

                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("clickPoison"));
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("clickThorns"));
                    EditorGUILayout.EndHorizontal();


                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("value"));
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("singleUse"));
                    EditorGUILayout.EndHorizontal();

                    // Enums and Bool (can be grouped as well)
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("rarity"));
                    EditorGUILayout.PropertyField(itemDataProp.FindPropertyRelative("itemClass"));
                    EditorGUILayout.EndHorizontal();


                    if (EditorGUI.EndChangeCheck())
                    {
                        so.ApplyModifiedProperties(); // Apply changes back to the prefab asset
                        EditorUtility.SetDirty(itemScript); // Mark the component/prefab dirty
                        AssetDatabase.SaveAssets(); // Save assets to disk
                        AssetDatabase.Refresh(); // Refresh the asset database to show changes
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("Could not find 'itemData' property on NewItemScript.", MessageType.Error);
                }

                // Example: Add a button to reset this specific item's stats (implement reset logic)
                if (GUILayout.Button("Reset " + selectedPrefab.name + " Stats"))
                {
                    if (EditorUtility.DisplayDialog("Confirm Reset", "Are you sure you want to reset " + selectedPrefab.name + "'s stats to default?", "Yes", "No"))
                    {
                        Undo.RecordObject(itemScript, "Reset Item Stats");
                        itemScript.itemData = new NewItemScript.ItemClass(); // Recreate and use default values
                        // Manually set default values as done in your Start() method
                        itemScript.itemData.name = "Default Item Name";
                        itemScript.itemData.description = "Default Description";
                        itemScript.itemData.damage = 0f;
                        itemScript.itemData.armor = 0f;
                        itemScript.itemData.poison = 0f;
                        itemScript.itemData.thorns = 0f;
                        itemScript.itemData.autoManaGain = 0f;
                        itemScript.itemData.autoStaminaUsage = 0f;
                        itemScript.itemData.staminaUsage = 0f;
                        itemScript.itemData.clickManaUsage = 0f;
                        itemScript.itemData.clickHealing = 0f;
                        itemScript.itemData.clickArmor = 0f;
                        itemScript.itemData.clickDamage = 0f;
                        itemScript.itemData.clickPoison = 0f;
                        itemScript.itemData.clickThorns = 0f;
                        itemScript.itemData.value = 0f;
                        itemScript.itemData.singleUse = false;
                        itemScript.itemData.rarity = NewItemScript.ItemClass.Rarity.Common;
                        itemScript.itemData.itemClass = NewItemScript.ItemClass.Class.Basic;

                        EditorUtility.SetDirty(itemScript);
                        AssetDatabase.SaveAssets();
                        AssetDatabase.Refresh();
                        Debug.Log("Resetting stats for " + selectedPrefab.name);
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Selected prefab does not have a 'NewItemScript' component.", MessageType.Warning);
            }
            EditorGUILayout.EndVertical(); // End outer box
        }

        EditorGUILayout.EndVertical();

        // Ensure the editor window updates when changes are made
        if (GUI.changed)
        {
            Repaint();
        }
    }
}
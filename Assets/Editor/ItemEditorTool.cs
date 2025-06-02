using UnityEditor;
using UnityEngine;
using System.Collections.Generic; // Required for List

public class ItemEditorTool : EditorWindow
{
    private List<GameObject> itemPrefabs;
    private int currentIndex = 0;
    private GameObject currentPrefab;
    private NewItemScript currentItemScript;

    // Add an Editor reference for the current prefab's inspector
    private Editor prefabEditor;

    [MenuItem("Tools/Item Editor")]
    public static void ShowWindow()
    {
        GetWindow<ItemEditorTool>("Item Editor");
    }

    private void OnEnable()
    {
        LoadItemPrefabsFromResources(); // Modified to load from Resources
    }

    private void OnGUI()
    {
        GUILayout.Label("Item Prefab Editor", EditorStyles.boldLabel);

        if (itemPrefabs == null || itemPrefabs.Count == 0)
        {
            EditorGUILayout.HelpBox("No item prefabs found in Resources/Items. Please ensure your item prefabs are in a subfolder named 'Items' within any 'Resources' folder.", MessageType.Warning);
            return;
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
                prefabEditor.OnInspectorGUI();
            }

            // Apply changes to the prefab asset
            if (GUI.changed)
            {
                // This part needs adjustment for Resources.LoadAll, as it loads runtime instances
                // For editor tools, it's generally better to work directly with the asset database
                // when modifying prefabs. However, if you *must* use Resources.LoadAll,
                // you'll need to save changes back to the *original asset*.
                // The current approach with SerializedObject/SerializedProperty and AssetDatabase.SaveAssets()
                // already works with prefab assets, even if the loading mechanism changes.
                // The key is that the 'currentPrefab' *is* the asset loaded via Resources.LoadAll.

                // If you were modifying a scene object and then wanted to apply it back to a prefab,
                // you'd use PrefabUtility.SaveAsPrefabAsset or PrefabUtility.ApplyPrefabInstance.
                // Here, since currentPrefab is loaded from Resources (which means it's a direct asset reference),
                // the existing saving mechanism is generally okay.
                EditorUtility.SetDirty(currentItemScript);
                PrefabUtility.RecordPrefabInstancePropertyModifications(currentItemScript); // Essential for prefab changes
                AssetDatabase.SaveAssets(); // Ensure assets are saved
            }
        }
        else
        {
            EditorGUILayout.HelpBox("Select a prefab to edit.", MessageType.Info);
        }
    }

    // New method to load from Resources
    private void LoadItemPrefabsFromResources()
    {
        itemPrefabs = new List<GameObject>();

        // Load all GameObjects from the "Items" subfolder within any Resources folder
        // The path is relative to any Resources folder.
        Object[] loadedObjects = Resources.LoadAll("Objects", typeof(GameObject));

        foreach (Object obj in loadedObjects)
        {
            GameObject prefab = obj as GameObject;
            if (prefab != null && prefab.GetComponent<NewItemScript>() != null)
            {
                itemPrefabs.Add(prefab);
            }
        }

        if (itemPrefabs.Count > 0)
        {
            currentIndex = 0;
            LoadCurrentPrefab();
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
        Repaint(); // Redraw the window
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
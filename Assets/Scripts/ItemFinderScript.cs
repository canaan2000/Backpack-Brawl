using UnityEngine;
using UnityEditor; // Crucial for AssetDatabase
using System.Collections.Generic;
using System.Linq; // For easier LINQ operations like .Where()

public class ItemFinderScript : EditorWindow
{
    private string componentTypeName = ""; // User input for the component type name
    private System.Type targetComponentType = null; // The actual Type object once resolved

    private Vector2 scrollPos; // For scrolling if many results

    [MenuItem("Tools/Find Prefabs with Component")]
    public static void ShowWindow()
    {
        GetWindow<ItemFinderScript>("Find Prefabs by Component");
    }

    private void OnGUI()
    {
        GUILayout.Label("Find Prefabs by Component Type", EditorStyles.boldLabel);

        // Input field for the component type name
        componentTypeName = EditorGUILayout.TextField("Component Full Type Name:", componentTypeName);
        EditorGUILayout.HelpBox("Enter the full name of the component type (e.g., 'UnityEngine.Rigidbody', 'MyNamespace.MyCustomMonoBehaviour').", MessageType.Info);

        if (GUILayout.Button("Find Prefabs"))
        {
            FindPrefabsWithComponent();
        }

        EditorGUILayout.Space();

        // Display results (if any)
        if (foundPrefabs.Any())
        {
            GUILayout.Label($"Found {foundPrefabs.Count} Prefabs:", EditorStyles.boldLabel);

            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            foreach (GameObject prefab in foundPrefabs)
            {
                if (prefab != null) // Check if the asset still exists
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.ObjectField(prefab.name, prefab, typeof(GameObject), false);
                    if (GUILayout.Button("Select", GUILayout.Width(60)))
                    {
                        Selection.activeObject = prefab;
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUILayout.EndScrollView();
        }
    }

    private List<GameObject> foundPrefabs = new List<GameObject>();

    private void FindPrefabsWithComponent()
    {
        foundPrefabs.Clear(); // Clear previous results

        // 1. Resolve the Type from the string name
        targetComponentType = GetTypeFromAllAssemblies(componentTypeName);

        if (targetComponentType == null)
        {
            Debug.LogError($"Component type '{componentTypeName}' not found. Please ensure the full namespace and type name are correct.");
            return;
        }

        // Ensure the type is actually a Component (or derived from it)
        if (!typeof(Component).IsAssignableFrom(targetComponentType))
        {
            Debug.LogError($"The specified type '{componentTypeName}' is not a Unity Component. This tool can only find prefabs based on attached Components.");
            return;
        }

        // 2. Find all prefab assets in the project
        string[] prefabGuids = AssetDatabase.FindAssets("t:Prefab"); // "t:Prefab" filters by asset type Prefab

        Debug.Log($"Searching {prefabGuids.Length} prefabs for component type '{targetComponentType.Name}'...");

        // 3. Iterate through each prefab
        foreach (string guid in prefabGuids)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

            if (prefab == null)
            {
                Debug.LogWarning($"Could not load prefab at path: {assetPath}");
                continue;
            }

            // 4. Check if the prefab (or any of its children) has the target component
            // We use GetComponentsInChildren to cover cases where the component might be on a child GameObject
            // 'true' as the second argument ensures that inactive children are also checked.
            Component[] components = prefab.GetComponentsInChildren(targetComponentType, true);

            if (components != null && components.Length > 0)
            {
                foundPrefabs.Add(prefab);
            }
        }

        Debug.Log($"Finished search. Found {foundPrefabs.Count} prefabs with component '{targetComponentType.Name}'.");
    }

    /// <summary>
    /// Helper method to find a Type by its name across all loaded assemblies.
    /// This is more robust than Type.GetType() alone for finding types in different assemblies.
    /// </summary>
    private System.Type GetTypeFromAllAssemblies(string typeName)
    {
        foreach (System.Reflection.Assembly assembly in System.AppDomain.CurrentDomain.GetAssemblies())
        {
            System.Type type = assembly.GetType(typeName);
            if (type != null)
            {
                return type;
            }
        }
        return null;
    }
}
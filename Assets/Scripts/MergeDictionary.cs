using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct MergeableItemData
{
    public string Obj1Tag;
    public string Obj2Tag;
    public GameObject ResultPrefab;
}

public class MergeDictionary : MonoBehaviour
{
    public MergeMathScript MergeMath;
    public List<MergeableItemData> MergeDataList = new List<MergeableItemData>();

    // Dictionary to store mergeable item combinations and their results for faster lookup
    private Dictionary<string, GameObject> mergeDictionary = new Dictionary<string, GameObject>();

    void Start()
    {
        // Populate the dictionary in Start for efficient access
        foreach (MergeableItemData data in MergeDataList)
        {
            // Create a unique key from the combination of Obj1Tag and Obj2Tag.
            string key = data.Obj1Tag + "_" + data.Obj2Tag;
            if (!mergeDictionary.ContainsKey(key))
            {
                mergeDictionary.Add(key, data.ResultPrefab);
            }
            else
            {
                Debug.LogWarning("Duplicate tag combination '" + key + "' in MergeDataList. Only the first entry will be used.");
            }
        }
    }

    public void CheckMerge(GameObject obj1, GameObject obj2)
    {
        // Create keys for both possible tag combinations
        string key1 = obj1.tag + "_" + obj2.tag;
        string key2 = obj2.tag + "_" + obj1.tag;

        // Check if either key exists in the dictionary
        if (mergeDictionary.ContainsKey(key1))
        {
            MergeMath.Merge(obj1, obj2, mergeDictionary[key1], mergeDictionary[key1].name);
            return;
        }
        else if (mergeDictionary.ContainsKey(key2))
        {
            MergeMath.Merge(obj1, obj2, mergeDictionary[key2], mergeDictionary[key2].name);
            return;
        }
        // If no matching combination is found. log a message
        Debug.Log("No matching merge combination found for " + obj1.tag + " and " + obj2.tag);
    }
}

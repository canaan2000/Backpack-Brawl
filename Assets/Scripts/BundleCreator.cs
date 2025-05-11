using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // To easily shuffle lists

public class BundleCreator : MonoBehaviour
{
    // Public list to hold all the GameObjects that can be part of a bundle.
    public List<GameObject> AllObjects;

    // Internal lists to categorize GameObjects based on their rarity.
    private List<GameObject> common;
    private List<GameObject> uncommon;
    private List<GameObject> rare;

    float commonPercentage = 90f;
    float uncommonPercentage = 50f;
    float rarePercentage = 10f;

    // Public list of lists to hold the three generated bundle options.
    public List<List<GameObject>> options = new List<List<GameObject>>();

    // Start is called before the first frame update.
    void Start()
    {
        // Initialize the rarity lists.
        common = new List<GameObject>();
        uncommon = new List<GameObject>();
        rare = new List<GameObject>();

        // Categorize all objects by their rarity.
        foreach (GameObject obj in AllObjects)
        {
            NewItemScript itemScript = obj.GetComponent<NewItemScript>();

            if (itemScript != null && itemScript.itemData != null)
            {
                switch (itemScript.itemData.rarity)
                {
                    case NewItemScript.ItemClass.Rarity.Common:
                        common.Add(obj);
                        break;
                    case NewItemScript.ItemClass.Rarity.Uncommon:
                        uncommon.Add(obj);
                        break;
                    case NewItemScript.ItemClass.Rarity.Rare:
                        rare.Add(obj);
                        break;
                }
            }
            else
            {
                Debug.LogWarning("GameObject " + obj.name + " is missing NewItemScript or its ItemData.");
            }
        }

        // Create the three bundle options.
        CreateOptions();

        // Log the contents of each generated option.
        Debug.Log("Generated Options:");
        for (int i = 0; i < options.Count; i++)
        {
            Debug.Log($"Option {i + 1}:");
            foreach (GameObject item in options[i])
            {
                Debug.Log($"- {item.name} ({GetRarity(item)}) - Value: ${item.GetComponent<NewItemScript>().itemData.value:F2}");
            }
            Debug.Log("---");
        }
    }

    // Helper function to get the rarity of a GameObject.
    private string GetRarity(GameObject obj)
    {
        NewItemScript itemScript = obj.GetComponent<NewItemScript>();
        if (itemScript != null && itemScript.itemData != null)
        {
            return itemScript.itemData.rarity.ToString();
        }
        return "Unknown Rarity";
    }

    // Method to create the three bundle options.
    public void CreateOptions()
    {
        options.Clear(); // Clear any existing options

        for (int i = 0; i < 3; i++)
        {
            List<GameObject> newOption = GenerateBundle();
            if (newOption != null)
            {
                options.Add(newOption);
            }
            else
            {
                Debug.LogError($"Failed to generate option {i + 1}. Check if there are enough items of each rarity.");
            }
        }
    }

    // Method to generate a single bundle based on the rarity specifications with random counts for each bundle.
    private List<GameObject> GenerateBundle()
    {
        List<GameObject> bundle = new List<GameObject>();
        List<GameObject> availableCommon = common.ToList();
        List<GameObject> availableUncommon = uncommon.ToList();
        List<GameObject> availableRare = rare.ToList();

        // Pick a random number of common items (between 2 and 5).
        int numCommon = Random.Range(2, Mathf.Min(5, availableCommon.Count + 1));
        for (int i = 0; i < numCommon; i++)
        {
            if (availableCommon.Count > 0)
            {
                int randomIndex = Random.Range(0, availableCommon.Count);
                bundle.Add(availableCommon[randomIndex]);
                availableCommon.RemoveAt(randomIndex);
            }
            else
            {
                Debug.LogWarning("Not enough common items to fulfill the requirement for a bundle.");
                return null;
            }
        }

        // Pick a random number of uncommon items (between 1 and 3).
        int numUncommon = Random.Range(0, Mathf.Min(4, availableUncommon.Count + 1));
        for (int i = 0; i < numUncommon; i++)
        {
            if (availableUncommon.Count > 0)
            {
                int randomIndex = Random.Range(0, availableUncommon.Count);
                bundle.Add(availableUncommon[randomIndex]);
                availableUncommon.RemoveAt(randomIndex);
            }
            else
            {
                Debug.LogWarning("Not enough uncommon items to fulfill the requirement for a bundle.");
                return null;
            }
        }

        // Pick a random number of rare items (either 0 or 1).
        int numRare = Random.Range(0, Mathf.Min(2, availableRare.Count + 1));
        for (int i = 0; i < numRare; i++)
        {
            if (availableRare.Count > 0)
            {
                int randomIndex = Random.Range(0, availableRare.Count);
                bundle.Add(availableRare[randomIndex]);
                availableRare.RemoveAt(randomIndex);
            }
            // It's okay if there are no rare items
        }

        return bundle;
    }
}
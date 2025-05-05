using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleCreator : MonoBehaviour
{
    // Public list to hold all the GameObjects that can be part of a bundle.
    public List<GameObject> AllObjects;

    // Public lists to categorize GameObjects based on their rarity.
    public List<GameObject> common;
    public List<GameObject> uncommon;
    public List<GameObject> rare;

    // Public lists to represent different options or bundles of GameObjects.
    public List<GameObject> option1;
    public List<GameObject> option2;
    public List<GameObject> option3;

    // Public list of lists to hold all the option lists together.
    public List<List<GameObject>> options;

    // Public integer to define the desired size of each bundle/option.
    public int bundleSize = 5;

    // Start is called before the first frame update. This method is executed once when the script starts.
    void Start()
    {
        // Iterate through each GameObject in the AllObjects list.
        foreach (GameObject obj in AllObjects)
        {
            // Get the NewItemScript component attached to the current GameObject.
            NewItemScript itemScript = obj.GetComponent<NewItemScript>();

            // If the GameObject has a NewItemScript component and its itemData is not null.
            if (itemScript != null && itemScript.itemData != null)
            {
                // Check if the rarity of the item is Common.
                if (itemScript.itemData.rarity == NewItemScript.ItemClass.Rarity.Common)
                {
                    // Add the current GameObject to the common list.
                    common.Add(obj);
                }
                // Check if the rarity of the item is Uncommon.
                if (itemScript.itemData.rarity == NewItemScript.ItemClass.Rarity.Uncommon)
                {
                    // Add the current GameObject to the uncommon list.
                    uncommon.Add(obj);
                }
                // Check if the rarity of the item is Rare.
                if (itemScript.itemData.rarity == NewItemScript.ItemClass.Rarity.Rare)
                {
                    // Add the current GameObject to the rare list.
                    rare.Add(obj);
                }
            }
            // If the NewItemScript or its itemData is missing, log a warning.
            else
            {
                Debug.LogWarning("GameObject " + obj.name + " is missing NewItemScript or its ItemData.");
            }
        }
        // After categorizing all objects by rarity, call the CreateOptions method to generate bundles.
        CreateOptions();
    }

    // Update is called once per frame. This method is executed every frame.
    void Update()
    {

    }

    // Method to create the different options (bundles) of GameObjects.
    void CreateOptions()
    {
        // Clear the option lists before adding new items.  This is VERY important
        option1.Clear();
        option2.Clear();
        option3.Clear();

        // Loop 'bundleSize' number of times to add elements to each option list.
        for (int i = 0; i < bundleSize; i++)
        {
            Debug.Log("Adding Objects to bundles. Iteration: " + i); //Added iteration

            // Add a random GameObject from the common list to option1.
            if (common.Count > 0)
            {
                int randomIndex = Random.Range(0, common.Count);
                option1.Add(common[randomIndex]);
                Debug.Log($"Added {common[randomIndex].name} (Common) to option1");
            }
            else
            {
                Debug.LogWarning("Common list is empty, cannot add to option1.");
            }

            // Add a random GameObject from the uncommon list to option2.
            if (uncommon.Count > 0)
            {
                int randomIndex = Random.Range(0, uncommon.Count);
                option2.Add(uncommon[randomIndex]);
                Debug.Log($"Added {uncommon[randomIndex].name} (Uncommon) to option2");
            }
            else
            {
                Debug.LogWarning("Uncommon list is empty, cannot add to option2.");
            }

            // Add a random GameObject from the rare list to option3.
            if (rare.Count > 0)
            {
                int randomIndex = Random.Range(0, rare.Count);
                option3.Add(rare[randomIndex]);
                Debug.Log($"Added {rare[randomIndex].name} (Rare) to option3");
            }
            else
            {
                Debug.LogWarning("Rare list is empty, cannot add to option3.");
            }
        }
        //consider adding the options list population here.
        options.Clear();
        options.Add(option1);
        options.Add(option2);
        options.Add(option3);
    }
}
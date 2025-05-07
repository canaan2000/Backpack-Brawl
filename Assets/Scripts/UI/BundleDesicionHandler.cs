using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BundleDesicionHandler : MonoBehaviour
{
    public BundleCreator Bundles;
    public SpawnerScript Spawner;
    public PlayerStats PlayerStats; // Reference to the player's stats

    // Buttons
    public Button button1;
    public Button button2;
    public Button button3;

    // Button Text
    public TextMeshProUGUI button1Text;
    public TextMeshProUGUI button2Text;
    public TextMeshProUGUI button3Text;

    // Start is called before the first frame update
    void Start()
    {
        // Disables buttons initially
        HideOptions();
    }

    // Update is called once per frame
    void Update()
    {
        // No per-frame logic needed for now
    }

    public void ShowOptions()
    {
        if (Bundles.options.Count < 3)
        {
            Debug.LogError("BundleCreator does not have 3 options generated.");
            return;
        }

        // Get the three bundle options from the BundleCreator
        List<GameObject> option1Items = Bundles.options[0];
        List<GameObject> option2Items = Bundles.options[1];
        List<GameObject> option3Items = Bundles.options[2];

        // Calculate the total price for each option
        float price1 = CalculateBundlePrice(option1Items);
        float price2 = CalculateBundlePrice(option2Items);
        float price3 = CalculateBundlePrice(option3Items);

        // Create formatted text for each button
        string button1Display = $"{string.Join(", ", option1Items.Select(item => item.GetComponent<NewItemScript>().itemData.name))} ( ${price1:F2} )";
        string button2Display = $"{string.Join(", ", option2Items.Select(item => item.GetComponent<NewItemScript>().itemData.name))} ( ${price2:F2} )";
        string button3Display = $"{string.Join(", ", option3Items.Select(item => item.GetComponent<NewItemScript>().itemData.name))} ( ${price3:F2} )";

        // Enable the buttons
        button1.gameObject.SetActive(true);
        button2.gameObject.SetActive(true);
        button3.gameObject.SetActive(true);

        // Set the text for each button
        button1Text.text = button1Display;
        button2Text.text = button2Display;
        button3Text.text = button3Display;

        // Update button interactability based on player's money
        button1.interactable = PlayerStats.money >= price1;
        button2.interactable = PlayerStats.money >= price2;
        button3.interactable = PlayerStats.money >= price3;
    }

    float CalculateBundlePrice(List<GameObject> bundle)
    {
        float totalPrice = 0f;
        foreach (var item in bundle)
        {
            NewItemScript itemScript = item.GetComponent<NewItemScript>();
            if (itemScript != null && itemScript.itemData != null)
            {
                totalPrice += itemScript.itemData.value;
            }
        }
        return totalPrice;
    }

    void HideOptions()
    {
        button1.gameObject.SetActive(false);
        button2.gameObject.SetActive(false);
        button3.gameObject.SetActive(false);
    }

    public void ButtonOption1()
    {
        if (Bundles.options.Count > 0 && Bundles.options[0] != null)
        {
            float price = CalculateBundlePrice(Bundles.options[0]);
            if (PlayerStats.money >= price)
            {
                PlayerStats.money -= price;
                foreach (var item in Bundles.options[0])
                {
                    Spawner.SpawnItem(item);
                }
                HideOptions();
            }
            else
            {
                Debug.Log("Not enough money to purchase Option 1.");
                // Optionally, provide visual feedback to the player
            }
        }
        else
        {
            Debug.LogError("Option 1 is not available.");
        }
    }

    public void ButtonOption2()
    {
        if (Bundles.options.Count > 1 && Bundles.options[1] != null)
        {
            float price = CalculateBundlePrice(Bundles.options[1]);
            if (PlayerStats.money >= price)
            {
                PlayerStats.money -= price;
                foreach (var item in Bundles.options[1])
                {
                    Spawner.SpawnItem(item);
                }
                HideOptions();
            }
            else
            {
                Debug.Log("Not enough money to purchase Option 2.");
                // Optionally, provide visual feedback to the player
            }
        }
        else
        {
            Debug.LogError("Option 2 is not available.");
        }
    }

    public void ButtonOption3()
    {
        if (Bundles.options.Count > 2 && Bundles.options[2] != null)
        {
            float price = CalculateBundlePrice(Bundles.options[2]);
            if (PlayerStats.money >= price)
            {
                PlayerStats.money -= price;
                foreach (var item in Bundles.options[2])
                {
                    Spawner.SpawnItem(item);
                }
                HideOptions();
            }
            else
            {
                Debug.Log("Not enough money to purchase Option 3.");
                // Optionally, provide visual feedback to the player
            }
        }
        else
        {
            Debug.LogError("Option 3 is not available.");
        }
    }
}
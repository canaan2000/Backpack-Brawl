using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class RandomEvent
{
    public string title;
    public string button1Text;
    public string button2Text;
    public ButtonOutcome outcomeButton1;
    public ButtonOutcome outcomeButton2;
}

[System.Serializable]
public class ButtonOutcome
{
    public GameObject dropItem;
    public bool loseRequiredItem;
    public string requiredItemName;
    public int requiredMoney;
    public int requiredHealth;
    public string outcomeText; // Optional text to display after choosing this outcome
}

public class RandomEventHandler : MonoBehaviour
{
    public CollectionManager Collection; // Assign your CollectionManager script
    public InventoryList playerInventory;   // Assign your player's InventoryList script
    public PlayerStats playerStats;       // Assign your player's PlayerStats script
    public GameObject eventPanel;          // Assign the panel containing event UI
    public TextMeshProUGUI eventTitleText; // Assign in Inspector
    public Button button1;                // Assign in Inspector
    public Button button2;                // Assign in Inspector
    public TextMeshProUGUI button1Text;    // Assign the Text component of button1
    public TextMeshProUGUI button2Text;    // Assign the Text component of button2
    public Color disabledButtonColor = Color.grey; // Color to use for disabled buttons
    public Color enabledButtonColor = Color.white; // Default button color (adjust in Inspector if needed)

    public List<RandomEvent> possibleEvents;

    private RandomEvent currentEvent;

    [SerializeField]
    float eventChance = .5f;

    void Start()
    {
        HideEventUI();
    }

    // Call this function to trigger a random event
    public void TriggerRandomEvent()
    {
        ShowEventUI();

        if (possibleEvents.Count > 0 && eventPanel != null)
        {
            // Choose a random event from the list
            int randomIndex = Random.Range(0, possibleEvents.Count);
            currentEvent = possibleEvents[randomIndex];

            // Update UI elements
            if (eventTitleText != null)
            {
                eventTitleText.text = currentEvent.title;
            }

            UpdateButtonState(button1, button1Text, currentEvent.outcomeButton1);
            UpdateButtonState(button2, button2Text, currentEvent.outcomeButton2);

            // Hide the third button (assuming you only want to show two for these events)
            Button button3 = null; // Assign your third button here if needed
            if (button3 != null)
            {
                button3.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("No random events defined in the Inspector or Event Panel not assigned!");
        }
    }

    void UpdateButtonState(Button button, TextMeshProUGUI buttonText, ButtonOutcome outcome)
    {
        bool canAfford = true;
        string buttonLabel = "";

        // Check for required item
        if (!string.IsNullOrEmpty(outcome.requiredItemName))
        {
            bool hasItem = false;
            foreach (var item in playerInventory.inventoryList)
            {
                NewItemScript itemScript = item.GetComponent<NewItemScript>();
                if (itemScript != null && itemScript.itemData.name == outcome.requiredItemName)
                {
                    hasItem = true;
                    break;
                }
            }
            if (!hasItem) canAfford = false;
            buttonLabel += $"Requires: {outcome.requiredItemName}\n";
        }

        // Check for required money
        if (outcome.requiredMoney > 0)
        {
            if (playerStats != null && playerStats.money < outcome.requiredMoney)
            {
                canAfford = false;
            }
            buttonLabel += $"Money Cost: {outcome.requiredMoney.ToString("00")}\n";
        }

        // Check for required health
        if (outcome.requiredHealth > 0)
        {
            if (playerStats != null && playerStats.health < outcome.requiredHealth)
            {
                canAfford = false;
            }
            buttonLabel += $"Health Cost: {outcome.requiredHealth.ToString("00")}\n";
        }

        // Append the text from the RandomEvent data
        buttonLabel += (button == button1) ? currentEvent.button1Text : currentEvent.button2Text;

        if (button != null && buttonText != null)
        {
            button.interactable = canAfford;
            buttonText.color = canAfford ? enabledButtonColor : disabledButtonColor;
            buttonText.text = buttonLabel; // Update the button text with cost info and event text

            // Remove previous listener and add a new one based on interactability
            button.onClick.RemoveAllListeners();
            if (canAfford)
            {
                button.onClick.AddListener(() => HandleButtonClick(outcome));
            }
        }
    }

    void HandleButtonClick(ButtonOutcome outcome)
    {
        bool canProceed = true;

        // Redundant check for safety
        if (!string.IsNullOrEmpty(outcome.requiredItemName))
        {
            bool hasItem = false;
            foreach (var item in playerInventory.inventoryList)
            {
                NewItemScript itemScript = item.GetComponent<NewItemScript>();
                if (itemScript != null && itemScript.itemData.name == outcome.requiredItemName)
                {
                    hasItem = true;
                    break;
                }
            }
            if (!hasItem) canProceed = false;
        }

        if (playerStats != null && playerStats.money < outcome.requiredMoney) canProceed = false;
        if (playerStats != null && playerStats.health < outcome.requiredHealth) canProceed = false;

        if (canProceed)
        {
            // Apply the outcome
            if (outcome.dropItem != null && Collection != null)
            {
                Collection.SpawnInCollection(outcome.dropItem);
                Debug.Log($"Dropped: {outcome.dropItem.name}");
            }

            if (!string.IsNullOrEmpty(outcome.requiredItemName) && outcome.loseRequiredItem)
            {
                RemoveItemFromInventory(outcome.requiredItemName);
            }

            if (playerStats != null)
            {
                playerStats.money -= outcome.requiredMoney;
                playerStats.health -= outcome.requiredHealth;
            }
            else
            {
                Debug.LogError("PlayerStats reference is not assigned!");
            }
            ChanceForNextEvent();
        }
        else
        {
            DisplayOutcome("You cannot afford this option.");
        }
    }

    void RemoveItemFromInventory(string itemName)
    {
        for (int i = 0; i < playerInventory.inventoryList.Count; i++)
        {
            GameObject item = playerInventory.inventoryList[i];
            NewItemScript itemScript = item.GetComponent<NewItemScript>();
            if (itemScript != null && itemScript.itemData.name == itemName)
            {
                playerInventory.inventoryList.RemoveAt(i);
                Destroy(item);
                Debug.Log($"Lost: {itemName}");
                break; // Remove only one instance if multiple exist
            }
        }
    }

    void DisplayOutcome(string text)
    {
        
       HideEventUI();
        
    }

    void HideEventUI()
    {
        if (eventPanel != null)
        {
            eventPanel.SetActive(false);
        }
    }

    public void ShowEventUI()
    {
        if (eventPanel != null)
        {
            eventPanel.SetActive(true);
            if (eventTitleText != null) eventTitleText.gameObject.SetActive(true);
            if (button1 != null) button1.gameObject.SetActive(true);
            if (button2 != null) button2.gameObject.SetActive(true);
        }
    }

    public void ChanceForNextEvent()
    {
        float num = Random.Range(0, 1f);
        if (num < eventChance)
        {
            TriggerRandomEvent();
        }
        else
        {
            HideEventUI();
        }
    }
}
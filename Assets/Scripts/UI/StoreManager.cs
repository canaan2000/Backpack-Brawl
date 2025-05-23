using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Make sure you are using TextMeshPro for the button texts

public class StoreManager : MonoBehaviour
{
    // Assign your actual item prefabs/GameObjects here
    // These GameObjects MUST have a NewItemScript component attached.
    public GameObject storeCanvas;
    public List<GameObject> allAvailableStoreItems; // Items that can potentially be sold in the store
    public List<Button> storeButtons; // The 8 buttons in your scroll menu

    // Reference to PlayerStats to check money
    public PlayerStats playerStats; // Assign in Inspector

    // Reference to the CollectionManager to spawn items
    public CollectionManager collectionManager; // Assign in Inspector

    [Header("UI References")]
    public TextMeshProUGUI feedbackText; // Optional: A TextMeshProUGUI to show purchase feedback

    // Corrected: Map buttons directly to NewItemScript instances for their data
    private Dictionary<Button, NewItemScript> buttonItemMap = new Dictionary<Button, NewItemScript>();

    void Start()
    {
        if (playerStats == null)
        {
            Debug.LogError("PlayerStats is not assigned in StoreManager!");
            return;
        }
        if (collectionManager == null)
        {
            Debug.LogError("CollectionManager is not assigned in StoreManager!");
            return;
        }
    }

    // You can call this method to refresh the store inventory with new random items
    public void RefreshStore()
    {
        SetupStoreButtons();
    }

    void SetupStoreButtons()
    {
        storeCanvas.SetActive(true);

        buttonItemMap.Clear(); // Clear previous mappings
        // Activate all buttons first in case some were deactivated
        foreach (Button btn in storeButtons)
        {
            btn.gameObject.SetActive(true); // Ensure button is active before processing
            // Also clear its text temporarily if needed, though new text will overwrite
            TextMeshProUGUI btnText = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (btnText != null) btnText.text = "";
        }

        // Create a temporary list to pick items from, to avoid duplicates in one store display
        List<GameObject> tempAvailableItems = new List<GameObject>(allAvailableStoreItems);

        // Ensure we don't try to assign more items than we have buttons or available items
        int itemsToDisplay = Mathf.Min(storeButtons.Count, tempAvailableItems.Count);

        for (int i = 0; i < itemsToDisplay; i++)
        {
            Button currentButton = storeButtons[i];

            if (tempAvailableItems.Count == 0) // No more items left to pick from
            {
                currentButton.gameObject.SetActive(false); // Hide button if no more items
                continue;
            }

            int randomIndex = Random.Range(0, tempAvailableItems.Count);
            GameObject itemPrefab = tempAvailableItems[randomIndex];
            tempAvailableItems.RemoveAt(randomIndex); // Remove to prevent picking it again

            // Get the NewItemScript directly from the chosen itemPrefab
            NewItemScript itemScript = itemPrefab.GetComponent<NewItemScript>();

            if (itemScript != null) // Check if NewItemScript exists
            {
                buttonItemMap[currentButton] = itemScript; // Store the mapping to the NewItemScript

                // Get the TextMeshProUGUI component from the button's children
                TextMeshProUGUI buttonText = currentButton.GetComponentInChildren<TextMeshProUGUI>();

                if (buttonText != null)
                {
                    // Set the button text to "Item Name: Value" using properties from NewItemScript
                    buttonText.text = $"{itemScript.itemData.name}: {itemScript.itemData.value}"; // <--- Changed to itemScript.value
                }
                else
                {
                    Debug.LogWarning($"Button {currentButton.name} does not have a TextMeshProUGUI child!");
                }

                // Add a listener to the button's click event
                currentButton.onClick.RemoveAllListeners(); // Clear existing listeners
                currentButton.onClick.AddListener(() => PurchaseItem(currentButton));
            }
            else
            {
                Debug.LogWarning($"Item GameObject {itemPrefab.name} is missing NewItemScript!");
                currentButton.gameObject.SetActive(false); // Hide button if no NewItemScript
            }
        }

        // Deactivate any remaining unused buttons (if allAvailableStoreItems.Count < storeButtons.Count)
        for (int i = itemsToDisplay; i < storeButtons.Count; i++)
        {
            storeButtons[i].gameObject.SetActive(false);
        }

        // Update button states immediately after setup
        UpdateAllButtonStates();
    }

    // Call this whenever player money changes or store state needs to be refreshed
    public void UpdateAllButtonStates()
    {
        foreach (var entry in buttonItemMap)
        {
            Button button = entry.Key;
            NewItemScript itemScript = entry.Value;
            UpdateSingleButtonState(button, itemScript);
        }
    }

    private void UpdateSingleButtonState(Button button, NewItemScript itemScript)
    {
        // Handle cases where a button might be hidden
        if (button == null || itemScript == null || playerStats == null || !button.gameObject.activeInHierarchy) return;

        bool canAfford = playerStats.money >= itemScript.itemData.value; // <--- Changed to itemScript.value
        button.interactable = canAfford;
    }

    void PurchaseItem(Button clickedButton)
    {
        if (buttonItemMap.TryGetValue(clickedButton, out NewItemScript itemToBuyScript))
        {
            // Corrected: Access value and name directly from itemToBuyScript
            if (playerStats.money >= itemToBuyScript.itemData.value) // <--- Changed to itemToBuyScript.value
            {
                playerStats.money -= itemToBuyScript.itemData.value; // <--- Changed to itemToBuyScript.value
                Debug.Log($"Purchased {itemToBuyScript.itemData.name} for {itemToBuyScript.itemData.value} money."); // <--- Changed to itemToBuyScript.value

                // Spawn the item using the CollectionManager
                // We need the original GameObject prefab (itemToBuyScript.gameObject), not just the script itself
                GameObject itemPrefabToSpawn = itemToBuyScript.gameObject; // The NewItemScript is on the prefab itself

                if (itemPrefabToSpawn != null)
                {
                    collectionManager.SpawnInCollection(itemPrefabToSpawn);
                    clickedButton.gameObject.SetActive(false);
                }
                else
                {
                    Debug.LogError($"Could not find prefab to spawn for {itemToBuyScript.itemData.name}! " +
                                   $"Ensure the NewItemScript is attached to a prefab in 'allAvailableStoreItems'.");
                }

                ShowFeedback($"Purchased {itemToBuyScript.itemData.name}!");
                UpdateAllButtonStates(); // Re-check button states after purchase
            }
            else
            {
                Debug.Log("Not enough money to buy " + itemToBuyScript.itemData.name);
                ShowFeedback("Not enough money!");
            }
        }
        else
        {
            Debug.LogError("Clicked button not found in buttonItemMap!");
        }
    }

    void ShowFeedback(string message)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            // You might want to add a coroutine here to fade out the text after a few seconds
            StartCoroutine(ClearFeedbackAfterDelay(3f));
        }
    }

    IEnumerator ClearFeedbackAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (feedbackText != null)
        {
            feedbackText.text = "";
        }
    }

    public void CloseStore()
    {
        storeCanvas.SetActive(false);
    }
}
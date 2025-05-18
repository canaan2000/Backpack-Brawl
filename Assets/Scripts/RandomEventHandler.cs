using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


[System.Serializable]
public class RandomEvent
{
    public string title;
    public string button1Text;
    public string button2Text;
    public GameObject dropButton1;
    public GameObject dropButton2;
}


public class RandomEventHandler : MonoBehaviour
{
    public CollectionManager Collection;

    public List<RandomEvent> possibleEvents;
    public TextMeshProUGUI eventTitleText; // Assign in Inspector
    public Button button1;                // Assign in Inspector
    public Button button2;                // Assign in Inspector
    public GameObject button1DropParent;   // Assign in Inspector (where button 1 drop will be parented)
    public GameObject button2DropParent;   // Assign in Inspector (where button 2 drop will be parented)

    private RandomEvent currentEvent;

    void Start()
    {
        HideEventUI();
    }

    // Call this function to trigger a random event
    public void TriggerRandomEvent()
    {
        ShowEventUI();

        if (possibleEvents.Count > 0)
        {
            // Choose a random event from the list
            int randomIndex = Random.Range(0, possibleEvents.Count);
            currentEvent = possibleEvents[randomIndex];

            // Update UI elements
            if (eventTitleText != null)
            {
                eventTitleText.text = currentEvent.title;
            }

            if (button1 != null)
            {
                TextMeshProUGUI button1TMP = button1.GetComponentInChildren<TextMeshProUGUI>();
                if (button1TMP != null)
                {
                    button1TMP.text = currentEvent.button1Text;
                }
                button1.onClick.RemoveAllListeners(); // Clear previous listeners
                button1.onClick.AddListener(() => HandleButton1Click());
                button1.gameObject.SetActive(true);
            }

            if (button2 != null)
            {
                TextMeshProUGUI button2TMP = button2.GetComponentInChildren<TextMeshProUGUI>();
                if (button2TMP != null)
                {
                    button2TMP.text = currentEvent.button2Text;
                }
                button2.onClick.RemoveAllListeners(); // Clear previous listeners
                button2.onClick.AddListener(() => HandleButton2Click());
                button2.gameObject.SetActive(true);
            }

            // Hide the third button (assuming you only want to show two for these events)
            // You'll need to assign your third button in the Inspector if you have one.
            Button button3 = null; // Assign your third button here if needed
            if (button3 != null)
            {
                button3.gameObject.SetActive(false);
            }
        }
        else
        {
            Debug.LogWarning("No random events defined in the Inspector!");
        }
    }

    void HandleButton1Click()
    {
        if (currentEvent != null && currentEvent.dropButton1 != null && button1DropParent != null)
        {
            Collection.SpawnInCollection(currentEvent.dropButton1);
            Debug.Log($"Button 1 clicked: Dropped {currentEvent.dropButton1.name}");
        }
        else
        {
            Debug.Log("Button 1 clicked, but no drop defined or drop parent is missing.");
        }
        // Optionally, you might want to hide the event UI after a choice is made
        HideEventUI();
    }

    void HandleButton2Click()
    {
        if (currentEvent != null && currentEvent.dropButton2 != null && button2DropParent != null)
        {
            Collection.SpawnInCollection(currentEvent.dropButton2);
            Debug.Log($"Button 2 clicked: Dropped {currentEvent.dropButton2.name}");
        }
        else
        {
            Debug.Log("Button 2 clicked, but no drop defined or drop parent is missing.");
        }
        // Optionally, you might want to hide the event UI after a choice is made
        HideEventUI();
    }

    void HideEventUI()
    {
        if (eventTitleText != null) eventTitleText.gameObject.SetActive(false);
        if (button1 != null) button1.gameObject.SetActive(false);
        if (button2 != null) button2.gameObject.SetActive(false);
        // Show your third button again if you hid it
        // Button button3 = null; // Assign your third button here if needed
        // if (button3 != null) button3.gameObject.SetActive(true);
    }

    // You might want a function to show the event UI again later
    public void ShowEventUI()
    {
        if (eventTitleText != null) eventTitleText.gameObject.SetActive(true);
        if (button1 != null) button1.gameObject.SetActive(true);
        if (button2 != null) button2.gameObject.SetActive(true);
        // Hide your third button again if needed
        // Button button3 = null; // Assign your third button here if needed
        // if (button3 != null) button3.gameObject.SetActive(false);
    }

    // Example of how you might trigger an event after a delay
    public void TriggerEventWithDelay(float delay)
    {
        Invoke(nameof(TriggerRandomEvent), delay);
    }
}

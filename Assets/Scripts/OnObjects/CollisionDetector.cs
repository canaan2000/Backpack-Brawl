using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public MergeMathScript merger; // Assigned in Inspector
    public MergeDictionary Dictionary; // Assigned in Inspector

    void Start()
    {
        // Locate singletons if missing
        if (merger == null)
        {
            merger = GameObject.Find("Merge Manager")?.GetComponent<MergeMathScript>();
            if (merger == null) Debug.LogError("Merge Manager or MergeMathScript missing!");
        }
        if (Dictionary == null)
        {
            Dictionary = GameObject.Find("Merge Manager")?.GetComponent<MergeDictionary>();
            if (Dictionary == null) Debug.LogError("Merge Manager or MergeDictionary missing!");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Get item scripts, ignore if missing
        NewItemScript thisItemScript = GetComponent<NewItemScript>();
        NewItemScript otherItemScript = collision.gameObject.GetComponent<NewItemScript>();

        if (thisItemScript == null)
        {
            Debug.LogWarning($"{gameObject.name} missing NewItemScript. Ignoring.");
            return;
        }
        if (otherItemScript == null) return; // Only process item-to-item collisions

        // Ignore floor collisions
        if (collision.transform.tag == "Floor") return;

        // Handle addition merge items
        if (otherItemScript.itemData.additionMergeItem)
        {
            Debug.Log($"Processing addition item {otherItemScript.itemData.name} with {thisItemScript.itemData.name}.");

            switch (otherItemScript.itemData.gainType)
            {
                case NewItemScript.ItemClass.GainType.Damage:
                    thisItemScript.itemData.damage += otherItemScript.itemData.gainAmount;
                    Debug.Log($"{thisItemScript.itemData.name} gained {otherItemScript.itemData.gainAmount} Damage. New: {thisItemScript.itemData.damage}");
                    break;
                case NewItemScript.ItemClass.GainType.Stamina:
                    thisItemScript.itemData.staminaUsage -= otherItemScript.itemData.gainAmount;
                    thisItemScript.itemData.autoStaminaUsage -= otherItemScript.itemData.gainAmount;
                    Debug.Log($"{thisItemScript.itemData.name} gained {otherItemScript.itemData.gainAmount} Stamina. New Usage: {thisItemScript.itemData.staminaUsage}");
                    break;
                case NewItemScript.ItemClass.GainType.Speed:
                    thisItemScript.itemData.cooldown -= otherItemScript.itemData.gainAmount;
                    Debug.Log($"{thisItemScript.itemData.name} gained {otherItemScript.itemData.gainAmount} Speed. New Cooldown: {thisItemScript.itemData.cooldown}");
                    break;
                default:
                    Debug.LogWarning($"Unhandled GainType: {otherItemScript.itemData.gainType}");
                    break;
            }

            // Apply effects and destroy addition item
            Destroy(collision.gameObject);
            return;
        }

        // Handle general merges (excluding certain tags)
        if (collision.transform.tag != "AdditionItem")
        {
            GameObject Obj1 = this.gameObject;
            GameObject Obj2 = collision.gameObject;

            Debug.Log($"Attempting merge between {Obj1.name} and {Obj2.name}.");
            Dictionary.CheckMerge(Obj1, Obj2);
        }
        else
        {
            Debug.Log($"Ignored collision with {collision.gameObject.name} ({collision.transform.tag})");
        }
    }

    private void OnMouseDown()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        if (rb != null && rb.isKinematic) rb.isKinematic = false;
    }
}
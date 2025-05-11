using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketInventoryManager : MonoBehaviour
{
    public GameObject storedObj;
    private bool hasStoredObject = false; // New flag
    public InventoryList inventoryList;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (storedObj == null && !hasStoredObject) // Check the flag as well
        {
            storedObj = other.gameObject;
            Rigidbody rb = storedObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;
            }
            hasStoredObject = true; // Set the flag

            if (inventoryList.inventoryList.Contains(storedObj))
            {
                inventoryList.inventoryList.Remove(storedObj);
                inventoryList.Stats.UpdateStats();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == storedObj)
        {
            storedObj = null;
            hasStoredObject = false; // Reset the flag when the object leaves
        }
        if (!inventoryList.inventoryList.Contains(storedObj))
        {
            inventoryList.inventoryList.Add(storedObj);
            inventoryList.Stats.UpdateStats();
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketInventoryManager : MonoBehaviour
{
    public GameObject storedObj;
    public InventoryList inventoryList;
    public PlayerStats playerStats;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        if (storedObj == null && !Input.GetMouseButton(0)) // Check the flag as well
        {
            storedObj = other.gameObject;
            Rigidbody rb = storedObj.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.isKinematic = true;

                inventoryList.Stats.storedItem = storedObj;
                inventoryList.Stats.UpdateStats();
            }

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
            inventoryList.Stats.storedItem = null;
            inventoryList.Stats.UpdateStats();
        }
        if (inventoryList.inventoryList.Contains(storedObj))
        {
            inventoryList.inventoryList.Add(storedObj);
            inventoryList.Stats.UpdateStats();
        }
    }
}
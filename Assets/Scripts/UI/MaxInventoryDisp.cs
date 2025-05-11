using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaxInventoryDisp : MonoBehaviour
{
    public PocketInventoryManager pocketManager;
    public InventoryList inventoryManager;
    public CollectionToInventory colToInv;

    public TextMeshProUGUI inventoryDisp;
    public TextMeshProUGUI pocketDisp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inventoryDisp.text = inventoryManager.inventoryList.Count.ToString();
        inventoryDisp.text += "/" + colToInv.maxInventory;
        if (inventoryManager.inventoryList.Count > colToInv.maxInventory)
        {
            inventoryDisp.color = Color.red;
        }
        else
        {
            inventoryDisp.color = Color.white;
        }

        if (pocketManager.storedObj != null) 
        {
            pocketDisp.text = "1";
        }
        else
        {
            pocketDisp.text = "0";
        }
        pocketDisp.text += "/1";
    }
}

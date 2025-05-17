using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipScript : MonoBehaviour
{
    public LayerMask IgnoreRaycast;
    public GameObject selectedGameobject;
    public TextMeshProUGUI tooltip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreRaycast))
        {
            if (hit.collider != null)
            {
                // Enable tooltip
                tooltip.enabled = true;
                // Select object
                selectedGameobject = hit.collider.gameObject;
                // Display all info in tooltip.
                NewItemScript itemScript = selectedGameobject.GetComponent<NewItemScript>();

                if (itemScript != null && itemScript.itemData != null)
                {
                    tooltip.text = itemScript.itemData.name + ":";

                    if (!string.IsNullOrEmpty(itemScript.itemData.description))
                    {
                        tooltip.text += "\n" + itemScript.itemData.description;
                    }

                    if (itemScript.itemData.damage > 0)
                    {
                        tooltip.text += "\nDamage per second: " + itemScript.itemData.damage;
                    }

                    if (itemScript.itemData.armor > 0)
                    {
                        tooltip.text += "\nArmor: " + itemScript.itemData.armor;
                    }

                    if (itemScript.itemData.poison > 0)
                    {
                        tooltip.text += "\nPoison: " + itemScript.itemData.poison;
                    }

                    if (itemScript.itemData.staminaUsage > 0)
                    {
                        tooltip.text += "\nStamina Cost: " + itemScript.itemData.staminaUsage;
                    }

                    if (itemScript.itemData.clickHealing > 0) 
                    {
                        tooltip.text += "\nHealing: " + itemScript.itemData.clickHealing;
                    }

                    if (itemScript.itemData.clickHunger > 0)
                    {
                        tooltip.text += "\nFood: " + itemScript.itemData.clickHunger;
                    }

                    if (itemScript.itemData.clickArmor > 0)
                    {
                        tooltip.text += "\nArmor: " + itemScript.itemData.clickArmor;
                    }

                    if (itemScript.itemData.clickDamage > 0)
                    {
                        tooltip.text += "\nDamage: " + itemScript.itemData.clickDamage;
                    }

                    if (itemScript.itemData.clickPoison > 0)
                    {
                        tooltip.text += "\nPoison: " + itemScript.itemData.clickPoison;
                    }

                    if (itemScript.itemData.singleUse)
                    {
                        tooltip.text += "\nSingle Use";
                    }
                }
                else
                {
                    tooltip.text = "No item data found on this object.";
                }
            }
        }
        else
        {
            tooltip.enabled = false;
        }
    }
}

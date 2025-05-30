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

        // Use '~IgnoreRaycast' to raycast against everything *except* the IgnoreRaycast layer.
        // Ensure that your UI elements with tooltips are on a layer *not* in IgnoreRaycast.
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
                    // Start with the item name
                    tooltip.text = itemScript.itemData.name + ":";

                    if (!string.IsNullOrEmpty(itemScript.itemData.description))
                    {
                        tooltip.text += "\n" + itemScript.itemData.description;
                    }

                    if (itemScript.itemData.damage > 0)
                    {
                        tooltip.text += "\n<sprite=0> " + itemScript.itemData.damage; // Damage sprite 0
                    }

                    if (itemScript.itemData.armor > 0)
                    {
                        // Assuming Armor still uses text, or you'll need a sprite ID for it
                        tooltip.text += "\nArmor: " + itemScript.itemData.armor;
                    }

                    if (itemScript.itemData.poison > 0)
                    {
                        tooltip.text += "\n<sprite=3> " + itemScript.itemData.poison; // Poison sprite 3
                    }

                    if (itemScript.itemData.staminaUsage > 0)
                    {
                        tooltip.text += "\nClick <sprite=4> " + itemScript.itemData.staminaUsage; // Stamina sprite 4
                    }

                    if (itemScript.itemData.clickHealing > 0)
                    {
                        tooltip.text += "\nClick <sprite=1> " + itemScript.itemData.clickHealing; // Healing/Health sprite 1
                    }

                    if (itemScript.itemData.clickHunger > 0)
                    {
                        // Assuming Food/Hunger still uses text, or you'll need a sprite ID for it
                        tooltip.text += "\nFood: " + itemScript.itemData.clickHunger;
                    }

                    if (itemScript.itemData.clickArmor > 0)
                    {
                        // Assuming Armor still uses text, or you'll need a sprite ID for it
                        tooltip.text += "\nArmor: " + itemScript.itemData.clickArmor;
                    }

                    if (itemScript.itemData.clickDamage > 0)
                    {
                        tooltip.text += "\nClick <sprite=0> " + itemScript.itemData.clickDamage; // Damage sprite 0
                    }

                    if (itemScript.itemData.clickPoison > 0)
                    {
                        tooltip.text += "\nClick <sprite=3> " + itemScript.itemData.clickPoison; // Poison sprite 3
                    }

                    if (itemScript.itemData.singleUse)
                    {
                        tooltip.text += "\nSingle Use";
                    }

                    if (itemScript.itemData.autoManaGain != 0)
                    {
                        tooltip.text += "\n<sprite=2> " + itemScript.itemData.autoManaGain; // Mana sprite 2
                    }

                    if (itemScript.itemData.clickManaUsage != 0)
                    {
                        tooltip.text += "\n<sprite=2> " + itemScript.itemData.clickManaUsage; // Mana sprite 2
                    }

                    // Add Thorns if it's a property of itemData (you didn't have it before)
                    // You'll need to add 'thorns' to your NewItemScript's itemData structure for this to work
                    // Example: if (itemScript.itemData.thorns > 0) { tooltip.text += "\n<sprite=5> " + itemScript.itemData.thorns; }
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
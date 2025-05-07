using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickManager : MonoBehaviour
{
    public CombatScript Combat;

    private float clickCooldown = 1f;
    public bool readyToClick = true;
    float cooldown;
    // Start is called before the first frame update
    void Start()
    {
        Combat = GameObject.Find("CombatManager").GetComponent<CombatScript>();

        cooldown = clickCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown <= 0)
        {
            readyToClick = true;
            cooldown = clickCooldown;
        }

        if (Input.GetMouseButtonDown(0))
        {
            readyToClick = false;

        }

        if (!readyToClick)
        {
            cooldown -= Time.deltaTime;
        }
    }

    //What an object does when clicked.
    private void OnMouseDown()
    {
        NewItemScript itemScript = gameObject.GetComponent<NewItemScript>();
        if (Combat.PlayerStats.stamina >= itemScript.itemData.staminaUsage && Combat.combatTrue == true)
        {
            readyToClick = false;
            Combat.PlayerStats.armor += itemScript.itemData.clickArmor;
            Combat.EnemyStats.Health -= itemScript.itemData.clickDamage;
            Combat.PlayerStats.hunger += itemScript.itemData.clickHunger;
            Combat.EnemyStats.Poison += itemScript.itemData.clickPoison;
            Combat.PlayerStats.health += itemScript.itemData.clickHealing;

            Combat.PlayerStats.stamina -= itemScript.itemData.staminaUsage;
            if (itemScript.itemData.singleUse == true)
            {
                Destroy(gameObject);
            }
        }

    }
}

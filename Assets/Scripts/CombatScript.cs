using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatScript : MonoBehaviour
{
    public PlayerStats PlayerStats;
    public EnemyScript EnemyStats;
    public InventoryStats InventoryStats;
    public InventoryList InventoryList;
    public PocketInventoryManager Pocket;
    public RandomEventHandler RandomEvent;
    public EnemyInventoryScript EnemyInventory;

    public Button startFightButton;

    public GameObject enemy;

    public GameObject damageNumber;
    public GameObject damageNumberSpawner;
    public GameObject thornsDamageNumberSpawner;

    public float globalCooldown = 1f;
    public float cooldown = 0;

    public float scalePercent = .3f;
    //public float baseAttack = 2;
    public float baseHealth = 30f;

    public int level = 1;

    public bool combatTrue = false;

    public Color poisonDamageColor;
    public Color thornsDamageColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (combatTrue == true && !Input.GetKey(KeyCode.Space)) 
        {
            foreach (var item in InventoryList.inventoryList)
            {
                NewItemScript itemScript = item.GetComponent<NewItemScript>();
                itemScript.itemData.timeRemaining -= Time.deltaTime;

                if (itemScript.itemData.timeRemaining < 0)
                {
                    itemScript.itemData.timeRemaining = itemScript.itemData.cooldown;
                    ActivateSingleItem(itemScript, true);
                }
            }


            foreach (var item in EnemyInventory.enemyInventory)
            {
                NewItemScript itemScript = item.GetComponent<NewItemScript>();
                itemScript.itemData.timeRemaining -= Time.deltaTime;

                if (itemScript.itemData.timeRemaining < 0)
                {
                    itemScript.itemData.timeRemaining = itemScript.itemData.cooldown;
                    ActivateSingleItem(itemScript, false);
                }
            }


            cooldown -= Time.deltaTime;
            if (cooldown <= 0 && combatTrue == true)
            {
                DealDamage();
            }

            if (EnemyStats.Health <= 0 && EnemyStats != null)
            {
                FightEnd();
            }
        }
        //When combat isnt happening
        else
        {
            
        }
    }

    

    public void FightStart()
    {
        Instantiate(enemy);

        EnemyInventory.FindEnemy();
        EnemyInventory.SpawnEnemyItem();

        EnemyStats = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyScript>();
        //EnemyStats.Attack = baseAttack * Mathf.Pow(1 + scalePercent, level - 1); //Made more item oritented
        EnemyStats.Health = baseHealth * Mathf.Pow(1 + scalePercent, level - 1);
        EnemyStats.Poison = InventoryStats.UpdatePoisonStats();

        combatTrue = true; 

        startFightButton.gameObject.SetActive(false);

        InventoryStats.UpdateArmorStats();

        InventoryStats.UpdateThorns();
        
    }

    void FightEnd()
    {
        Destroy(EnemyStats.gameObject);

        foreach (var item in EnemyInventory.enemyInventory)
        {
            Destroy(item);
        }

        PlayerStats.thorns = 0f;

        PlayerStats.money += Random.Range(10f, 55f);

        combatTrue = false;

        level++;

        startFightButton.gameObject.SetActive(true);

        RandomEvent.TriggerRandomEvent();
    }

    void ActivateSingleItem(NewItemScript itemScript, bool player)
    {
        {
            if (itemScript == null) return;

            //If item belongs to player
            if (player)
            {
                // Apply item's damage if it has any and player has stamina
                if (PlayerStats.stamina > itemScript.itemData.autoStaminaUsage)
                {
                    InventoryList.StartDamageNumbers(itemScript.gameObject);

                    // Apply stamina usage specific to this item
                    PlayerStats.stamina -= itemScript.itemData.autoStaminaUsage;

                    for (int i = 0; i < itemScript.itemData.damage; i++)
                    {
                        if (EnemyStats.Health > 0)
                        {
                            EnemyStats.Health--;
                        }
                    }

                    // Apply item's mana gain
                    if (itemScript.itemData.autoManaGain > 0)
                    {
                        PlayerStats.mana += itemScript.itemData.autoManaGain;
                    }
                }
                else
                {
                    GameObject DNText = Instantiate(damageNumber, damageNumberSpawner.transform.position, Quaternion.identity);
                    DNText.GetComponentInChildren<TextMeshProUGUI>().text = "Not Enough <sprite=4>";
                    DamageNumberBehavior Behavior = DNText.GetComponent<DamageNumberBehavior>();
                    DNText.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Healing);
                }
            }
            //if item does not belong to player
            else
            {
                EnemyInventory.StartDamageNumbers(itemScript.gameObject);

                for (int i = 0; i < itemScript.itemData.damage; i++)
                {
                    if (PlayerStats.health > 0)
                    {
                        PlayerStats.health--;
                    }
                }
            }
            
        }
    }

    void DealDamage()
    {
        DealPoisonDamage();
        DealThornDamage();
        
        cooldown = globalCooldown;
    }

    void DealPoisonDamage()
    {
        if (EnemyStats.Poison > 0) 
        {
            EnemyStats.Health -= EnemyStats.Poison;

            //damageNumber
            GameObject PDN = Instantiate(damageNumber, damageNumberSpawner.transform.position, Quaternion.identity);
            PDN.GetComponentInChildren<TextMeshProUGUI>().text = EnemyStats.Poison.ToString() + "<sprite=3>";
            DamageNumberBehavior Behavior = PDN.GetComponent<DamageNumberBehavior>();
            PDN.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Poison);
            EnemyStats.Poison -= 1;
        }

        if (PlayerStats.poison > 0) 
        {
            PlayerStats.health -= PlayerStats.poison;

            //damageNumber
            GameObject PDN = Instantiate(damageNumber, damageNumberSpawner.transform.position, Quaternion.identity);
            PDN.GetComponentInChildren<TextMeshProUGUI>().text = PlayerStats.poison.ToString() + "<sprite=3>";
            DamageNumberBehavior Behavior = PDN.GetComponent<DamageNumberBehavior>();
            PDN.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Poison);
            PlayerStats.poison -= 1;
        }
    }

    void DealThornDamage()
    {
        if (PlayerStats.thorns > 0)
        {
            EnemyStats.Health -= PlayerStats.thorns;

            //ThornDamageNumber
            GameObject TDN = Instantiate(damageNumber, thornsDamageNumberSpawner.transform.position, Quaternion.identity);
            TDN.GetComponentInChildren<TextMeshProUGUI>().text = PlayerStats.thorns.ToString() + "<sprite=5>"; 
            DamageNumberBehavior Behavior = TDN.GetComponent<DamageNumberBehavior>();
            TDN.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Thorns);
        }

        //Add enemy Thorns..
    }
}

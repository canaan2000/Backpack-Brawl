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

    public Button startFightButton;

    public GameObject enemy;

    public GameObject damageNumber;
    public GameObject damageNumberSpawner;
    public GameObject thornsDamageNumberSpawner;

    public float attackCooldown = 1f;
    public float cooldown = 0;

    public float scalePercent = .3f;
    public float baseAttack = 2;
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

    

    //NAME ENEMIES "ENEMY"
    public void FightStart()
    {
        Instantiate(enemy);

        EnemyStats = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyScript>();
        EnemyStats.Attack = baseAttack * Mathf.Pow(1 + scalePercent, level - 1);
        EnemyStats.Health = baseHealth * Mathf.Pow(1 + scalePercent, level - 1);
        EnemyStats.Poison = InventoryStats.UpdatePoisonStats();

        combatTrue = true; 

        startFightButton.gameObject.SetActive(false);

        InventoryStats.UpdateArmorStats();
        
    }

    void FightEnd()
    {
        Destroy(EnemyStats.gameObject);

        PlayerStats.thorns = 0f;

        PlayerStats.money += Random.Range(10f, 55f);

        combatTrue = false;

        level++;

        startFightButton.gameObject.SetActive(true);

        RandomEvent.TriggerRandomEvent();
    }

    void DealDamage()
    {
        float playerDamage = PlayerStats.attack;
        float enemyDamage = EnemyStats.Attack;
        DealPoisonDamage();
        InventoryStats.HandleMana();
        if (PlayerStats.stamina > 0)
        {
            InventoryList.StartDamageNumbers();
            
            InventoryStats.HandleAutoStaminaUsage();

            for (int i = 0; i < playerDamage; i++)
            {
                if (EnemyStats.Health > 0)
                {
                    EnemyStats.Health--;
                }
            }
        }
        DealThornDamage();
        
        for (int i = 0; i < enemyDamage; i++)
        {
            if (PlayerStats.armor > 0)
            {
                PlayerStats.armor--;
            }
            else
            {
                PlayerStats.health--;
            }
        }
        
        
            
        
        cooldown = attackCooldown;
    }

    void DealPoisonDamage()
    {
        if (EnemyStats.Poison > 0) 
        {
            EnemyStats.Health -= EnemyStats.Poison;

            //damageNumber
            GameObject PDN = Instantiate(damageNumber, damageNumberSpawner.transform.position, Quaternion.identity);
            PDN.GetComponentInChildren<TextMeshProUGUI>().text = EnemyStats.Poison.ToString();
            DamageNumberBehavior Behavior = PDN.GetComponent<DamageNumberBehavior>();
            PDN.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Mana);
            EnemyStats.Poison -= 1;
        }
    }

    void DealThornDamage()
    {
        if (PlayerStats.thorns > 0)
        {
            EnemyStats.Health -= PlayerStats.thorns;

            //ThornDamageNumber
            GameObject TDN = Instantiate(damageNumber, thornsDamageNumberSpawner.transform.position, Quaternion.identity);
            TDN.GetComponentInChildren<TextMeshProUGUI>().text = PlayerStats.thorns.ToString(); 
            DamageNumberBehavior Behavior = TDN.GetComponent<DamageNumberBehavior>();
            TDN.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Mana);
        }
    }
}

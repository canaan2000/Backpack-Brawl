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

    public GameObject poisonDamageNumber;
    public GameObject poisonDamageNumberSpawner;

    public float attackCooldown = 1f;
    public float cooldown = 0;

    public float scalePercent = .3f;
    public float baseAttack = 2;
    public float baseHealth = 30f;

    public int level = 1;

    public bool combatTrue = false;
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

        PlayerStats.money += Random.Range(10f, 55f);

        combatTrue = false;

        level++;

        startFightButton.gameObject.SetActive(true);

        RandomEvent.TriggerRandomEvent();
    }

    void DealDamage()
    {
        InventoryList.StartDamageNumbers();
        DealPoisonDamage();

        float playerDamage = PlayerStats.attack;
        float enemyDamage = EnemyStats.Attack;
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

        for (int i = 0; i < playerDamage; i++)
            {
                if (EnemyStats.Health > 0)
                {
                    EnemyStats.Health--;
                }
            }
        cooldown = attackCooldown;
    }

    void DealPoisonDamage()
    {
        if (EnemyStats.Poison > 0) 
        {
            EnemyStats.Health -= EnemyStats.Poison;

            //PoisonDamageNumber
            GameObject PDN = Instantiate(poisonDamageNumber, poisonDamageNumberSpawner.transform.position, Quaternion.identity);
            PDN.GetComponentInChildren<TextMeshProUGUI>().text = EnemyStats.Poison.ToString();
            EnemyStats.Poison -= 1;
        }
    }
}

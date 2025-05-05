using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public BundleDesicionHandler Bundles;

    public PlayerStats PlayerStats;
    public EnemyScript EnemyStats;
    public InventoryStats InventoryStats;

    public GameObject enemy;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject Enemy = Instantiate(enemy);
            FightStart();
        }
        if (combatTrue == true)
        {


            cooldown -= Time.deltaTime;
            if (cooldown <= 0 && combatTrue == true)
            {
                DealDamage();
            }

            if (EnemyStats.Health <= 0)
            {
                FightEnd();
            }
        }
    }

    

    //NAME ENEMIES "ENEMY"
    void FightStart()
    {
        EnemyStats = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyScript>();
        EnemyStats.Attack = baseAttack * Mathf.Pow(1 + scalePercent, level - 1);
        EnemyStats.Health = baseHealth * Mathf.Pow(1 + scalePercent, level - 1);

        combatTrue = true;

        InventoryStats.UpdateArmorStats();
    }

    void FightEnd()
    {
        Destroy(EnemyStats.gameObject);

        PlayerStats.money += Random.Range(1f, 55f);

        combatTrue = false;

        level++;

        Bundles.ShowOptions();
    }

    void DealDamage()
    {
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
}

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public BundleDesicionHandler Bundles; // Reference to bundle decision UI.
    public PlayerStats PlayerStats; // Reference to player statistics.
    public EnemyScript EnemyStats; // Reference to enemy statistics.
    public InventoryStats InventoryStats; // Reference to inventory statistics.

    public GameObject enemy; // Enemy prefab to instantiate.

    public float attackCooldown = 1f; // Time between attacks.
    public float cooldown = 0; // Current attack cooldown timer.

    public float baseHealth = 25f; // Base enemy health.
    public float baseAttack = 2f; // Base enemy attack.

    public float healthScaleFactor = .3f; // Scaling factor for enemy health per level.
    public float attackScaleFactor = .2f; // Scaling factor for enemy attack per level.

    int level = 1; // Current enemy level.

    public bool combatTrue = false; // Flag indicating if combat is active.
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Spawn new enemy and set its attack and health on spacebar press.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject newEnemy = Instantiate(enemy);
            newEnemy.GetComponent<EnemyScript>().Health = baseHealth * (1 + (level - 1) * healthScaleFactor); // Scale enemy health.
            newEnemy.GetComponent<EnemyScript>().Attack = baseAttack * (1 + (level - 1) * attackScaleFactor); // Scale enemy attack.
            FightStart(); // Begin combat.
        }
        // Combat active: decrease cooldown and deal damage.
        if (combatTrue == true)
        {
            cooldown -= Time.deltaTime; // Reduce cooldown.
            if (cooldown <= 0 && combatTrue == true)
            {
                DealDamage(); // Execute damage dealing.
            }

            if (EnemyStats.Health <= 0)
            {
                FightEnd(); // End combat if enemy health is zero.
            }
        }

    }

    // NAME ENEMIES "ENEMY"
    void FightStart()
    {
        EnemyStats = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyScript>(); // Find the spawned enemy.
        combatTrue = true; // Set combat to active.
        InventoryStats.UpdateArmorStats(); // Update player armor based on inventory.
    }

    void FightEnd()
    {
        Destroy(EnemyStats.gameObject); // Destroy the defeated enemy.
        PlayerStats.money += Random.Range(1f, 55f); // Grant random money to player.
        combatTrue = false; // Set combat to inactive.
        Bundles.ShowOptions(); // Show bundle decision options.
        level++;
    }

    void DealDamage()
    {
        float playerDamage = PlayerStats.attack; // Get player attack.
        float enemyDamage = EnemyStats.Attack; // Get enemy attack.
        for (int i = 0; i < enemyDamage; i++)
        {
            if (PlayerStats.armor > 0)
            {
                PlayerStats.armor--; // Reduce player armor.
            }
            else
            {
                PlayerStats.health--; // Reduce player health if no armor.
            }
        }

        for (int i = 0; i < playerDamage; i++)
        {
            if (EnemyStats.Health > 0)
            {
                EnemyStats.Health--; // Reduce enemy health.
            }
        }
        cooldown = attackCooldown; // Reset the attack cooldown.
    }
}
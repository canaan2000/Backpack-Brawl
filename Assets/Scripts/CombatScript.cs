using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatScript : MonoBehaviour
{
    public BundleDesicionHandler Bundles;

    public PlayerStats PlayerStats;
    public EnemyScript EnemyStats;

    public GameObject enemy;

    public float attackCooldown = 1f;
    float cooldown = 0;

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
            Instantiate(enemy);
            FightStart();
        }
        if (combatTrue == true)
        {


            cooldown -= Time.deltaTime;
            if (cooldown <= 0 && combatTrue == true)
            {
                PlayerStats.health -= EnemyStats.Attack;
                EnemyStats.Health -= PlayerStats.attack;
                cooldown = attackCooldown;
            }

            if (EnemyStats.Health <= 0)
            {
                FightEnd();
            }
        }
    }

    

    //NAME ENEMY'S "ENEMY"
    void FightStart()
    {
        EnemyStats = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyScript>();

        combatTrue = true;
    }

    void FightEnd()
    {
        Destroy(EnemyStats.gameObject);

        PlayerStats.money += Random.Range(1f, 55f);

        combatTrue = false;

        Bundles.ShowOptions();
    }
}

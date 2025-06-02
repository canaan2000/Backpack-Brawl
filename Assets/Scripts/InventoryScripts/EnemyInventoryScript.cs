using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInventoryScript : MonoBehaviour
{
    public List<GameObject> itemsToSpawn = new List<GameObject>();

    public List<GameObject> enemyInventory = new List<GameObject>();

    public EnemyScript enemyScript;

    public GameObject enemySpawner;

    public CombatScript combatScript;

    public float spawnCooldown = 5f;
    public float spawnTime;
    public int indexToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        spawnTime = spawnCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (combatScript.combatTrue)
        {
            spawnTime -= Time.deltaTime;
            if (spawnTime < 0)
            {
                spawnTime = spawnCooldown;
                indexToSpawn++;
                Instantiate(itemsToSpawn[indexToSpawn], enemySpawner.transform.position, Quaternion.identity);
            }
        }


        if (enemyInventory != null)
        {
            foreach (var item in enemyInventory)
            {
                if (item == null)
                {
                    enemyInventory.Remove(item);
                }
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        enemyScript.Attack += collision.GetComponent<NewItemScript>().itemData.damage;


        if (!enemyInventory.Contains(collision.gameObject))
        {
            enemyInventory.Add(collision.gameObject);
        }
    }

    public void StartDamageNumbers(GameObject item)
    {

        NewItemScript itemScript = item.GetComponent<NewItemScript>();
        if (itemScript.itemData.damage > 0)
        {
            item.GetComponent<DamageNumberSpawner>().SpawnDamageNumber();
        }

        if (itemScript.itemData.autoManaGain != 0)
        {
            item.GetComponent<DamageNumberSpawner>().SpawnManaNumber();
        }

    }

    public void FindEnemy()
    {
        enemyScript = GameObject.FindGameObjectWithTag("Enemy").GetComponent<EnemyScript>();
    }

    public void SpawnEnemyItem()
    {
        Instantiate(itemsToSpawn[0], enemySpawner.transform.position, Quaternion.identity);
    }
}

using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyInventoryScript : MonoBehaviour
{
    public List<GameObject> itemsToSpawn = new List<GameObject>();

    public List<GameObject> enemyInventory = new List<GameObject>();

    public List<GameObject> commonItems;
    public List<GameObject> uncommonItems;
    public List<GameObject> rareItems;

    public EnemyScript enemyScript;

    public GameObject enemySpawner;

    public CombatScript combatScript;

    [Range(0f, 1f)]
    public float additionalItemChance = .5f;

    public float commonChance = 75f;
    public float uncommonChance = 20f;
    public float rareChance = 5f;

    public float spawnCooldown = 5f;
    public float spawnTime;
    public int indexToSpawn;
    // Start is called before the first frame update
    void Start()
    {
        foreach (var item in Resources.LoadAll<GameObject>("Objects"))
        {
            if (item.GetComponent<NewItemScript>().itemData.rarity == NewItemScript.ItemClass.Rarity.Common)
            {
                commonItems.Add(item);
            }
            else if (item.GetComponent<NewItemScript>().itemData.rarity == NewItemScript.ItemClass.Rarity.Uncommon)
            {
                uncommonItems.Add(item);
            }
            else if (item.GetComponent<NewItemScript>().itemData.rarity == NewItemScript.ItemClass.Rarity.Rare)
            {
                rareItems.Add(item);
            }
        }
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
                SpawnEnemyItem(indexToSpawn);
            }
        }
        else
        {
            spawnTime = 0;
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
        enemyScript.Attack += collision.GetComponent<NewItemScript>().itemData.damage / collision.GetComponent<NewItemScript>().itemData.cooldown;


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

    public void GenerateItemList()
    {
        float chance = Random.Range(0f, 1f);
        if (chance < additionalItemChance || itemsToSpawn.Count == 0 && itemsToSpawn.Count < 10)
        {
            float rarityChance = Random.Range(0f, commonChance + uncommonChance + rareChance);

            // Check ranges sequentially (roulette wheel)
            if (rarityChance < commonChance)
            {
                int randIndex = Random.Range(0, commonItems.Count);
                itemsToSpawn.Add(commonItems[randIndex]);
            }
            else if (rarityChance < commonChance + uncommonChance)
            {
                int randIndex = Random.Range(0, uncommonItems.Count);
                itemsToSpawn.Add(uncommonItems[randIndex]);
            }
            else if (rarityChance < commonChance + uncommonChance + rareChance) 
            {
                int randIndex = Random.Range(0, rareItems.Count);
                itemsToSpawn.Add(rareItems[randIndex]);
            }
            GenerateItemList();
        }
        else
        {
            return;
        }
    }

    public void SpawnEnemyItem(int index)
    {
        GameObject enemyItem = Instantiate(itemsToSpawn[index], enemySpawner.transform.position, Quaternion.identity);
        enemyItem.tag = "EnemyItem";
        indexToSpawn++;
    }
}

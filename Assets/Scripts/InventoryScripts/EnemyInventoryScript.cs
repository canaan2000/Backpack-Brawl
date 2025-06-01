using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInventoryScript : MonoBehaviour
{
    public List<GameObject> itemsToSpawn = new List<GameObject>();

    public List<GameObject> enemyInventory = new List<GameObject>();

    public GameObject enemySpawner;
    // Start is called before the first frame update
    void Start()
    {
        foreach (GameObject item in itemsToSpawn) 
        {
            Instantiate(item, enemySpawner.transform.position, Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {

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
}

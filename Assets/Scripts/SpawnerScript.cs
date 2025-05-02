using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject cube;
    public GameObject sphere;
    public GameObject cylinder;
    public GameObject capsule;
    public GameObject[] Rocks;
    public GameObject Stick;
    public GameObject Leaf;
    public GameObject Ingot;

    public GameObject spawnArea;

    Vector3 minSpawn;
    Vector3 maxSpawn;

    Vector3 spawnPoint;
    // Start is called before the first frame update
    void Start()
    {
        minSpawn = spawnArea.GetComponent<Collider>().bounds.min;
        maxSpawn = spawnArea.GetComponent<Collider>().bounds.max;

        RandSpawnPos();

        Instantiate(Stick, spawnPoint, Quaternion.identity);
        Instantiate(Rocks[Random.Range(0,Rocks.Length)], spawnPoint, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            RandSpawnPos();
            //changes spawnpoint position
            Instantiate(cube, spawnPoint, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            RandSpawnPos();

            Instantiate(Ingot, spawnPoint, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            RandSpawnPos();

            Instantiate(Leaf, spawnPoint, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            RandSpawnPos();

            Instantiate(Stick, spawnPoint, Quaternion.identity);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            RandSpawnPos();

            Instantiate(Rocks[Random.Range(0,Rocks.Length)], spawnPoint, Quaternion.identity);
        }
    }

    public void SpawnItem(GameObject item)
    {
        RandSpawnPos();

        Instantiate(item, spawnPoint, Quaternion.identity);
    }

    void RandSpawnPos() 
    {
        float randX = Random.Range(minSpawn.x, maxSpawn.x);
        float randY = Random.Range(minSpawn.y, maxSpawn.y);

        spawnPoint = new Vector3(randX, randY, 0);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageNumberSpawner : MonoBehaviour
{
    public GameObject damageNumber;
    Vector3 spawnPos;
    // Start is called before the first frame update
    void Start()
    {
        spawnPos = this.gameObject.transform.position;
        spawnPos.y += 1;
        GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
        damageNumber.transform.SetParent(this.gameObject.transform, true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

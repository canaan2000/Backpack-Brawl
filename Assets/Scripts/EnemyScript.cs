using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public PlayerStats PlayerStats;

    public float Attack;
    public float Health;
    public float Poison;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerStats = GameObject.Find("PlayerStatManager").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Poison > 0) 
        {
            Health -= Poison * Time.deltaTime;

            Poison -= Time.deltaTime / PlayerStats.poisonDepleteSpeed;
        }
    }
}

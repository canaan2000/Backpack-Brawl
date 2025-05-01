using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 100;
    public float armor = 0;
    public float hunger = 100;
    public float attack = 1f;
    public float poison = 0f;
    public float durability = 100f;
    public float money = 0f;

    public int hungerDepleteSpeed = 5; //Higher is slower
    public int poisonDepleteSpeed = 3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //deplete hunger and poison over time.
        hunger -= Time.deltaTime / hungerDepleteSpeed;
        if (poison > 0)
        {
            poison -= Time.deltaTime / poisonDepleteSpeed;
        }
        //Poison depletes health
        health -= poison * Time.deltaTime;
        
        
    }
}

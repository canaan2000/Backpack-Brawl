using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public PlayerStats PlayerStats;

    public float Attack;
    public float Health;
    public float Poison;
    public float stamina;
    public float maxStamina = 10f;
    public float mana;
    public float thorns;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerStats = GameObject.Find("PlayerStatManager").GetComponent<PlayerStats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Poison < 0) 
        {
            Poison = 0;
        }


        if (stamina <= maxStamina && !Input.GetKey(KeyCode.Space))
        {
            stamina += Time.deltaTime;
        }
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTossed : MonoBehaviour
{
    public PlayerStats Stats;
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(collision.gameObject);
        //Player gets one stamina when destroying an item
        Stats.stamina += 1;
        //Give player half of value of item
        Stats.money += collision.gameObject.GetComponent<NewItemScript>().itemData.value / 2;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickManager : MonoBehaviour
{
    public CombatScript Combat;
    
    // Start is called before the first frame update
    void Start()
    {
        Combat = GameObject.Find("CombatManager").GetComponent<CombatScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnMouseDown()
    {
        switch (this.gameObject.tag)
        {
            case "RockTag":
                if (Combat.combatTrue == true)
                {
                    Combat.EnemyStats.Health -= 15f;
                    Destroy(this.gameObject);
                }
                break;

            case "BurgerTag":
                Combat.PlayerStats.hunger += 25f;
                Destroy(this.gameObject);
                break;



        }

    }
}

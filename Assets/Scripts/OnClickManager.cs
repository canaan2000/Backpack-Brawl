using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickManager : MonoBehaviour
{
    public CombatScript Combat;

    private float clickCooldown = 1f;
    public bool readyToClick = true;
    float cooldown;
    // Start is called before the first frame update
    void Start()
    {
        Combat = GameObject.Find("CombatManager").GetComponent<CombatScript>();

        cooldown = clickCooldown;
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown <= 0)
        {
            readyToClick = true;
            cooldown = clickCooldown;
        }

        if (Input.GetMouseButtonDown(0))
        {
            readyToClick = false;

        }

        if (!readyToClick)
        {
            cooldown -= Time.deltaTime;
        }
    }

    //What an object does when clicked.
    private void OnMouseDown()
    {
        if (readyToClick == true)
        {
            readyToClick = false;
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

                case "LeafTag":
                    Combat.PlayerStats.health += 5f;
                    Destroy(this.gameObject);
                    break;

                case "FlowerTag":
                    Combat.PlayerStats.health += 15f;
                    Destroy(this.gameObject);
                    break;

                case "AxeTag":
                    Combat.EnemyStats.Health -= 1f;
                    break;


            }
        }

    }
}

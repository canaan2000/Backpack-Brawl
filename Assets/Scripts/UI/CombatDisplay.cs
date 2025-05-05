using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatDisplay : MonoBehaviour
{
    public CombatScript CombatScript;

    public GameObject attackCooldownDis;

    public GameObject clickCooldownDis;

    public TextMeshProUGUI enemyHealth;
    public TextMeshProUGUI enemyDamage;

    Vector3 normalScale;
    // Start is called before the first frame update
    void Start()
    {
        //get attack cooldown display scale at start
        normalScale = attackCooldownDis.transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        if (CombatScript.combatTrue == true) 
        {
            enemyHealth.enabled = true;
            enemyDamage.enabled = true;

            enemyHealth.text = CombatScript.EnemyStats.Health.ToString("0");
            enemyHealth.color = Color.red;

            enemyDamage.text = CombatScript.EnemyStats.Attack.ToString("0");
        }   
        else
        {
            enemyHealth.enabled = false;
            enemyDamage.enabled = false;
        }

        //Activate and set size of attack cooldown display.
        if (CombatScript.combatTrue == true)
        {
            attackCooldownDis.SetActive(true);
            Vector3 newScale = new Vector3(Mathf.Lerp(0, 1, CombatScript.cooldown / CombatScript.attackCooldown), normalScale.y, normalScale.z);
            attackCooldownDis.transform.localScale = newScale;

            //if combat is active the display the clickcooldown

        }
        else
        {
            attackCooldownDis.SetActive(false);
        }
        
    }
}

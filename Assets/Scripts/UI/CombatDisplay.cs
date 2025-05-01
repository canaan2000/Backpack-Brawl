using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatDisplay : MonoBehaviour
{
    public CombatScript CombatScript;

    public TextMeshProUGUI enemyHealth;
    public TextMeshProUGUI enemyDamage;
    // Start is called before the first frame update
    void Start()
    {
        
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
        
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatDisplay : MonoBehaviour
{
    public CombatScript CombatScript;

    public GameObject attackCooldownDis;

    public TextMeshProUGUI enemyStamina;
    public TextMeshProUGUI enemyMana;
    public TextMeshProUGUI enemyThorns;

    public TextMeshProUGUI enemyHealth;
    public TextMeshProUGUI enemyDamage;

    bool enemyFound = false;

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
        if (CombatScript.combatTrue == true && CombatScript.EnemyStats != null) 
        {
           
            
                enemyHealth.enabled = true;
                enemyDamage.enabled = true;
                enemyMana.enabled = true;
                enemyThorns.enabled = true;
                enemyStamina.enabled = true;

                enemyHealth.text = $"<sprite=1> {CombatScript.EnemyStats.Health:0}";
                enemyHealth.color = Color.red;

                enemyDamage.text = $"<sprite=0> {CombatScript.EnemyStats.Attack:0}";

                enemyStamina.text = $"<sprite=4> {CombatScript.EnemyStats.stamina:0.000}";

                enemyThorns.text = $"<sprite=5> {CombatScript.EnemyStats.thorns:0}";

                enemyMana.text = $"<sprite=2> {CombatScript.EnemyStats.mana:0}";
            

        }   
        else
        {
            enemyHealth.enabled = false;
            enemyDamage.enabled = false;
            enemyMana.enabled = false;
            enemyThorns.enabled = false;
            enemyStamina.enabled = false;
        }

        //Activate and set size of attack cooldown display.
        if (CombatScript.combatTrue == true)
        {
            attackCooldownDis.SetActive(true);
            Vector3 newScale = new Vector3(Mathf.Lerp(0, 1, CombatScript.cooldown / CombatScript.globalCooldown), normalScale.y, normalScale.z);
            attackCooldownDis.transform.localScale = newScale;

            //if combat is active the display the clickcooldown

        }
        else
        {
            attackCooldownDis.SetActive(false);
        }
        
    }
}

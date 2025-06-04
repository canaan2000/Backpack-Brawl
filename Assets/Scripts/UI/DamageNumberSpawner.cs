using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageNumberSpawner : MonoBehaviour
{
    public GameObject damageNumber;
    Vector3 spawnPos;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnDamageNumber()
    {
        spawnPos = this.gameObject.transform.position;
        spawnPos.y += 1;
        GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
        //DamageNumber.transform.SetParent(this.gameObject.transform, true);
        DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.damage.ToString() + "<sprite=0>";
        DamageNumberBehavior Behavior = DamageNumber.GetComponent<DamageNumberBehavior>();
        DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Damage);

    }

    public void SpawnManaNumber()
    {
        spawnPos = this.gameObject.transform.position;
        spawnPos.y += 1;
        GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
        //DamageNumber.transform.SetParent(this.gameObject.transform, true);

        DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.autoManaGain.ToString() + "<sprite=2>";
        DamageNumberBehavior Behavior = DamageNumber.GetComponent<DamageNumberBehavior>();
        DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Mana);
    }

    public void OnClickSpawnNumber()
    {
        NewItemScript itemScript = GetComponent<NewItemScript>();
        if (itemScript.itemData.clickDamage > 0)
        {
            spawnPos = this.gameObject.transform.position;
            spawnPos.y += 1;
            GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
            //DamageNumber.transform.SetParent(this.gameObject.transform, true);
            DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.clickDamage.ToString() + "<sprite=0>";
            DamageNumberBehavior Behavior = DamageNumber.GetComponent<DamageNumberBehavior>();
            DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Damage);

        }

        if (itemScript.itemData.clickPoison > 0)
        {
            spawnPos = this.gameObject.transform.position;
            spawnPos.y += 1;
            GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
            //DamageNumber.transform.SetParent(this.gameObject.transform, true);
            DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.clickPoison.ToString() + "<sprite=3>"; 
            DamageNumberBehavior Behavior = DamageNumber.GetComponent<DamageNumberBehavior>();
            DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Poison);

        }

        if (itemScript.itemData.clickHealing != 0)
        {
            spawnPos = this.gameObject.transform.position;
            spawnPos.y += 1;
            GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
            //DamageNumber.transform.SetParent(this.gameObject.transform, true);
            DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.clickHealing.ToString() + "<sprite=1>"; 
            DamageNumberBehavior Behavior = DamageNumber.GetComponent<DamageNumberBehavior>();
            DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Healing);
        }

        if (itemScript.itemData.clickManaUsage != 0)
        {
            spawnPos = this.gameObject.transform.position;
            spawnPos.y += 1;
            GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
            //DamageNumber.transform.SetParent(this.gameObject.transform, true);
            float manaDisplayAmount = GetComponent<NewItemScript>().itemData.clickManaUsage * -1;
            DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = manaDisplayAmount.ToString() + "<sprite=2>"; 
            DamageNumberBehavior Behavior = DamageNumber.GetComponent<DamageNumberBehavior>();
            DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Mana);
        }
    }
}

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
        damageNumber.transform.SetParent(this.gameObject.transform, true);
        DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.damage.ToString();
    }

    public void OnClickSpawnNumber()
    {
        NewItemScript itemScript = GetComponent<NewItemScript>();
        if (itemScript.itemData.clickDamage > 0)
        {
            spawnPos = this.gameObject.transform.position;
            spawnPos.y += 1;
            GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
            damageNumber.transform.SetParent(this.gameObject.transform, true);
            DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.clickDamage.ToString();
        }

        if (itemScript.itemData.clickPoison > 0)
        {
            spawnPos = this.gameObject.transform.position;
            spawnPos.y += 1;
            GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
            damageNumber.transform.SetParent(this.gameObject.transform, true);
            DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.clickPoison.ToString();
        }
    }
}

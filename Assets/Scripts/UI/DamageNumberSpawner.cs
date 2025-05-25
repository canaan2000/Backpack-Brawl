using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DamageNumberSpawner : MonoBehaviour
{
    public GameObject damageNumber;
    public List<Color> floatingNumberColor = new List<Color>();
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
        DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.damage.ToString();
        DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(floatingNumberColor[0]);

    }

    public void SpawnManaNumber()
    {
        spawnPos = this.gameObject.transform.position;
        spawnPos.y += 1;
        GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
        //DamageNumber.transform.SetParent(this.gameObject.transform, true);
        DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.autoManaGain.ToString();
        DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(floatingNumberColor[3]);
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
            DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.clickDamage.ToString();
            DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(floatingNumberColor[0]);

        }

        if (itemScript.itemData.clickPoison > 0)
        {
            spawnPos = this.gameObject.transform.position;
            spawnPos.y += 1;
            GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
            //DamageNumber.transform.SetParent(this.gameObject.transform, true);
            DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.clickPoison.ToString();
            DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(floatingNumberColor[1]);

        }

        if (itemScript.itemData.clickHealing != 0)
        {
            spawnPos = this.gameObject.transform.position;
            spawnPos.y += 1;
            GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
            //DamageNumber.transform.SetParent(this.gameObject.transform, true);
            DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.clickHealing.ToString();
            DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(floatingNumberColor[2]);
        }

        if (itemScript.itemData.clickManaUsage != 0)
        {
            spawnPos = this.gameObject.transform.position;
            spawnPos.y += 1;
            GameObject DamageNumber = Instantiate(damageNumber, spawnPos, Quaternion.identity);
            //DamageNumber.transform.SetParent(this.gameObject.transform, true);
            DamageNumber.GetComponentInChildren<TextMeshProUGUI>().text = GetComponent<NewItemScript>().itemData.clickManaUsage.ToString();
            DamageNumber.GetComponent<DamageNumberBehavior>().InitialColor(floatingNumberColor[3]);
        }
    }
}

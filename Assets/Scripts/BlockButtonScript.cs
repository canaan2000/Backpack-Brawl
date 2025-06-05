using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockButtonScript : MonoBehaviour
{
    public PlayerStats playerStats;
    public CombatScript combatScript;

    public GameObject blockCanvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Block(NewItemScript itemScript)
    {
        playerStats.health += itemScript.itemData.damage;
        Destroy(gameObject);
    }

    public void SpawnBlockButton(NewItemScript itemScript)
    {
        GameObject currentCanvas = Instantiate(blockCanvas, combatScript.damageNumberSpawner.transform.position, Quaternion.identity);
        Button currentButton = currentCanvas.GetComponentInChildren<Button>();
        currentButton.onClick.AddListener(() => Block(itemScript));

        Canvas canvas = currentCanvas.GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;

        DamageNumberBehavior Behavior = currentCanvas.GetComponent<DamageNumberBehavior>();
        currentCanvas.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Healing);
    }   
}

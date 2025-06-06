using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BlockButtonScript : MonoBehaviour
{
    public PlayerStats playerStats;
    public CombatScript combatScript;

    public GameObject blockCanvas;
    public RectTransform canvasRect;

    public float blockChance = .5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Block(NewItemScript itemScript, GameObject currentCanvas)
    {
        playerStats.health += itemScript.itemData.damage;
        Destroy(currentCanvas);
    }

    public void SpawnBlockButton(NewItemScript itemScript)
    {
        float rand = Random.Range(0f, 1f);
        if (rand < blockChance)
        {
            GameObject currentCanvas = Instantiate(blockCanvas, combatScript.damageNumberSpawner.transform.position, Quaternion.identity);


            Button currentButton = currentCanvas.GetComponentInChildren<Button>();
            currentButton.onClick.AddListener(() => Block(itemScript, currentCanvas));

            RectTransform buttonRect = currentButton.GetComponent<RectTransform>();

            // Generate random position within the Canvas
            float randomX = Random.Range(-canvasRect.rect.width / 2, canvasRect.rect.width / 2);
            float randomY = Random.Range(-canvasRect.rect.height / 2, canvasRect.rect.height / 2);

            // Set position
            buttonRect.anchoredPosition = new Vector2(randomX, randomY);


            Canvas canvas = currentCanvas.GetComponent<Canvas>();

            DamageNumberBehavior Behavior = currentCanvas.GetComponent<DamageNumberBehavior>();
            currentCanvas.GetComponent<DamageNumberBehavior>().InitialColor(Behavior.currentType = DamageNumberBehavior.numType.Healing);
        }
    }   
}

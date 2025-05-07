using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemySpriteManager : MonoBehaviour
{
    public List<Sprite> sprites;
    public Sprite activeSprite;
    public CombatScript Combat;
    public Image enemySprite;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Combat.combatTrue == true)
        {
            enemySprite.enabled = true;
            activeSprite = sprites[Combat.level - 1];
            enemySprite.sprite = activeSprite;
        }
        else
        {
            enemySprite.enabled = false;
        }
    }
}

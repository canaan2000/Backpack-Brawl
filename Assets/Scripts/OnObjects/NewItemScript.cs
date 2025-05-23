using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(OnMergeScript))]
[RequireComponent(typeof(OnClickManager))]
[RequireComponent(typeof(CollisionDetector))]
[RequireComponent(typeof(MeshCollider))]
[RequireComponent(typeof(DamageNumberSpawner))]


public class NewItemScript : MonoBehaviour
{

    [System.Serializable]
    public class ItemClass
    {
        public string name;
        public string description;
        public float damage;
        public float armor;
        public float poison;
        public float staminaUsage;
        public float clickHealing;
        public float clickHunger;
        public float clickArmor;
        public float clickDamage;
        public float clickPoison;
        public float value;
        public bool singleUse;

        public enum Rarity {Common, Uncommon, Rare};
        public Rarity rarity;

        public enum Class { Basic };
        public Class itemClass;
    }

    public ItemClass itemData;
    // Start is called before the first frame update
    void Start()
    {
        if (itemData == null) // Check if it's already assigned (e.g., in the Inspector)
        {
            itemData = new ItemClass();
            itemData.name = "Default Item Name"; // Set default values.
            itemData.description = "Default Description";
            itemData.damage = 0f;
            itemData.armor = 0f;
            itemData.poison = 0f;
            itemData.staminaUsage = 0f;
            itemData.clickHealing = 0f;
            itemData.clickHunger = 0f;
            itemData.clickArmor = 0f;
            itemData.clickDamage = 0f;
            itemData.clickPoison = 0f;
            itemData.value = 0f;
            itemData.singleUse = false;

            itemData.rarity = ItemClass.Rarity.Common;
            itemData.itemClass = ItemClass.Class.Basic;
        }
        gameObject.name = itemData.name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

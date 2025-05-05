using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewItemScript : MonoBehaviour
{
    [System.Serializable]
    public class ItemClass
    {
        public string name;
        public string description;
        public enum Rarity {Common, Uncommon, Rare};
        public Rarity rarity;
    }

    public ItemClass itemData;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

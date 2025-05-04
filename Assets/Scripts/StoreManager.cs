using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class StoreManager : MonoBehaviour
{
    public CombatScript Combat;
    public BundleCreator BundleCreator;
    // Start is called before the first frame update
    void Start()
    {
        BundleCreator.CreateOptions();
    }

    // Update is called once per frame
    void Update()
    {
        if (Combat.combatTrue == true)
        {
            
        }
    }
}

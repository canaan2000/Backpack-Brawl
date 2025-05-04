using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreCanvasDisp : MonoBehaviour
{
    public CombatScript CombatScript;
    public GameObject storeCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (CombatScript.combatTrue == true) 
        {
            storeCanvas.SetActive(true);
        }
        else
        {
            storeCanvas.SetActive(false);
        }
    }
}

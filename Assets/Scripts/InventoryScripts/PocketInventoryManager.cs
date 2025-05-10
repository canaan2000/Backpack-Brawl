using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PocketInventoryManager : MonoBehaviour
{
    public GameObject storedObj;
     
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!Input.GetMouseButton(0))
        {
            if (storedObj == null)
            {
                storedObj = other.gameObject;
                storedObj.GetComponent<Rigidbody>().isKinematic = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == storedObj) 
        {
            storedObj = null;
        }
    }
}

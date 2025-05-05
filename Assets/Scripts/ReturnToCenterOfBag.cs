using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturnToCenterOfBag : MonoBehaviour
{
    public GameObject backpack;
    public Vector3 returnPos;
    // Start is called before the first frame update
    void Start()
    {
        returnPos = new Vector3 (backpack.transform.position.x, backpack.transform.position.y + 5f, backpack.transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        collision.gameObject.transform.position = returnPos;
    }
}

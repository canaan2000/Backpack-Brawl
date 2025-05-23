using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public MergeMathScript merger;
    public MergeDictionary Dictionary;

    // Start is called before the first frame update
    void Start()
    {
        merger = GameObject.Find("Merge Manager").GetComponent<MergeMathScript>();
        Dictionary = GameObject.Find("Merge Manager").GetComponent<MergeDictionary>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag != "Floor" && collision.transform.tag != "Backpack")   
        {
            GameObject Obj1 = this.gameObject;
            GameObject Obj2 = collision.gameObject;
            Debug.Log(Obj1.tag + " + " + Obj2.tag);
            Dictionary.CheckMerge(Obj1, Obj2);
        }
    }

    private void OnMouseDown()
    {
        if (gameObject.GetComponent<Rigidbody>().isKinematic == true)
        {
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

    }
}

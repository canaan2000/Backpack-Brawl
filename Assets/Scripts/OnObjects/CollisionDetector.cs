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
        Debug.Log("Collision Detected!");
        if (collision.transform.tag != "Floor" && collision.transform.tag != "Backpack")   
        {
            Debug.Log("It is an item!");
            GameObject Obj1 = this.gameObject;
            GameObject Obj2 = collision.gameObject;
            Debug.Log(Obj1.tag + " + " + Obj2.tag);
            Dictionary.CheckMerge(Obj1, Obj2);
        }
    }
}

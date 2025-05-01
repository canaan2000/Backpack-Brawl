using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeMathScript : MonoBehaviour
{
    public GameObject Obj1;
    public GameObject Obj2;

    public bool readyToMerge = true;
    float delay = .05f;
    float timer;

    // Start is called before the first frame update
    void Start()
    {
        timer = delay;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            readyToMerge = true;
            timer = delay;
        }
    }

    public void Merge(GameObject merge1, GameObject merge2, GameObject result, string newName)
    {
        Debug.Log("Trying to merge");
        if (readyToMerge == true)
        {
            Vector3 pos1 = merge1.transform.position;
            Vector3 pos2 = merge2.transform.position;

            Vector3 newPos = Vector3.Lerp(pos1, pos2, .5f);

            GameObject newObject = Instantiate(result, newPos, Quaternion.identity);
            newObject.name = newName;
            //So it will trigger on trigger enter.
            //newObject.GetComponent<Collider>().enabled = false;
            //newObject.GetComponent<Collider>().enabled = true;

            Destroy(merge1); Destroy(merge2);

            readyToMerge = false;
        }
    }
}

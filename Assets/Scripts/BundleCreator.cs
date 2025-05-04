using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BundleCreator : MonoBehaviour
{
    public List<GameObject> common;
    public List<GameObject> uncommon;
    public List<GameObject> rare;

    public List<GameObject> option1;
    public List<GameObject> option2;
    public List<GameObject> option3;

    public List<List<GameObject>> options;

    public int bundleSize = 5;
    // Start is called before the first frame update
    void Start()
    {
        CreateOptions();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateOptions()
    {
        for (int i = 0; i < bundleSize; i++) 
        {
            Debug.Log("Adding Objects");
            option1.Add(common[Random.Range(0, common.Count)]);
            option3.Add(common[Random.Range(0, common.Count)]);
            option2.Add(common[Random.Range(0, common.Count)]);
        }
    }
}

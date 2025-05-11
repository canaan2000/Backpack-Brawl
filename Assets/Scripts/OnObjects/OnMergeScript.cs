using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OnMergeScript : MonoBehaviour
{
    Vector3 objScale;
    public float scaleSpeed = 5f;
    private float timeElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        if (transform.localScale != Vector3.zero)
        {
            objScale = transform.localScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, objScale, Time.deltaTime);
    }

    public void OnMerge()
    {
        objScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }


}

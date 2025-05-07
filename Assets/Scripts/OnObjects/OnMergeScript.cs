using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMergeScript : MonoBehaviour
{
    Vector3 objScale;
    public float scaleSpeed = 5f;
    private float timeElapsed = 0f;

    // Start is called before the first frame update
    void Start()
    {
        objScale = transform.localScale;
        transform.localScale = Vector3.zero; // Set initial scale to zero
    }

    // Update is called once per frame
    void Update()
    {
        timeElapsed += Time.deltaTime;
        float t = timeElapsed * scaleSpeed;

        

        // Linearly interpolate from the initial zero scale to the original scale
        transform.localScale = Vector3.Lerp(Vector3.zero, objScale, t);

        // Optional: You can stop the scaling once it reaches the original size
        if (t >= 1f)
        {
            enabled = false; // Disable the script to save on performance
        }
    }
}

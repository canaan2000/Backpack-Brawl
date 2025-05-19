using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointLightToMouse : MonoBehaviour
{
    public Light light;
    [SerializeField] 
    float distanceFromCamera = 15;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 mousePosScreen = Input.mousePosition;
        light.transform.position = Camera.main.ScreenToWorldPoint(new Vector3(mousePosScreen.x, mousePosScreen.y, distanceFromCamera));
    }
}

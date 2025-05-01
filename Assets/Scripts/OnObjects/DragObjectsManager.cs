using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DragObjectsManager : MonoBehaviour
{
    public LayerMask IgnoreRaycast;


    public GameObject dragObject;
    public Camera CameraMain;
    public GameObject backpack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = CameraMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreRaycast))
            {
                if (hit.collider != null)
                {
                    dragObject = hit.collider.gameObject;
                }
            }
        }

        if (Input.GetMouseButtonUp(0)) 
        {
            dragObject = null;
        }

        if (dragObject != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 18;
            dragObject.transform.position = CameraMain.ScreenToWorldPoint(mousePos);
        }
    }
}

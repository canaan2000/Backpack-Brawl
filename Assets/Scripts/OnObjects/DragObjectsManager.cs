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
            dragObject.GetComponent<Rigidbody>().useGravity = true;
            dragObject = null;
        }

        if (dragObject != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 18;
            dragObject.transform.position = CameraMain.ScreenToWorldPoint(mousePos);
            dragObject.GetComponent<Rigidbody>().useGravity = false;

            

            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                dragObject.transform.rotation = new Quaternion(dragObject.transform.rotation.x + Input.GetAxis("Mouse ScrollWheel"), dragObject.transform.rotation.y, dragObject.transform.rotation.z, dragObject.transform.rotation.w);
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                dragObject.transform.rotation = new Quaternion(dragObject.transform.rotation.x + Input.GetAxis("Mouse ScrollWheel"), dragObject.transform.rotation.y, dragObject.transform.rotation.z, dragObject.transform.rotation.w);
            }
        }
    }
}

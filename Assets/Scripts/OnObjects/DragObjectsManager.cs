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

    float dragSpeed = 2;
    float targetDistance = 1f;
    float dragForceMultiplier = 5f;
    float maxDragMultiplier = 10f;
    float baseDrag = 1f;
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
                    dragObject.GetComponent<Rigidbody>().useGravity = false;
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && dragObject != null) 
        {
            dragObject.GetComponent<Rigidbody>().useGravity = true;
            dragObject.GetComponent<Rigidbody>().drag = 0f;
            dragObject = null;
        }

        if (dragObject != null)
        {
            Rigidbody rb = dragObject.GetComponent<Rigidbody>();
            Vector3 mousePosScreen = Input.mousePosition;
            // Convert mouse position from screen space to world space
            Vector3 mousePosWorld = Camera.main.ScreenToWorldPoint(new Vector3(mousePosScreen.x, mousePosScreen.y, dragObject.transform.position.z - Camera.main.transform.position.z));

            // Calculate the direction vector from the object to the mouse
            Vector3 direction = mousePosWorld - rb.position;
            //find distance from dragobject to mouse
            float distance = Vector3.Distance(mousePosWorld, dragObject.transform.position);
            //apply distance and a multiplier to the force.
            float appliedForceMagnitude = dragSpeed * distance * dragForceMultiplier;

            // Apply a force in that direction
            rb.AddForce(direction.normalized * appliedForceMagnitude, ForceMode.Force);

            //if the object is close to the mouse.
            if (distance <= targetDistance)
            {
                // Calculate a drag multiplier based on proximity.
                float normalizedDistance = Mathf.Clamp01(distance / targetDistance);
                float dragMultiplier = Mathf.Lerp(maxDragMultiplier, 1f, normalizedDistance);

                // Apply the increased drag to the dragged object's Rigidbody.
                rb.drag = baseDrag * dragMultiplier;
                rb.angularDrag = baseDrag * dragMultiplier;
            }
            if (Input.GetAxis("Mouse ScrollWheel") != 0f)
            {
                float rotationAmount = Input.GetAxis("Mouse ScrollWheel") * 100f; // Adjust sensitivity
                rb.angularVelocity = Vector3.up * rotationAmount; // Rotate around Y-axis (you might want to change this)
            }
        }
    }
}

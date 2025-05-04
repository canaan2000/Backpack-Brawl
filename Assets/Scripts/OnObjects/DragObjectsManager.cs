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

    [Header("Dragging Settings")]
    public float dragSpeed = 10f; // Increased for more responsive dragging
    public float dragForceMultiplier = 100f; // Multiplier for the force applied

    [Header("Proximity Drag Settings")]
    public float targetDistance = 3f; // Distance at which drag increase starts
    public float baseDrag = 10f;     // Base drag of the Rigidbody
    public float maxDragMultiplier = 5f; // Maximum multiplier for drag when very close


    // Update is called once per frame
    void Update()
    {
        //if mouse down cast a ray that selects "DragObject"
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = CameraMain.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreRaycast))
            {
                if (hit.collider != null && hit.collider.GetComponent<Rigidbody>() != null) // Ensure the hit object has a Rigidbody
                {
                    dragObject = hit.collider.gameObject;
                    Rigidbody rb = dragObject.GetComponent<Rigidbody>();
                    rb.useGravity = false; // Turn off gravity immediately when picked up
                    rb.drag = baseDrag;     // Reset drag to base value when picked up
                }
                else
                {
                    dragObject = null; // Deselect if no Rigidbody
                }
            }
            else
            {
                dragObject = null; // Deselect if no hit
            }
        }

        //On mouse up turn gravity back on and deselect dragObject (move to end)
        if (Input.GetMouseButtonUp(0) && dragObject != null)
        {
            dragObject.GetComponent<Rigidbody>().useGravity = true;
            dragObject.GetComponent<Rigidbody>().drag = 0;
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
            }
            else
            {
                rb.drag = baseDrag; // Ensure drag resets when further away
            }

            //rotate object with scrollwheel
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                dragObject.transform.Rotate(Vector3.up, -10f); // Rotate around Y-axis
            }
            if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                dragObject.transform.Rotate(Vector3.up, 10f); // Rotate around Y-axis
            }
        }
    }
}
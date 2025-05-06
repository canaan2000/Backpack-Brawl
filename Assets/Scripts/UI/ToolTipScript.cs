using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ToolTipScript : MonoBehaviour
{
    public LayerMask IgnoreRaycast;
    public GameObject selectedGameobject;
    public TextMeshProUGUI tooltip;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~IgnoreRaycast))
        {
            if (hit.collider != null)
            {
                tooltip.enabled = true;
                selectedGameobject = hit.collider.gameObject;
                tooltip.text = selectedGameobject.GetComponent<NewItemScript>().itemData.name + ": \n" + selectedGameobject.GetComponent<NewItemScript>().itemData.description;
            }
        }
        else
        {
            tooltip.enabled = false;
        }
    }
}

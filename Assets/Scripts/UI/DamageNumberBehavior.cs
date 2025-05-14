using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageNumberBehavior : MonoBehaviour
{
    public TextMeshProUGUI damageNumbers;
    float decaySpeed = .5f;
    float speed = 7;
    Color transparency = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce(Vector3.up * speed, ForceMode.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        transparency.a -= Time.deltaTime * decaySpeed;
        damageNumbers.color = transparency;

        if (damageNumbers.color.a <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}

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

    Vector3 randomDirection = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        randomDirection += Vector3.right * Random.Range(0f, 1f);
        randomDirection += Vector3.left * Random.Range(0f, 1f);

        Rigidbody rb = gameObject.GetComponent<Rigidbody>();
        rb.AddForce((Vector3.up * speed) + randomDirection, ForceMode.Impulse);
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

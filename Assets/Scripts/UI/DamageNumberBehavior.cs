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
    Color transparency;

    Vector3 randomDirection = Vector3.zero;
    public enum numType { Damage, Poison, Healing, Thorns, Mana };
    public numType currentType;

    public List<Color> damageColors = new List<Color>();

    public void InitialColor(numType type)
    {
        switch (type)
        {
            case numType.Damage:
                damageNumbers.color = damageColors[0];
                break;
            case numType.Poison:
                damageNumbers.color = damageColors[1];
                break;
            case numType.Healing:
                damageNumbers.color = damageColors[2];
                break;
            case numType.Thorns:
                damageNumbers.color = damageColors[3];
                break;
            case numType.Mana:
                damageNumbers.color = damageColors[4];
                break;
        }
        transparency = damageNumbers.color; // Initialize transparency here
    }


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

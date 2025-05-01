using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayPlayerStats : MonoBehaviour
{
    public PlayerStats PlayerStats;

    public TextMeshProUGUI healthDisp;
    public TextMeshProUGUI armorDisp;
    public TextMeshProUGUI moneyDisp;
    public TextMeshProUGUI attackDisp;
    public TextMeshProUGUI hungerDisp;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthDisp.text = PlayerStats.health.ToString("0");
        armorDisp.text = PlayerStats.armor.ToString("0");
        moneyDisp.text = PlayerStats.money.ToString("$0.00");
        attackDisp.text = PlayerStats.attack.ToString("0.0");
        hungerDisp.text = PlayerStats.hunger.ToString("00");
    }
}

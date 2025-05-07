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
    public TextMeshProUGUI staminaDisp;
    public TextMeshProUGUI poisonDisp;

    float lastHealth = 100;
    float lastArmor;
    float lastStamina;

    Color defaultHealthColor = Color.white;
    Color defaultArmorColor = Color.grey;
    Color defaultStaminaColor = Color.yellow;

    float flashDuration = 0.2f;

    Coroutine currentHealthFlashCoroutine = null;
    Coroutine currentArmorFlashCoroutine = null;
    Coroutine currentStaminaFlashCoroutine = null;

    // Start is called before the first frame update
    void Start()
    {
        if (healthDisp != null) defaultHealthColor = healthDisp.color;
        if (armorDisp != null) defaultArmorColor = armorDisp.color;
        if (staminaDisp != null) defaultStaminaColor = staminaDisp.color;

        lastArmor = PlayerStats.armor;
        lastStamina = PlayerStats.stamina;
    }

    // Update is called once per frame
    void Update()
    {
        if (healthDisp != null) healthDisp.text = PlayerStats.health.ToString("Health: 0");
        if (armorDisp != null) armorDisp.text = PlayerStats.armor.ToString("Armor: 0");
        if (moneyDisp != null) moneyDisp.text = PlayerStats.money.ToString("$0.00");
        if (attackDisp != null) attackDisp.text = PlayerStats.attack.ToString("Auto Attack: 0.0");
        if (hungerDisp != null) hungerDisp.text = PlayerStats.hunger.ToString("Food: 00");
        if (staminaDisp != null) staminaDisp.text = PlayerStats.stamina.ToString("Stamina: 0.000");
        if (poisonDisp != null) poisonDisp.text = PlayerStats.poison.ToString("Poison: 0");

        // Health Flash
        if (healthDisp != null && lastHealth > PlayerStats.health)
        {
            if (currentHealthFlashCoroutine != null) StopCoroutine(currentHealthFlashCoroutine);
            currentHealthFlashCoroutine = StartCoroutine(FlashColor(healthDisp, Color.red, defaultHealthColor));
        }
        lastHealth = PlayerStats.health;

        // Armor Flash
        if (armorDisp != null && lastArmor > PlayerStats.armor)
        {
            if (currentArmorFlashCoroutine != null) StopCoroutine(currentArmorFlashCoroutine);
            currentArmorFlashCoroutine = StartCoroutine(FlashColor(armorDisp, Color.red, defaultArmorColor));
        }
        lastArmor = PlayerStats.armor;

        // Stamina Flash
        if (staminaDisp != null && lastStamina > PlayerStats.stamina)
        {
            if (currentStaminaFlashCoroutine != null) StopCoroutine(currentStaminaFlashCoroutine);
            currentStaminaFlashCoroutine = StartCoroutine(FlashColor(staminaDisp, Color.red, defaultStaminaColor));
        }
        lastStamina = PlayerStats.stamina;
    }

    IEnumerator FlashColor(TextMeshProUGUI textComponent, Color flashColor, Color defaultColor)
    {
        textComponent.color = flashColor;
        yield return new WaitForSeconds(flashDuration);
        textComponent.color = defaultColor;

        // Reset the corresponding coroutine variable
        if (textComponent == healthDisp) currentHealthFlashCoroutine = null;
        else if (textComponent == armorDisp) currentArmorFlashCoroutine = null;
        else if (textComponent == staminaDisp) currentStaminaFlashCoroutine = null;
    }
}
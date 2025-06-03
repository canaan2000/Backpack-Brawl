using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Included for completeness, even if not strictly used with TextMeshProUGUI only

public class DisplayPlayerStats : MonoBehaviour
{
    public PlayerStats PlayerStats; // Assuming PlayerStats is a ScriptableObject or similar

    public TextMeshProUGUI healthDisp;
    public TextMeshProUGUI armorDisp;
    public TextMeshProUGUI moneyDisp;
    public TextMeshProUGUI attackDisp;
    public TextMeshProUGUI staminaDisp;
    public TextMeshProUGUI poisonDisp;
    public TextMeshProUGUI manaDisp;
    public TextMeshProUGUI thornsDisp;

    float lastHealth = 100; // Initialize with a value that ensures first update doesn't flash
    float lastArmor;
    float lastStamina;
    float lastMana; // New: To track changes in mana

    Color defaultHealthColor = Color.white;
    Color defaultArmorColor = Color.grey;
    Color defaultStaminaColor = Color.yellow;
    Color defaultManaColor = Color.blue; // New: Default color for Mana

    float flashDuration = 0.2f;

    Coroutine currentHealthFlashCoroutine = null;
    Coroutine currentArmorFlashCoroutine = null;
    Coroutine currentStaminaFlashCoroutine = null;
    Coroutine currentManaFlashCoroutine = null; // New: Coroutine for Mana flash

    // Start is called before the first frame update
    void Start()
    {
        // Initialize default colors from the TextMeshProUGUI components, if available
        if (healthDisp != null) defaultHealthColor = healthDisp.color;
        if (armorDisp != null) defaultArmorColor = armorDisp.color;
        if (staminaDisp != null) defaultStaminaColor = staminaDisp.color;
        if (manaDisp != null) defaultManaColor = manaDisp.color; // New: Initialize default mana color

        // Initialize last values to prevent flashes on first frame
        lastHealth = PlayerStats.health; // Ensure this is set to current health, not just 100
        lastArmor = PlayerStats.armor;
        lastStamina = PlayerStats.stamina;
        lastMana = PlayerStats.mana; // New: Initialize lastMana
    }

    // Update is called once per frame
    void Update()
    {
        // Display updates for all stats
        if (healthDisp != null) healthDisp.text = $"<sprite=1>{PlayerStats.health:0}";
        if (armorDisp != null) armorDisp.text = $"Armor: {PlayerStats.armor:0}";
        if (moneyDisp != null) moneyDisp.text = $"${PlayerStats.money:0}";
        if (attackDisp != null) attackDisp.text = $"<sprite=0>{PlayerStats.attack:0.0}"; // Assuming <sprite=0> for attack
        if (staminaDisp != null) staminaDisp.text = $"<sprite=4>{PlayerStats.stamina:0.000}";
        if (poisonDisp != null) poisonDisp.text = $"<sprite=3>{PlayerStats.poison:0}";
        if (manaDisp != null) manaDisp.text = $"<sprite=2>{PlayerStats.mana:0}";
        if (thornsDisp != null) thornsDisp.text = $"<sprite=5>{PlayerStats.thorns:0}";

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

        // Mana Flash (New functionality)
        if (manaDisp != null && lastMana > PlayerStats.mana) // If mana has decreased
        {
            if (currentManaFlashCoroutine != null) StopCoroutine(currentManaFlashCoroutine); // Stop any existing flash
            currentManaFlashCoroutine = StartCoroutine(FlashColor(manaDisp, Color.blue, defaultManaColor)); // Flash blue
        }
        lastMana = PlayerStats.mana; // Update lastMana for the next frame
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
        else if (textComponent == manaDisp) currentManaFlashCoroutine = null; // New: Reset Mana coroutine
    }
}
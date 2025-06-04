using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnClickManager : MonoBehaviour
{
    public CombatScript Combat;
    public DamageNumberSpawner NumberSpawner;

    public float clickCooldown = 1f;
    public bool readyToClick = true; // Tracks if *this specific object* is ready to be clicked/used
    public float cooldown;

    void Start()
    {
        // Find the CombatManager in the scene and get its CombatScript component.
        // This assumes there is only one GameObject named "CombatManager" in the scene.
        Combat = GameObject.Find("CombatManager")?.GetComponent<CombatScript>();
        if (Combat == null)
        {
            Debug.LogError("OnClickManager: CombatManager GameObject or CombatScript component not found!");
        }

        // Get the DamageNumberSpawner component attached to the same GameObject as this script.
        NumberSpawner = GetComponent<DamageNumberSpawner>();
        if (NumberSpawner == null)
        {
            Debug.LogWarning($"OnClickManager: DamageNumberSpawner component not found on {gameObject.name}. Damage numbers will not spawn.");
        }

        // Initialize the cooldown for this specific object.
        cooldown = clickCooldown;
    }

    void Update()
    {
        // --- Cooldown Management for this specific object ---
        // If the object is currently not ready to click, decrement its cooldown.
        if (!readyToClick)
        {
            cooldown -= Time.deltaTime; // Decrease cooldown over time.

            // If cooldown has finished, set the object as ready to click again.
            if (cooldown <= 0)
            {
                readyToClick = true;
                cooldown = clickCooldown; // Reset cooldown for the next cycle
                // Debug.Log($"{gameObject.name} is now ready to click again.");
            }
        }

        // --- Enemy AI "Click" Logic ---
        // This block handles when an enemy item 'uses' itself automatically.
        // It only runs if this GameObject is tagged as "EnemyItem" AND it's currently ready to click.
        if (this.transform.tag == "EnemyItem" && readyToClick)
        {
            // Attempt to perform the enemy's action.
            // EnemyClick() will internally check for resources and combat state.
            // If EnemyClick() successfully performs an action, it will set readyToClick = false.
            EnemyClick();
        }

        // IMPORTANT: Removed Input.GetMouseButtonDown(0) from Update().
        // This was the most likely cause of inconsistent enemy clicks.
        // If it was here, any player mouse click would set 'readyToClick = false'
        // for ALL instances of OnClickManager, including enemy items,
        // preventing them from ever becoming ready if the player was clicking frequently.
        // Player clicks are handled exclusively by OnMouseDown().
    }

    // --- Player Click Logic ---
    // This Unity message is called when the mouse button is pressed over this collider.
    private void OnMouseDown()
    {
        // Debug.Log($"OnMouseDown called on {gameObject.name}. readyToClick: {readyToClick}, Tag: {this.tag}");

        // Ensure this object is ready to be clicked by the player AND it's not an "EnemyItem".
        if (readyToClick && this.tag != "EnemyItem")
        {
            // Get the NewItemScript component from this GameObject to access its item data.
            NewItemScript itemScript = gameObject.GetComponent<NewItemScript>();
            if (itemScript == null)
            {
                Debug.LogWarning($"OnMouseDown: {gameObject.name} is missing NewItemScript. Cannot process click.");
                return; // Exit if no item script is found.
            }

            // Check if player has sufficient stamina and mana, and if combat is active.
            if (Combat != null && Combat.PlayerStats != null && Combat.EnemyStats != null &&
                Combat.PlayerStats.stamina >= itemScript.itemData.staminaUsage &&
                Combat.PlayerStats.mana >= itemScript.itemData.clickManaUsage &&
                Combat.combatTrue == true)
            {
                // Player action successful:
                readyToClick = false; // Set this object to not ready (start cooldown)
                cooldown = clickCooldown; // Reset the cooldown for this player item.

                // Apply item effects to player/enemy stats.
                Combat.PlayerStats.armor += itemScript.itemData.clickArmor;
                Combat.EnemyStats.Health -= itemScript.itemData.clickDamage;
                Combat.EnemyStats.Poison += itemScript.itemData.clickPoison;
                Combat.PlayerStats.health += itemScript.itemData.clickHealing;

                // Deduct resources from player.
                Combat.PlayerStats.stamina -= itemScript.itemData.staminaUsage;
                Combat.PlayerStats.mana -= itemScript.itemData.clickManaUsage;

                // Spawn damage numbers if the spawner exists.
                NumberSpawner?.OnClickSpawnNumber(); // Null-conditional operator for safety

                // If the item is single-use, destroy it after use.
                if (itemScript.itemData.singleUse == true)
                {
                    Debug.Log($"Destroying player single-use item: {gameObject.name}");
                    Destroy(gameObject);
                }
            }
            else
            {
                // Log why the player click failed (uncomment for detailed debugging)
                // Debug.Log($"Player click failed for {gameObject.name}. " +
                //           $"Stamina: {Combat.PlayerStats?.stamina ?? 0}/{itemScript.itemData.staminaUsage}, " +
                //           $"Mana: {Combat.PlayerStats?.mana ?? 0}/{itemScript.itemData.clickManaUsage}, " +
                //           $"Combat Active: {Combat?.combatTrue}");
            }
        }
    }

    // --- Enemy Action Logic ---
    // This method is called by the Update() loop for "EnemyItem" tagged GameObjects.
    private void EnemyClick()
    {
        // Debug.Log($"EnemyClick called on {gameObject.name}. readyToClick: {readyToClick}, Tag: {this.tag}");

        // Get the NewItemScript component.
        NewItemScript itemScript = gameObject.GetComponent<NewItemScript>();
        if (itemScript == null)
        {
            Debug.LogWarning($"EnemyClick: {gameObject.name} is missing NewItemScript. Cannot perform action.");
            return;
        }

        // Check if the enemy has sufficient resources, if combat is active,
        // and if this object is indeed tagged as "EnemyItem".
        if (Combat != null && Combat.EnemyStats != null && Combat.PlayerStats != null &&
            Combat.EnemyStats.stamina >= itemScript.itemData.staminaUsage &&
            Combat.EnemyStats.mana >= itemScript.itemData.clickManaUsage &&
            Combat.combatTrue == true &&
            this.tag == "EnemyItem") // Double-check tag for safety, though Update() already filters this.
        {
            // Enemy action successful:
            readyToClick = false; // Set this object to not ready (start cooldown)
            // The cooldown is implicitly reset in Update() when readyToClick becomes true again.

            // Apply item effects to the Player (since it's an enemy item) or heal enemy.
            Combat.PlayerStats.armor += itemScript.itemData.clickArmor;
            Combat.PlayerStats.health -= itemScript.itemData.clickDamage; // Enemy deals damage to Player
            Combat.PlayerStats.poison += itemScript.itemData.clickPoison; // Enemy applies poison to Player
            Combat.EnemyStats.Health += itemScript.itemData.clickHealing; // Enemy heals itself

            // Deduct stamina and mana from the Enemy.
            Combat.EnemyStats.stamina -= itemScript.itemData.staminaUsage;
            Combat.EnemyStats.mana -= itemScript.itemData.clickManaUsage;

            // Spawn damage numbers.
            NumberSpawner?.OnClickSpawnNumber(); // Null-conditional operator for safety

            // If the item is single-use, destroy it after use.
            if (itemScript.itemData.singleUse == true)
            {
                Debug.Log($"Destroying enemy single-use item: {gameObject.name}");
                Destroy(gameObject);
            }
        }
    }
}
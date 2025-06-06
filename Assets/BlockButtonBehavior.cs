using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Make sure this is included for the Button component

public class BlockButtonBehavior : MonoBehaviour
{
    public Button button; // Reference to the Button component, assigned in Start()

    // Define animation parameters
    public float scaleUpDuration = 0.5f; // Time for scaling up
    public float scaleDownDuration = 0.5f; // Time for scaling down
    public float peakScaleMultiplier = 1.2f; // How much larger than its original size the button should get

    private Vector3 initialScale; // Stores the button's scale when it starts
    private Vector3 peakScale;    // The target scale for the first phase of the animation

    // Start is called before the first frame update
    void Start()
    {
        // Get the Button component from children. 
        // This assumes BlockButtonBehavior is on the parent Canvas, and the Button is a child.
        button = GetComponentInChildren<Button>();

        if (button == null)
        {
            Debug.LogError("Button component not found as a child of " + gameObject.name + ". Cannot animate scale.");
            Destroy(gameObject); // Destroy the canvas if no button is found to prevent issues
            return;
        }

        // Store the button's initial scale
        initialScale = button.transform.localScale;
        // Calculate the peak scale based on the initial scale and multiplier
        peakScale = initialScale * peakScaleMultiplier;

        // Start the scale animation coroutine
        StartCoroutine(AnimateScale());
    }

    // Coroutine to handle the scaling animation
    private IEnumerator AnimateScale()
    {
        float timer = 0f; // Timer for tracking animation progress

        // --- Phase 1: Scale Up ---
        while (timer < scaleUpDuration)
        {
            timer += Time.deltaTime; // Increment timer by time since last frame
            // Lerp (Linear Interpolation) from initialScale to peakScale
            // The 't' value (timer / scaleUpDuration) goes from 0 to 1
            button.transform.localScale = Vector3.Lerp(initialScale, peakScale, timer / scaleUpDuration);
            yield return null; // Wait until the next frame before continuing
        }
        // Ensure the button reaches the exact peak scale at the end of the phase
        button.transform.localScale = peakScale;

        // --- Phase 2: Scale Down to Zero ---
        timer = 0f; // Reset timer for the new phase
        // The starting scale for this phase is the peakScale we just reached
        Vector3 startScaleForDown = peakScale;

        while (timer < scaleDownDuration)
        {
            timer += Time.deltaTime; // Increment timer
            // Lerp from peakScale down to Vector3.zero (no scale)
            button.transform.localScale = Vector3.Lerp(startScaleForDown, Vector3.zero, timer / scaleDownDuration);
            yield return null; // Wait until the next frame
        }

        // Ensure the button's scale is exactly zero to avoid floating point inaccuracies
        button.transform.localScale = Vector3.zero;

        // --- Final: Destroy the GameObject ---
        Debug.Log("Scale animation complete. Destroying: " + gameObject.name);
        Destroy(gameObject); // Destroys the GameObject this script is attached to (your canvas/button)
    }

    // Update is called once per frame (not used for this animation)
    void Update()
    {

    }
}
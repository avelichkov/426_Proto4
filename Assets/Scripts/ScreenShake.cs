using System.Collections;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{   
    // Strength of the shake effect
    private float shakeMagnitude = 0.1f;
    
    // How quickly the shake effect should fade
    private float dampingSpeed = 0f;

    // Original position of the camera
    private Vector3 initialPosition;
    
    // Time left for the shake effect
    private float shakeTimeRemaining;

    private void Start()
    {
        // Store the initial position of the camera
        initialPosition = transform.localPosition;
    }

    private void Update()
    {
        // If there is shake time remaining, shake the camera
        if (shakeTimeRemaining > 0)
        {
            // Create a random offset for the shake
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;

            // Reduce the shake time remaining and apply damping to the shake duration
            shakeTimeRemaining -= Time.deltaTime * dampingSpeed;
        }
        else
        {
            // If no shake time remains, reset the camera position
            shakeTimeRemaining = 0;
            transform.localPosition = initialPosition;
        }
    }

    // Call this method to start the screen shake
    public void TriggerShake(float duration)
    {
        shakeTimeRemaining = duration;
        StartCoroutine(ShakeCamera());
    }

    private IEnumerator ShakeCamera()
    {
        while (shakeTimeRemaining > 0)
        {
            transform.localPosition = initialPosition + Random.insideUnitSphere * shakeMagnitude;
            shakeTimeRemaining -= Time.unscaledDeltaTime * dampingSpeed;
            yield return null;
        }
        // If no shake time remains, reset the camera position
        shakeTimeRemaining = 0;
        transform.localPosition = initialPosition;
    }
}
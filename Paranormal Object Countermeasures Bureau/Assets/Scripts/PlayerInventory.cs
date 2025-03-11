using UnityEngine;

public class FlashlightInventory : MonoBehaviour
{
    public GameObject flashlight; // The flashlight GameObject (the light or the model)
    public KeyCode toggleKey = KeyCode.Alpha1; // The key to toggle flashlight (default is '1')

    private bool isFlashlightEquipped = false; // Track if the flashlight is currently equipped or not

    private void Start()
    {
        // Ensure the flashlight is initially turned off
        flashlight.SetActive(false);
    }

    private void Update()
    {
        // Check if the player presses the designated key (e.g., 1)
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleFlashlight();
        }
    }

    // Method to toggle the flashlight on or off
    private void ToggleFlashlight()
    {
        // Switch the flashlight state (on/off)
        isFlashlightEquipped = !isFlashlightEquipped;
        flashlight.SetActive(isFlashlightEquipped); // Turn flashlight on or off
    }
}

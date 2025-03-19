using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework;

public class FlashlightInventory : MonoBehaviour
{
    [Header("Inventory Items")]
    public GameObject flashlight;
    public GameObject offhandLight;
    public GameObject axe;

    bool isSwinging = false;
    public GameObject drill;
    public Light flashlightLight;

    [Header("Player Settings")]
    public bool isFlashlightEquipped = false;
    public bool isAxeEquipped = false;
    public bool isFlashlightOn = false;
    public bool axeObtained = false;
    public bool drillObtained = false;

    [Header("Axe Settings")]
    public float axeSwingDistance = 2f; // How far the axe can hit
    public LayerMask boardLayer; // Set this to a layer for board objects

    private void Start()
    {
        flashlight.SetActive(false);
        offhandLight.SetActive(false);
        axe.SetActive(false);
        drill.SetActive(false);
        flashlightLight.enabled = false;
    }

    private void Update()
    {
        // Weapon Selection
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            EquipFlashlight();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && axeObtained)
        {
            EquipAxe();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3) && drillObtained)
        {
            EquipPowerDrill();
        }

        // Flashlight Toggle
        if (isFlashlightEquipped && Input.GetKeyDown(KeyCode.Mouse0))
        {
            ToggleFlashlightLight();
        }

        // Axe Swing + Board Destruction
        if (isAxeEquipped && Input.GetKeyDown(KeyCode.Mouse0) && !isSwinging)
        {
            SwingAxe();
        }
    }

    private void EquipFlashlight()
    {
        isFlashlightEquipped = true;
        isAxeEquipped = false;

        flashlight.SetActive(true);
        offhandLight.SetActive(false);
        axe.SetActive(false);
        drill.SetActive(false);

        flashlightLight.enabled = isFlashlightOn;
    }

    private void EquipAxe()
    {
        isAxeEquipped = true;

        axe.SetActive(true);
        drill.SetActive(false);
        offhandLight.SetActive(isFlashlightOn);
        flashlight.SetActive(false);
    }

    private void EquipPowerDrill()
    {
        isAxeEquipped = false;

        drill.SetActive(true);
        axe.SetActive(false);
        offhandLight.SetActive(isFlashlightOn);
        flashlight.SetActive(false);
    }

    private void ToggleFlashlightLight()
    {
        isFlashlightOn = !isFlashlightOn;
        flashlightLight.enabled = isFlashlightOn;
    }

private void SwingAxe()
{
    StartCoroutine(AxeSwingAnimation()); // Play swing animation

    // Check if the player is inside a board's trigger zone
    Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // Adjust radius
    foreach (Collider col in hitColliders)
    {
        BreakBoard board = col.GetComponent<BreakBoard>();
        if (board != null && board.IsPlayerInside())
        {
            Destroy(board.gameObject); // Deactivate board
            break;
        }
    }
}

    private IEnumerator AxeSwingAnimation()
    {
        isSwinging = true;

        float swingAngle = 60f;
        float swingSpeed = 0.2f;

        Quaternion startRotation = axe.transform.localRotation;
        Quaternion swingDownRotation = Quaternion.Euler(startRotation.eulerAngles.x + swingAngle, startRotation.eulerAngles.y, startRotation.eulerAngles.z);

        float elapsedTime = 0f;

        while (elapsedTime < swingSpeed)
        {
            axe.transform.localRotation = Quaternion.Lerp(startRotation, swingDownRotation, elapsedTime / swingSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        axe.transform.localRotation = swingDownRotation;

        elapsedTime = 0f;
        while (elapsedTime < swingSpeed)
        {
            axe.transform.localRotation = Quaternion.Lerp(swingDownRotation, startRotation, elapsedTime / swingSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        axe.transform.localRotation = startRotation;
        isSwinging = false;
    }
}


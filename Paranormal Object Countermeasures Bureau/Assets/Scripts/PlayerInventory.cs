using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework;
using Unity.VisualScripting;



public class PlayerInventory : MonoBehaviour
{
    [Header("Inventory Items")]
    public GameObject flashlight;
    public GameObject offhandLight;
    public GameObject axe;

    bool isSwinging = false;
    public GameObject drill;
    public Light flashlightLight;

    public Light dimVentLight;

    //SOUNDS
    public AudioSource ItemAudioSource;
    [SerializeField] private AudioClip flashlightSound;
    [SerializeField] private AudioClip axeSound;

    public AudioSource AmbientAudioSource;

    [SerializeField] private AudioClip ambientBGM;

    [SerializeField] private AudioClip redRoomBGM;

    [SerializeField] private AudioClip chaseBGM;

    [SerializeField] private AudioClip heartbeat;





    [Header("Player Settings")]
    public bool isFlashlightEquipped = false;
    public bool isAxeEquipped = false;
    public bool isDrillEquipped = false;
    public bool isFlashlightOn = false;
    public bool axeObtained = false;
    public bool drillObtained = false;
    public bool keyObtained = false;

    [Header("Axe Settings")]
    public float axeSwingDistance = 2f; //How far the axe can hit
    //public LayerMask boardLayer; 

    bool inVent = false; //is the player in a vent
    bool inRedRoom = false; //is the player in the red room

    private void Start()
    {
        flashlight.SetActive(false);
        offhandLight.SetActive(false);
        axe.SetActive(false);
        drill.SetActive(false);
        flashlightLight.enabled = false; //flash light
        dimVentLight.enabled = false; //vent light

        //play ambient bgm
        AmbientAudioSource.clip = ambientBGM;
        AmbientAudioSource.Play();
        AmbientAudioSource.loop = true;
    }

    private void Update()
    {
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

        if (isFlashlightEquipped && Input.GetKeyDown(KeyCode.Mouse0))
        {
            ToggleFlashlightLight();
        }

        if (isAxeEquipped && Input.GetKeyDown(KeyCode.Mouse0) && !isSwinging)
        {
            SwingAxe();
        }
        if (inVent){
            flashlightLight.enabled = false; //turn off flashlight in vent
            offhandLight.SetActive(false); //turn off offhand light in vent
            isFlashlightOn = false;
            AmbientAudioSource.Stop(); //stop music in vent
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
        isDrillEquipped = true;

        drill.SetActive(true);
        axe.SetActive(false);
        offhandLight.SetActive(isFlashlightOn);
        flashlight.SetActive(false);
    }

    private void ToggleFlashlightLight()
    {
        //audioSource.PlayOneShot(flashlightSound);

        if (inVent || inRedRoom) return; //don't toggle flashlight in vent

        //increase volume of flashlight sound
        ItemAudioSource.PlayOneShot(flashlightSound, 1.5f);

        isFlashlightOn = !isFlashlightOn;
        flashlightLight.enabled = isFlashlightOn;
    }

    private void SwingAxe()
    {
        StartCoroutine(AxeSwingAnimation());

        // Check if the player is inside a board's trigger zone
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 1f); // Adjust radius
        foreach (Collider col in hitColliders)
        {
<<<<<<< Updated upstream
            board.SetKinematic();
            col.isTrigger = false;
            break;
=======
            BreakBoard board = col.GetComponent<BreakBoard>();
            if (board != null && board.IsPlayerInside())
            {
                board.SetKinematic();
                col.isTrigger = false;
                break;
            }
>>>>>>> Stashed changes
        }
    }

    private IEnumerator AxeSwingAnimation()
    {
        isSwinging = true;
        ItemAudioSource.PlayOneShot(axeSound);

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

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("VentEvent"))
        {
            inVent = true;
            AmbientAudioSource.Stop(); //stop music in vent

            //turn off flashlight and turn on vent light
            flashlightLight.enabled = false;
            offhandLight.SetActive(false); //turn off offhand light in vent
            isFlashlightOn = false;
            //play vent sound

            dimVentLight.enabled = true;
        }
        if (other.CompareTag("VentScare") && keyObtained)//play scare returning to vent after player has key
        {
            Debug.Log("Scare sound played");
            //play scare sound
        }
        if (other.CompareTag("RedRoom")){
            inRedRoom = true;
            Debug.Log("Entered red room");

            flashlightLight.enabled = false;
            offhandLight.SetActive(false);
            isFlashlightOn = false;

            //play ambient sound and stop normal bgm
            AmbientAudioSource.Stop(); //stop normal bgm
            AmbientAudioSource.clip = redRoomBGM;
            //AmbientAudioSource.volume = 1.5f; //increase volume
            AmbientAudioSource.Play();
            AmbientAudioSource.loop = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("VentEvent"))
        {
            inVent = false;
            AmbientAudioSource.Play(); //play music when leaving vent
            //turn off vent light, let player toggle flashlight again
            dimVentLight.enabled = false;
        }
        if (other.CompareTag("RedRoom")){
            Debug.Log("Leaving red room");
            inRedRoom = false;
            //stop red room bgm and play normal bgm
            AmbientAudioSource.Stop();
            AmbientAudioSource.clip = ambientBGM;
            //AmbientAudioSource.volume = 1.0f; //reset volume
            AmbientAudioSource.Play();
            AmbientAudioSource.loop = true;
        }
    }
}


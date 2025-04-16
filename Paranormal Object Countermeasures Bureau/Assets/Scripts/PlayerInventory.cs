using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework;
using Unity.VisualScripting;
using System;



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
    [SerializeField] private AudioClip drillSoundItem;

    public AudioSource AmbientAudioSource;

    [SerializeField] private AudioClip ambientBGM;

    [SerializeField] private AudioClip redRoomBGM;

    [SerializeField] private AudioClip chaseBGM;

    [SerializeField] private AudioClip heartbeat;

    public AudioSource MiscEventAudioSource;

    [SerializeField] private AudioClip BellChime;
    [SerializeField] private AudioClip VentScare;


    //MISC OBJECTS
    public GameObject VentScareObj;

    public GameObject BellChimeObj;

    public GameObject facultyNoteObj;

    public GameObject MonsterHallway; //spawnable monster in hallway after key
    public GameObject Monster1B; //spawnable monster after vent

    //array of barrier objects
    public GameObject[] barriers;


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

    public bool inVent = false; //is the player in a vent
    bool inRedRoom = false; //is the player in the red room

    private bool canPlayDrillSound = true; // Flag to control drill sound cooldown

    private void Start() //UPON START
    {
        //show initial objective and dialogue
        FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I've got to find the missing student here...",
            () => FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("First I should head to the faculty office for any clues.",
                () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Head to the faculty office")
            )
        );
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

        if (isDrillEquipped && Input.GetKeyDown(KeyCode.Mouse0))
        {
            DrillSound();
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
        isDrillEquipped = false;

        flashlight.SetActive(true);
        offhandLight.SetActive(false);
        axe.SetActive(false);
        drill.SetActive(false);

        flashlightLight.enabled = isFlashlightOn;
    }

    private void EquipAxe()
    {
        isAxeEquipped = true;
        isFlashlightEquipped = false;
        isDrillEquipped = false;

        axe.SetActive(true);
        drill.SetActive(false);
        offhandLight.SetActive(isFlashlightOn);
        flashlight.SetActive(false);
    }

    private void EquipPowerDrill()
    {
        isDrillEquipped = true;
        isFlashlightEquipped = false;
        isAxeEquipped = false;

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

    private void DrillSound()
    {
        if (!canPlayDrillSound) return; //exit if the cooldown is active

        ItemAudioSource.PlayOneShot(drillSoundItem, 1.5f);

        //disable further sound playing and start cooldown
        canPlayDrillSound = false;
        Invoke(nameof(ResetDrillSoundCooldown), 2f); //reset cooldown after 2 seconds
    }

    private void ResetDrillSoundCooldown()
    {
        canPlayDrillSound = true; //re-enable drill sound
    }

    private void SwingAxe()
    {
        StartCoroutine(AxeSwingAnimation());        
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
        //play bell sound when entering bell trigger and note is not active
        if (other.CompareTag("BellChime") && facultyNoteObj.activeSelf == false)
        {
            Debug.Log("Bell sound played");
            MiscEventAudioSource.clip = BellChime;
            MiscEventAudioSource.PlayOneShot(BellChime);
            BellChimeObj.SetActive(false); //turn off bell chime object
            //remove all barriers
            foreach (GameObject barrier in barriers)
            {
                barrier.SetActive(false);
            }
            FindAnyObjectByType<ThoughtDialogueManager>().ShowThought("  The bell chime..? I should check it out...",
                () => FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("Just have to watch out for Paranormal Objects...",
                () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Investigate the school.")
                )
            );
        }
        if (other.CompareTag("VentEvent"))
        {
            inVent = true;
            AmbientAudioSource.Stop(); //stop music in vent
            RenderSettings.fog = false;

            //turn off flashlight and turn on vent light
            flashlightLight.enabled = false;
            offhandLight.SetActive(false); //turn off offhand light in vent
            isFlashlightOn = false;
            dimVentLight.enabled = true;
        }
        if (other.CompareTag("VentScare") && keyObtained)//play scare returning to vent after player has key
        {
            RenderSettings.fog = false;
            Debug.Log("Scare sound played");
            MiscEventAudioSource.clip = VentScare;
            MiscEventAudioSource.volume = 2f; //increase volume of scare
            MiscEventAudioSource.PlayOneShot(VentScare);
            VentScareObj.SetActive(false); //turn off vent scare object

        }
        if (other.CompareTag("RedRoom")){
            inRedRoom = true;
            Debug.Log("Entered red room");
            RenderSettings.fog = false;

            flashlightLight.enabled = false;
            offhandLight.SetActive(false);
            isFlashlightOn = false;

            //play ambient sound and stop normal bgm
            AmbientAudioSource.Stop(); //stop normal bgm
            AmbientAudioSource.clip = redRoomBGM;
            AmbientAudioSource.volume = 1.3f; //increase volume
            AmbientAudioSource.Play();
            AmbientAudioSource.loop = true;
        }
        if (other.CompareTag("Board") && isAxeEquipped && axeObtained && isSwinging){//remove board if axe is equipped and swing
            Debug.Log("Destroying board");
            Destroy(other.gameObject);
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

            if (keyObtained) //on the way out after key
                RenderSettings.fog = true;
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
            
            if (keyObtained) //on the way out after key
                RenderSettings.fog = true;
        }
    }

    public bool getObtained(string item){
        if(item == "axe"){
            return axeObtained;
        }
        if(item == "drill"){
            return drillObtained;
        }
        return false;
    }

    public bool isEquipped(string item){
        if (item == "axe")
        {
            return isAxeEquipped;
        }
        if (item == "drill")
        {
            return isDrillEquipped;
        }
        if (item == "flashlight"){
            return isFlashlightEquipped;
        }
        return false;
    }
}


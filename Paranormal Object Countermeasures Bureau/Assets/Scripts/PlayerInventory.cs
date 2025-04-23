using UnityEngine;
using System.Collections;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using NUnit.Framework;
using Unity.VisualScripting;
using System;
using UnityEngine.SceneManagement;



public class PlayerInventory : MonoBehaviour
{

    public bool secondFloor; //true if the player is on the second floor, false if on the first floor

    [Header("Inventory Items")]
    public GameObject flashlight;
    public GameObject offhandLight;
    public GameObject axe;
    public GameObject lighter;
    public GameObject fuelCanister;
    public GameObject cloth; 

    bool isSwinging = false;
    public GameObject drill;
    public Light flashlightLight;
    public Light lighterLight;

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

    public GameObject artNoteObj;//SECOND FLOOR NOTE

    public GameObject MonsterHallway; //spawnable monster in hallway after key
    public GameObject Monster1B; //spawnable monster after vent

    public GameObject MainMonster; //main monster object

    //array of barrier objects
    public GameObject[] barriers;


    [Header("Player Settings")]
    public bool isFlashlightEquipped = false;
    public bool isAxeEquipped = false;
    public bool isDrillEquipped = false;
    public bool isFlashlightOn = false;
    public bool isLighterOn = false;
    public bool isLighterEquipped = false;
    public bool isFuelCanisterEquipped = false;
    public bool isClothEquipped = false; 
    public bool axeObtained = false;
    public bool drillObtained = false;
    public bool keyObtained = false;
    public bool lighterObtained = false;
    public bool fuelcanObtained = false;
    public bool clothObtained = false; 

    [Header("Axe Settings")]
    public float axeSwingDistance = 2f; //How far the axe can hit
    //public LayerMask boardLayer; 

    public bool inVent = false; //is the player in a vent
    bool inRedRoom = false; //is the player in the red room

    private bool canPlayDrillSound = true; // Flag to control drill sound cooldown

    private void Start() //UPON START
    {
        //show initial objective and dialogue
        if (!secondFloor){
        FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I've got to find the missing student here...",
            () => FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("First I should head to the faculty office for any clues.",
                () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Head to the faculty office")
            )
        );
        }
        if (secondFloor){
        FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("The missing student must be up here somewhere...",
            () => FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("Maybe they had some connection to the past events here?",
                () => FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I'll go and check out the art room up ahead.",
                () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Head to the art room at the end of the hall")
            )
        ));
        }
        flashlight.SetActive(false);
        offhandLight.SetActive(false);
        axe.SetActive(false);
        drill.SetActive(false);
        lighter.SetActive(false);
        fuelCanister.SetActive(false); 
        cloth.SetActive(false); 
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
        if (Input.GetKeyDown(KeyCode.Alpha4) && lighterObtained)
        {
            EquipLighter();
        }
        if (Input.GetKeyDown(KeyCode.Alpha5) && fuelcanObtained)
        {
            EquipFuelCanister();
        }
        if (Input.GetKeyDown(KeyCode.Alpha6) && clothObtained)
        {
            EquipCloth();
        }

        if (isFlashlightEquipped && Input.GetKeyDown(KeyCode.Mouse0))
        {
            ToggleFlashlightLight();
        }

        if (isLighterEquipped && Input.GetKeyDown(KeyCode.Mouse0))
        {
            ToggleLighterLight();
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
            isLighterOn = false; //turn off lighter in vent
            lighterLight.enabled = isLighterOn;
            AmbientAudioSource.Stop(); //stop music in vent
        }
    }

    private void EquipFlashlight()
    {
        isFlashlightEquipped = true;
        isAxeEquipped = false;
        isDrillEquipped = false;
        isLighterEquipped = false;
        isLighterOn = false; 
        //lighterLight.enabled = isLighterOn;
        isFuelCanisterEquipped = false; //turn off fuel canister
        isClothEquipped = false;
        
        cloth.SetActive(false); 
        flashlight.SetActive(true);
        offhandLight.SetActive(false);
        axe.SetActive(false);
        drill.SetActive(false);
        lighter.SetActive(false);
        fuelCanister.SetActive(false);

        flashlightLight.enabled = isFlashlightOn;
    }

    private void EquipAxe()
    {
        isAxeEquipped = true;
        isFlashlightEquipped = false;
        isDrillEquipped = false;
        isLighterEquipped = false;
        isLighterOn = false; 
        //lighterLight.enabled = isLighterOn;
        isFuelCanisterEquipped = false;
        isClothEquipped = false;
        
        cloth.SetActive(false); 
        axe.SetActive(true);
        drill.SetActive(false);
        offhandLight.SetActive(isFlashlightOn);
        flashlight.SetActive(false);
        lighter.SetActive(false);
        fuelCanister.SetActive(false);
    }

    private void EquipPowerDrill()
    {
        isDrillEquipped = true;
        isFlashlightEquipped = false;
        isAxeEquipped = false;
        isLighterEquipped = false;
        isLighterOn = false; 
        //lighterLight.enabled = isLighterOn;
        isFuelCanisterEquipped = false;
        isClothEquipped = false;
        
        cloth.SetActive(false); 
        drill.SetActive(true);
        axe.SetActive(false);
        offhandLight.SetActive(isFlashlightOn);
        flashlight.SetActive(false);
        lighter.SetActive(false);
        fuelCanister.SetActive(false);
    }

    private void EquipLighter()
    {
        isLighterEquipped = true;
        isFlashlightEquipped = false;
        isAxeEquipped = false;
        isDrillEquipped = false;
        isFuelCanisterEquipped = false;
        isClothEquipped = false;
        
        cloth.SetActive(false); 
        lighter.SetActive(true);
        flashlight.SetActive(false);
        offhandLight.SetActive(false);
        axe.SetActive(false);
        drill.SetActive(false);
        fuelCanister.SetActive(false);

        isLighterOn = false; // Lighter starts off
        lighterLight.enabled = isLighterOn; // Ensure the lighter light is off
    }

    private void EquipFuelCanister()
    {
        isFuelCanisterEquipped = true;
        isFlashlightEquipped = false;
        isAxeEquipped = false;
        isDrillEquipped = false;
        isLighterEquipped = false;
        isClothEquipped = false;

        cloth.SetActive(false); 
        fuelCanister.SetActive(true);
        flashlight.SetActive(false);
        offhandLight.SetActive(false);
        axe.SetActive(false);
        drill.SetActive(false);
        lighter.SetActive(false);
    }

    private void EquipCloth()
    {
        isClothEquipped = true;
        isFlashlightEquipped = false;
        isAxeEquipped = false;
        isDrillEquipped = false;
        isLighterEquipped = false;
        isFuelCanisterEquipped = false;

        cloth.SetActive(true);
        flashlight.SetActive(false);
        offhandLight.SetActive(false);
        axe.SetActive(false);
        drill.SetActive(false);
        lighter.SetActive(false);
        fuelCanister.SetActive(false);
    }

    private void ToggleLighterLight()
    {
        if (inVent || inRedRoom) return; // Don't toggle lighter in vent or red room

        // Play lighter toggle sound 
        ItemAudioSource.PlayOneShot(flashlightSound, 1.5f); // Reuse flashlight sound for now

        isLighterOn = !isLighterOn;
        lighterLight.enabled = isLighterOn; // Toggle the lighter light
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
        SoundManager.MakeSound(transform.position, 40f); //play sound when using drill

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
        SoundManager.MakeSound(transform.position, 20f); //play sound when swinging axe      
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
        if (other.CompareTag("BellChime"))
        {
            if (!secondFloor && facultyNoteObj.activeSelf == false){//if on first floor
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
                () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Investigate the school for any tools or clues.")
                )
            );
            }
            if (secondFloor && artNoteObj.activeSelf == false){ //if on second floor
                Debug.Log("Bell sound played");
                MiscEventAudioSource.clip = BellChime;
                MiscEventAudioSource.PlayOneShot(BellChime);
                BellChimeObj.SetActive(false); //turn off bell chime object
                //remove all barriers
                foreach (GameObject barrier in barriers)
                {
                    barrier.SetActive(false);
                }
                FindAnyObjectByType<ThoughtDialogueManager>().ShowThought("  That can't be good... If whatever this note is about is true...",
                    () => FindFirstObjectByType<ThoughtDialogueManager>().ShowThought("I'm not alone up here.",
                    () => FindFirstObjectByType<ObjectiveManager>().SetObjective("□ Find the cloth, gasoline, and lighter to escape.")
                    )
                );
                MainMonster.SetActive(true); //spawn main monster
            }
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
        if (other.CompareTag("Locker")){//hiding in locker so mame monster not chase player
            AIScript monsterAI = FindFirstObjectByType<AIScript>();
            if (monsterAI != null)
            {
                Debug.Log("Player is hiding in locker");
                monsterAI.isPlayerHiding = true; // Set the monster's isPlayerHiding variable to true
            }
        }
        if (other.CompareTag("Stairs")){//change levels
            SceneManager.LoadScene("2ndFloor");
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
        if (other.CompareTag("Locker")) //stop hiding in locker
        {
            AIScript monsterAI = FindFirstObjectByType<AIScript>();
            if (monsterAI != null)
            {
                Debug.Log("Player is no longer hiding in locker");
                monsterAI.isPlayerHiding = false; // Set the monster's isPlayerHiding variable to false
            }
        }
    }

    public bool getObtained(string item){
        if(item == "axe"){
            return axeObtained;
        }
        if(item == "drill"){
            return drillObtained;
        }
        if(item == "lighter"){
            return lighterObtained;
        }
        if(item == "fuelcan"){
            return fuelcanObtained;
        }
        if(item == "cloth"){
            return clothObtained;
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
        if (item == "lighter"){
            return isLighterEquipped;
        }
        if (item == "fuelcan"){
            return isFuelCanisterEquipped;
        }
        if (item == "cloth"){
            return isClothEquipped;
        }

        return false;
    }
}


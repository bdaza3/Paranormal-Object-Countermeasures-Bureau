using System.Numerics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    private float health;
    private float currTime;
    private float startTime;
    public float interval = 1f;
    private bool inDistance;
    public float damage = 1f;
    private float damageMultiplier;
    public float opacity = 0.5f;
    public float bigM;
    [SerializeField] private Graphic redUI;
    [SerializeField]public Graphic gameOver;

    public Color MyColor;
    public Color gameOverColor;
    public float healingFactor = 3f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        health = 100f;
        startTime = 0f;
        MyColor = Color.red;
        MyColor.a = 0;
        gameOverColor = Color.white;
        gameOverColor.a = 0;
        redUI.color = MyColor;
        gameOver.color = gameOverColor;
    }

    // Update is called once per frame
    void Update()
    {
        currTime += Time.deltaTime;
        if(inDistance && currTime - startTime >= interval){
            health -= damage * damageMultiplier;
            Debug.Log("Health: " + health);
            startTime = currTime;
            MyColor.a = (1 - (health/100)) * opacity;
            redUI.color =  MyColor;
        } 
        else if(!inDistance && health < 100){
            if(currTime - startTime >= interval){
                health += healingFactor;
                startTime = currTime;
                MyColor.a = (1 - (health / 100)) * opacity;
                redUI.color = MyColor;
            }
        }
        if(health <= 0)
        {
            gameOverColor.a = 1; 
            gameOver.color = gameOverColor;

            StartCoroutine(LoadMenuAfterDelay(4f)); // Start the coroutine to load the menu after 2 seconds
        }
    }

    // Coroutine to handle the delay and scene loading
    private IEnumerator LoadMenuAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        SceneManager.LoadScene("MenuScene"); // Replace "MenuScene" with the name of your menu scene
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("BigMonster")){
            inDistance = true;
            damageMultiplier = bigM;
        }
    }

    void OnTriggerExit(Collider other)
    {
        Debug.Log("monster not in range");
        inDistance = false;
    }
}

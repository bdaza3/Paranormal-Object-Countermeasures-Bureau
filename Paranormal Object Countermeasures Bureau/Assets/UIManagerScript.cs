using UnityEngine;
using UnityEngine.UI;

//change inventory UI
public class UIManagerScript : MonoBehaviour
{
    public Graphic drill;
    public Graphic flashlight;
    public Graphic axe;
    public Graphic lighter;
    public Graphic fuelCan;
    public Graphic cloth;
    public GameObject player;

    private PlayerInventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flashlight.enabled = true;
        axe.enabled = false;
        drill.enabled = false;
        lighter.enabled = false;
        fuelCan.enabled = false;
        cloth.enabled = false;

        inventory = player.GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        inventory = player.GetComponent<PlayerInventory>();
        if(inventory.getObtained("axe")){
            axe.enabled = true;
        }
        if(inventory.getObtained("drill")){
            drill.enabled = true;
        }
        if(inventory.getObtained("lighter")){
            lighter.enabled = true;
        }
        if(inventory.getObtained("fuelcan")){
            fuelCan.enabled = true;
        }
        if(inventory.getObtained("cloth")){
            cloth.enabled = true;
        }
    }
}

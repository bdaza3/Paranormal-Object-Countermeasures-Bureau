using UnityEngine;
using UnityEngine.UI;

//change inventory UI
public class UIManagerScript : MonoBehaviour
{
    public Graphic drill;
    public Graphic flashlight;
    public Graphic axe;
    public GameObject player;

    private PlayerInventory inventory;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flashlight.enabled = true;
        axe.enabled = false;
        drill.enabled = false;

        inventory = player.GetComponent<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if(inventory.getObtained("axe")){
            axe.enabled = true;
        }
        if(inventory.getObtained("drill")){
            drill.enabled = true;
        }
    }
}

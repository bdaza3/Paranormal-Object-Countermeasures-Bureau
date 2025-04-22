using UnityEngine;

public class InRoomScript : MonoBehaviour
{
    private bool inRoom;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        inRoom = false;
    }

    // Update is called once per frame
    void OnTriggerEnter(Collider other)
    {
        Debug.Log(other + "detected");
        if (other.CompareTag("Player"))
        {
            inRoom = true;
            Debug.Log(other + "in room");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRoom = false;
            Debug.Log(other + "left room");
        }
    }

    public bool IsInRoom(){
        return inRoom;
    }
}

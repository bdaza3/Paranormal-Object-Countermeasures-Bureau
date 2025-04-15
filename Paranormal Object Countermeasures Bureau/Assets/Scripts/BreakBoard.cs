using UnityEngine;

public class BreakBoard : MonoBehaviour
{
    private bool playerInTrigger = false;
    private Rigidbody rb;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }

    public bool IsPlayerInside()
    {
        return playerInTrigger;
    }

    public void SetKinematic(){
        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;
    }
}


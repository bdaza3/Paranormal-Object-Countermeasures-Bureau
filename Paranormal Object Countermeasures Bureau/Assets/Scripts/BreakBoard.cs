using UnityEngine;

public class BreakBoard : MonoBehaviour
{
    private bool playerInTrigger = false;
    private Rigidbody rb;
    public GameObject barrier;
    private Collider myCollider;
    private Collider barrierCollider;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        myCollider = GetComponent<Collider>();
        barrierCollider = barrier.GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Collision Detected");
        if (other.CompareTag("AxeItem"))
        {
            Debug.Log("Axe Collided with Board");
            playerInTrigger = true;
            rb.isKinematic = false;
            myCollider.isTrigger = false;
            barrierCollider.enabled = false;
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

    // public void SetKinematic(){
    //     rb = GetComponent<Rigidbody>();
    //     rb.isKinematic = false;
    //     barrier.SetActive(false);
    // }
}


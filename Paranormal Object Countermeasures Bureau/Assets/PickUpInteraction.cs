using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour
{
    public GameObject player;
    public Transform holdPos; // The position where the object is held
    public float throwForce = 750f; // Force applied when throwing the object
    public float pickUpRange = 5f; // Max distance to pick up an object
    private GameObject heldObj; // The object being held
    private Rigidbody heldObjRb; // Rigidbody of the held object

    void Update()
    {
        // Pickup object
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (heldObj == null)
            {
                TryPickUp();
            }
            else
            {
                DropObject();
            }
        }

        // Throw object
        if (Input.GetKeyDown(KeyCode.T) && heldObj != null)
        {
            ThrowObject();
        }

        // Keep held object at hold position
        if (heldObj != null)
        {
            heldObj.transform.position = holdPos.position;
        }
    }

    void TryPickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, pickUpRange))
        {
            if (hit.collider.gameObject.CompareTag("canPickUp"))
            {
                heldObj = hit.collider.gameObject;
                heldObjRb = heldObj.GetComponent<Rigidbody>();

                if (heldObjRb != null)
                {
                    heldObjRb.useGravity = false;
                    heldObjRb.isKinematic = true;
                    heldObj.transform.position = holdPos.position;
                    heldObj.transform.parent = holdPos;
                }
            }
        }
    }

    void DropObject()
    {
        if (heldObj != null)
        {
            heldObjRb.useGravity = true;
            heldObjRb.isKinematic = false;
            heldObj.transform.parent = null;
            heldObj = null;
        }
    }

    void ThrowObject()
    {
        if (heldObj != null)
        {
            heldObjRb.useGravity = true;
            heldObjRb.isKinematic = false;
            heldObj.transform.parent = null;
            heldObjRb.AddForce(Camera.main.transform.forward * throwForce);
            heldObj = null;
        }
    }
}

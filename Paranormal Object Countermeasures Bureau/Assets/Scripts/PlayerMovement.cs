using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; //default movement speed
    public float runSpeed = 9f;  //running speed
    public float lookSpeedX = 3f; 
    public float lookSpeedY = 3f; 

    private Camera playerCamera;
    private float rotationX = 0f; 

    private void Start()
    {
        playerCamera = Camera.main; 
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false; 
    }

    private void Update()
    {
        MoveCharacter(); 
        LookAround();
    }

    private void MoveCharacter()
    {
        float horizontal = Input.GetAxis("Horizontal"); 
        float vertical = Input.GetAxis("Vertical"); 

        float currentMoveSpeed = moveSpeed; //default speed
        if (Input.GetKey(KeyCode.LeftShift)) //if shift is pressed
        {
            currentMoveSpeed = runSpeed; //run speed
            SoundManager.MakeSound(transform.position, 15f); //running sound trigger
        }
        else
        {
            SoundManager.MakeSound(transform.position, 8f); //walking sound trigger
        }
        Vector3 moveDirection = new Vector3(horizontal, 0f, vertical).normalized;

        if (moveDirection.magnitude >= 0.1f)
        {
            float moveSpeedAdjusted = currentMoveSpeed * Time.deltaTime;
            transform.Translate(moveDirection * moveSpeedAdjusted); 
        }
    }

    private void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * lookSpeedX; 
        float mouseY = Input.GetAxis("Mouse Y") * lookSpeedY; 
        transform.Rotate(Vector3.up * mouseX);

        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f); 
        playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
    }
}
using UnityEngine;

public class AIclassroom : MonoBehaviour
{
    private Animator anim;
    public Transform player; // Reference to the player's transform
    public float chaseRange = 11f; // Range within which the zombie starts chasing
    public float stopRange = 4f; // Range within which the zombie stops and attacks
    private bool isAttacking = false; // To prevent multiple attack triggers

    // Reference to the player's flashlight
    public Light flashlightLight; // Assign this in the Unity Editor

    void Start()
    {
        anim = GetComponent<Animator>();
        Debug.Log("AIclassroom script started.");
    }

    void Update()
    {
        // Check if the flashlight is on
        if (flashlightLight != null && flashlightLight.enabled)
        {
            // If the flashlight is on, force the zombie to stay idle
            SetAnimationState(true, false, false); // Idle state
            return; // Exit the Update method to prevent further checks
        }

        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= stopRange && !isAttacking)
        {
            // Player is within attack range
            isAttacking = true;
            SetAnimationState(false, false, true); // Attack state
            Invoke(nameof(ResetAttack), 1f); // Reset attack state after 1 second (adjust as needed)
        }
        else if (distanceToPlayer > stopRange && distanceToPlayer <= chaseRange)
        {
            // Player is within chase range but not attack range
            if (!isAttacking)
            {
                SetAnimationState(false, true, false); // Walk state
                ChasePlayer();
            }
        }
        else
        {
            // Player is out of range
            if (!isAttacking)
            {
                SetAnimationState(true, false, false); // Idle state
            }
        }
    }

    void ChasePlayer()
    {
        // Calculate direction to the player, ignoring the Y-axis
        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // Ensure no movement on the Y-axis

        // Rotate towards the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Move towards the player, ignoring the Y-axis
        Vector3 targetPosition = new Vector3(player.position.x, transform.position.y, player.position.z);
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2f); // Adjust speed as needed
    }

    void ResetAttack()
    {
        isAttacking = false;
    }

    void SetAnimationState(bool isIdle, bool isWalking, bool isAttacking)
    {
        anim.SetBool("Idle", isIdle);
        anim.SetBool("Walk", isWalking);
        anim.SetBool("Attack", isAttacking);
    }
}

using UnityEngine;

public class AIclassroom : MonoBehaviour
{
    private Animator anim;
    public Transform player; // Reference to the player's transform
    public float chaseRange = 11f; // Range within which the zombie starts chasing
    public float stopRange = 4f; // Range within which the zombie stops and attacks
    private bool isAttacking = false; // To prevent multiple attack triggers

    void Start()
    {
        anim = GetComponent<Animator>();
        Debug.Log("AIclassroom script started.");
    }

    void Update()
    {
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
        // Rotate towards the player
        Vector3 direction = (player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);

        // Move towards the player
        transform.position = Vector3.MoveTowards(transform.position, player.position, Time.deltaTime * 2f); // Adjust speed as needed
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

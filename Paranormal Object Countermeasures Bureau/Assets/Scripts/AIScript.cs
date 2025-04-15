using UnityEngine;
using UnityEngine.AI;

public class AIScript : MonoBehaviour
{

    public GameObject monster; //reference to the monster which has the animator component
    public Animator animator; //reference to animator component in monster fbx

    public enum State { Patrol, Investigate, Chase } //list of states
    public State currentState = State.Patrol; //start of patrolling

    public Transform[] patrolPoints;
    public float investigateTime = 3f;
    public float chaseRange = 15f; //once the player is within this range, the monster will chase

    private int patrolIndex = 0;
    public float investigateTimer = 0f;
    public NavMeshAgent agent;
    public Vector3 lastHeardSound;
    private Transform player;

    void Start()
    {
        animator = monster.GetComponent<Animator>(); //get the animator component from the monster
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        //Debug.Log("Distance to player: " + distanceToPlayer);

        switch (currentState)
        {
            case State.Patrol: //patrol state
                animator.SetTrigger("Walk"); //set the walk animation
                //Debug.Log("Patrolling..");
                agent.speed = 3f; //normal speed
                PatrolBehavior();
                if (distanceToPlayer < chaseRange)
                    currentState = State.Chase;
                break;

            case State.Investigate: //monster hears a sound
                animator.SetTrigger("Walk"); //set the walk animation
                //Debug.Log("Investigating..");
                agent.speed = 5f; //speed walk to investigate
                InvestigateBehavior();
                if (distanceToPlayer < chaseRange)
                    currentState = State.Chase;
                break;

            case State.Chase: //chase player
                animator.SetTrigger("Chase"); //set the run animation
                //Debug.Log("Chasing..");
                agent.speed = 7f; //run speed
                ChaseBehavior();
                if (distanceToPlayer > chaseRange * 3f) //if the player is too far away
                    currentState = State.Patrol;
                break;
        }
    }

    public void HearSound(Vector3 soundPos) //investigate state to go to sound (not during chase)
    {
        lastHeardSound = soundPos;
        investigateTimer = investigateTime;
        currentState = State.Investigate;
        agent.SetDestination(lastHeardSound);
    }

    void PatrolBehavior()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.5f)
        {
            GoToNextPatrolPoint();
        }
    }

    void InvestigateBehavior()
    {
        investigateTimer -= Time.deltaTime; //investigate area for a few seconds

        if (investigateTimer <= 0f)
        {
            currentState = State.Patrol;
            GoToNextPatrolPoint();
        }
    }

    void ChaseBehavior()//chase player
    {
        agent.SetDestination(player.position); //set the destination to the player's position
        if (Vector3.Distance(transform.position, player.position) < 2f) //if the monster is close to the player
        {
            //animator.SetTrigger("Attack"); //attack animation
            //Debug.Log("Attacking player..");
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }
}


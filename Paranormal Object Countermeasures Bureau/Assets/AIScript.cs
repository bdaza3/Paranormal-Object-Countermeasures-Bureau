using UnityEngine;
using UnityEngine.AI;

public class AIScript : MonoBehaviour
{
    public enum State { Patrol, Investigate, Chase } //list of states
    public State currentState = State.Patrol; //start of patrolling

    public Transform[] patrolPoints;
    public float investigateTime = 3f;
    public float chaseRange = 8f;
    public float playerDetectionRange = 5f; //if player gets too close

    private int patrolIndex = 0;
    private float investigateTimer = 0f;
    private NavMeshAgent agent;
    private Vector3 lastHeardSound;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        GoToNextPatrolPoint();
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        switch (currentState)
        {
            case State.Patrol: //patrol state
                agent.speed = 3f; //normal speed
                PatrolBehavior();
                if (distanceToPlayer < chaseRange)
                    currentState = State.Chase;
                break;

            case State.Investigate: //monster hears a sound
                agent.speed = 4.5f; //speed walk to investigate
                InvestigateBehavior();
                if (distanceToPlayer < chaseRange)
                    currentState = State.Chase;
                break;

            case State.Chase: //chase player
                agent.speed = 6.2f; //run speed
                ChaseBehavior();
                if (distanceToPlayer > chaseRange * 1.5f)
                    currentState = State.Patrol;
                break;
        }
    }

    public void HearSound(Vector3 soundPos)
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
        investigateTimer -= Time.deltaTime;

        if (investigateTimer <= 0f)
        {
            currentState = State.Patrol;
            GoToNextPatrolPoint();
        }
    }

    void ChaseBehavior()
    {
        agent.SetDestination(player.position);
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }
}


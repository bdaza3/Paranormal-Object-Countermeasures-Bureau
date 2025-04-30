using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class AIScript : MonoBehaviour
{
    public GameObject monster; //reference to the monster which has the animator component
    public Animator animator; //reference to animator component in monster fbx

    public bool isPlayerHiding = false; //check if the player is hiding in a locker

    //SOUNDS

    bool isBgmPlaying = false; //check if bgm is playing
    public AudioSource MonsterWalkAudioSource; //reference to the audio source for chase bgm

    public AudioSource MonsterMiscAudioSource;

    public AudioSource AmbientAudioSource; //reference to the audio source for ambient sounds

    private float footstepTimer = 0f;
    public float footstepInterval = 1.5f; //how fast footsteps happen

    [SerializeField] private AudioClip footstepSound; 

    [SerializeField] private AudioClip stompSound; 

    [SerializeField] private AudioClip attackSound; 

    [SerializeField] private AudioClip spawnSound; 

    [SerializeField] private AudioClip chaseBGM; 

    public enum State { Patrol, Investigate, Chase, Death } //list of states
    public State currentState = State.Patrol; //start of patrolling

    public Transform[] patrolPoints;
    public float investigateTime = 7f; //how long the monster investigates the area
    public float chaseRange = 20f; //once the player is within this range, the monster will chase

    private int patrolIndex = 0;
    public float investigateTimer = 0f;
    public NavMeshAgent agent;
    public Vector3 lastHeardSound;
    private Transform player;
    public GameObject playerObject; //reference to the player object

    public bool inLabKillable = false;

    public bool death = false; //check if the monster is dead

    IEnumerator PlayFirstHalf()
    {
        MonsterWalkAudioSource.time = 0f; // start at beginning
        MonsterWalkAudioSource.Play();
        yield return new WaitForSeconds(footstepSound.length / 2.5f); // wait half the clip
        MonsterWalkAudioSource.Stop(); // stop playback
    }

    IEnumerator PlayFirstPart()
    {
        MonsterWalkAudioSource.time = 0f; // start at beginning
        MonsterWalkAudioSource.Play();
        yield return new WaitForSeconds(stompSound.length / 2f); // wait half the clip
        MonsterWalkAudioSource.Stop(); // stop playback
    }

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

                MonsterWalkAudioSource.clip = footstepSound;
                footstepTimer += Time.deltaTime;
                if (footstepTimer >= footstepInterval)
                {
                    MonsterWalkAudioSource.pitch = Random.Range(0.8f, 1.3f);
                    StartCoroutine(PlayFirstHalf());
                    footstepTimer = 0f;
                }

                if (distanceToPlayer < chaseRange && !isPlayerHiding)
                {
                    currentState = State.Chase;
                    footstepTimer = 0f;
                }
                if (death) //if the monster is dead
                {
                    currentState = State.Death; //go to death state
                    footstepTimer = 0f;
                }
                break;

            case State.Investigate: //monster hears a sound
                animator.SetTrigger("Walk"); //set the walk animation
                //Debug.Log("Investigating..");
                agent.speed = 6f; //speed walk to investigate
                InvestigateBehavior();

                MonsterWalkAudioSource.clip = footstepSound;
                footstepTimer += Time.deltaTime;
                if (footstepTimer >= footstepInterval)
                {
                    MonsterWalkAudioSource.pitch = Random.Range(0.8f, 1.3f);
                    StartCoroutine(PlayFirstHalf());
                    footstepTimer = 0f;
                }
                    PlayerInventory playerInventory = playerObject.GetComponent<PlayerInventory>();
                    if (distanceToPlayer < chaseRange && !isPlayerHiding || !playerInventory.inVent)
                    {
                        currentState = State.Chase;
                        footstepTimer = 0f;
                    }
                    if (death) //if the monster is dead
                    {
                        currentState = State.Death; //go to death state
                        footstepTimer = 0f;
                    }
                    break;

            case State.Chase: //chase player
                animator.SetTrigger("Chase"); //set the run animation
                //Debug.Log("Chasing..");
                agent.speed = 6f; //run speed
                if (!isPlayerHiding)
                    ChaseBehavior();

                if (!isBgmPlaying)
                {
                    AmbientAudioSource.Stop();
                    AmbientAudioSource.clip = chaseBGM;
                    AmbientAudioSource.volume = 1.2f;
                    AmbientAudioSource.Play();
                    isBgmPlaying = true;
                }

                MonsterWalkAudioSource.clip = stompSound;
                footstepTimer += Time.deltaTime;
                if (footstepTimer >= footstepInterval)
                {
                    MonsterWalkAudioSource.pitch = Random.Range(0.8f, 1.3f);
                    StartCoroutine(PlayFirstPart());
                    footstepTimer = 0f;
                }
                playerInventory = playerObject.GetComponent<PlayerInventory>();
                if (distanceToPlayer > chaseRange * 2f || isPlayerHiding || playerInventory.inVent){ //if the player is too far away or hiding
                    AmbientAudioSource.Stop();
                    isBgmPlaying = false;
                    currentState = State.Patrol;
                    footstepTimer = 0f;
                }
                if (death) //if the monster is dead
                {
                    currentState = State.Death; //go to death state
                    footstepTimer = 0f;
                }
                break;
            case State.Death:
                animator.SetTrigger("Death"); //set the death animation
                Debug.Log("Monster is dead..");
                MonsterMiscAudioSource.clip = spawnSound;
                MonsterMiscAudioSource.pitch = 1f;
                MonsterMiscAudioSource.volume = 4f;
                MonsterMiscAudioSource.PlayOneShot(spawnSound);
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
        if (Vector3.Distance(transform.position, player.position) < 4f) //if the monster is close to the player
        {
            animator.SetTrigger("Attack"); //attack animation
            Debug.Log("Attacking player..");
            MonsterMiscAudioSource.clip = attackSound;
            MonsterMiscAudioSource.pitch = Random.Range(0.8f, 1.3f);
            MonsterMiscAudioSource.volume = 0.3f;
            MonsterMiscAudioSource.PlayOneShot(attackSound);
        }
    }

    void GoToNextPatrolPoint()
    {
        if (patrolPoints.Length == 0) return;

        agent.SetDestination(patrolPoints[patrolIndex].position);
        patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lab"))//if object enters the lab
        {
            inLabKillable = true; //set the monster to be killable
            Debug.Log("Monster is killable in lab");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Lab"))//if object exits the lab
        {
            inLabKillable = false; //set the monster to not be killable
            Debug.Log("Monster is not killable in lab");
        }
    }
}


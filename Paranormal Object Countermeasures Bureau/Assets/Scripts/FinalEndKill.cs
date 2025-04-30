using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class FinalEndKill : MonoBehaviour
{
    public PlayerInventory playerInventory; //reference to the player inventory script

    public AIScript aiScript; //reference to the AI scriptS

    public GameObject monster; //reference to the monster which has the animator component

    public GameObject[] fireParticles;

    private bool burning = false; //check if the monster is burning
    void Start()
    {
        playerInventory = FindFirstObjectByType<PlayerInventory>();
    }

    // Update is called once per frame
    void Update()
    {
        if (monster != null){
            aiScript = FindFirstObjectByType<AIScript>();
        }
        
        if (playerInventory.playerInLab && playerInventory.all3ItemsObtained && aiScript.inLabKillable){
            FindFirstObjectByType<ThoughtDialogueManager>().ShowHoverText("(Press F to destroy Object 676)");
            if (Input.GetKeyDown(KeyCode.F))
            {
                //show fire
                burning = true;
                aiScript.death = true; //set monster to dead
                //stop monster from moving and stop its chase bgm
                aiScript.agent.isStopped = true;
                aiScript.MonsterWalkAudioSource.Stop();
                aiScript.AmbientAudioSource.Stop();
                foreach (GameObject fireParticle in fireParticles)
                {
                    fireParticle.SetActive(true);
                }

                //Destroy(monster);
                Debug.Log("Object 676 destroyed");
                //wait for 7 seconds before switching scenes
                StartCoroutine(WaitAndSwitchScene(7f));

            }
        }
        if (!burning){//do not show fire if not killed
            foreach (GameObject fireParticle in fireParticles)
            {
                fireParticle.SetActive(false);
            }
        }
    }
    IEnumerator WaitAndSwitchScene(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene("EndCutscene");
    }
}

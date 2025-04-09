using System;
using UnityEngine;

public static class SoundManager
{
    public static void MakeSound(Vector3 soundOrigin, float soundRange)
    {
        //Debug.Log("Sound MADE by player");

        //get all colliders within the sound range
        Collider[] colliders = Physics.OverlapSphere(soundOrigin, soundRange);

        foreach (Collider col in colliders)
        {
            if (col.CompareTag("BigMonster"))
            {
                AIScript ai = col.GetComponent<AIScript>();
                if (ai != null)
                {
                    ai.HearSound(soundOrigin);  //call the function on the monster
                    Debug.Log("Sound heard by monster");
                }
            }
        }
    }
}

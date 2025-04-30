using UnityEngine;
using System.Collections;

public class BGMMenu : MonoBehaviour
{
    public AudioClip bgmClip;
    public float fadeDuration = 2f;
    public float waitBetweenLoops = 0.2f;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = bgmClip;
        audioSource.volume = 0f;
        audioSource.loop = false;

        StartCoroutine(LoopFade());
    }

    IEnumerator LoopFade()
    {
        while (true)
        {
            audioSource.time = 0f;
            audioSource.Play();

            // Fade In
            float tIn = 0f;
            while (tIn < fadeDuration)
            {
                tIn += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(0f, 1f, tIn / fadeDuration);
                yield return null;
            }

            // Wait until it's time to fade out
            float playTime = bgmClip.length - fadeDuration;
            yield return new WaitForSeconds(playTime);

            // Fade Out
            float tOut = 0f;
            while (tOut < fadeDuration)
            {
                tOut += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(1f, 0f, tOut / fadeDuration);
                yield return null;
            }

            audioSource.Stop();

            // Optional short silence before looping again
            yield return new WaitForSeconds(waitBetweenLoops);
        }
    }
}

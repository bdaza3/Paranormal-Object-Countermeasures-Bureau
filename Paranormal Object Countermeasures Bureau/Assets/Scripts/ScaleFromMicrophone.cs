using UnityEngine;
using UnityEngine.UI;

public class ScaleFromMicrophone : MonoBehaviour
{
    public AudioSource source;
    public Vector3 minScale;
    public Vector3 maxScale;
    public AudioLoudnessDetection detector;

    public float loudnessSensitivity = 1;
    public float threshold = 0.1f;

    public float soundRange;
    public float soundCooldown = 1f;
    private float soundCooldownTimer = 0f;

    public Slider loudnessSlider;

    public Slider loudnessSlider2; 

    public Image fillImage; 

    public Image fillImage2; 

    public Color lowColor = Color.green;
    public Color mediumColor = Color.yellow;
    public Color highColor = Color.red;

    public Transform playerPosition;

    void Update()
    {
        float loudness = detector.GetLoudnessFromMicrophone() * loudnessSensitivity;
        loudness = Mathf.Clamp01(loudness); //normalized between 0 and 1

        transform.localScale = Vector3.Lerp(minScale, maxScale, loudness);

        soundRange = Mathf.Lerp(0, 40, loudness); //adjust sound range based on loudness

        if (loudnessSlider != null)
        {
            loudnessSlider.value = loudness;
        }

        if (fillImage != null)
        {
            if (soundRange > 25f)
                fillImage.color = highColor;//red
            else if (soundRange > 15f)
                fillImage.color = mediumColor;//yellow
            else
                fillImage.color = lowColor;//green
        }

        if (loudnessSlider2 != null)
        {
            loudnessSlider2.value = loudness;
        }

        if (fillImage2 != null)
        {
            if (soundRange > 25f)
                fillImage2.color = highColor;//red
            else if (soundRange > 15f)
                fillImage2.color = mediumColor;//yellow
            else
                fillImage2.color = lowColor;//green
        }
        
        if (loudness > threshold && soundCooldownTimer <= 0f)
        {
            Debug.Log("Loud azz Sound ("+ soundRange + ") made at position: " + playerPosition.position);
            SoundManager.MakeSound(playerPosition.position, soundRange * 1.2f);
            soundCooldownTimer = soundCooldown;
        }

        if (soundCooldownTimer > 0f)
        {
            soundCooldownTimer -= Time.deltaTime;
        }
    }
}


using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private Vector3 initialLocalPos;
    private float shakeAmount = 0f;
    private bool isShaking = false;

    [Range(1f, 50f)] public float shakeSpeed = 15f;

    void Start()
    {
        initialLocalPos = transform.localPosition;
    }

    void Update()
    {
        if (isShaking)
        {
            Vector3 shakeOffset = Random.insideUnitSphere * shakeAmount;
            shakeOffset.z = 0f;
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos + shakeOffset, Time.deltaTime * shakeSpeed);
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, initialLocalPos, Time.deltaTime * shakeSpeed);
        }
    }

    public void StartShake(float amount)
    {
        shakeAmount = amount;
        isShaking = true;
    }

    public void StopShake()
    {
        isShaking = false;
    }
}



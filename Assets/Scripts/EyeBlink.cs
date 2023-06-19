using System.Collections;
using UnityEngine;

public class EyeBlink : MonoBehaviour
{
    public float blinkDuration = 0.1f;  // Time it takes to close the eyes
    public float timeBetweenBlinks = 4f;  // Time between blinks
    private Vector3 initialScale;

    void Start()
    {
        initialScale = transform.localScale;
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(timeBetweenBlinks);
            StartCoroutine(Blink());
        }
    }

    IEnumerator Blink()
    {
        float time = 0;

        while (time < blinkDuration)
        {
            // Scale down
            time += Time.deltaTime;
            float t = time / blinkDuration;

            transform.localScale = Vector3.Lerp(initialScale, new Vector3(initialScale.x, 0, initialScale.z), t);
            yield return null;
        }

        time = 0;

        while (time < blinkDuration)
        {
            // Scale up
            time += Time.deltaTime;
            float t = time / blinkDuration;
            transform.localScale = Vector3.Lerp(new Vector3(initialScale.x, 0, initialScale.z), initialScale, t);
            yield return null;
        }

        // Ensure eyes are fully open
        transform.localScale = initialScale;
    }
}

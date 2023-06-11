using Sirenix.OdinInspector;
using UnityEngine;

public class FlyingBob : MonoBehaviour
{
    public Vector3 minPosition;  // The lower point where the object will bob to relative to its initial position
    public Vector3 maxPosition;  // The upper point where the object will bob to relative to its initial position
    public float bobSpeedMin = 1f;  // The minimum speed at which the object can bob
    public float bobSpeedMax = 2f;  // The maximum speed at which the object can bob

    private Vector3 initialLocalPosition;  // The initial local position of the object
    private float currentBobSpeed;  // The current speed at which the object is bobbing
    private float time;  // The elapsed time used to calculate the lerp function for bobbing

    public void Start()
    {
        initialLocalPosition = transform.localPosition;

        // Randomize the bob speed
        currentBobSpeed = Random.Range(bobSpeedMin, bobSpeedMax);

        // Randomize the initial time so the objects don't all bob in sync
        time = Random.Range(0f, 1f);
    }

    private void Update()
    {
        // Update the elapsed time
        time += Time.deltaTime * currentBobSpeed;

        // Ensure time stays within the required range
        if (time > 1f)
        {
            // Swap the min and max positions to make the object bob in the other direction
            var temp = minPosition;
            minPosition = maxPosition;
            maxPosition = temp;

            // Reset the time
            time = 0f;
        }

        // Calculate and apply the new local position for the bobbing effect
        ApplyBobbing();
    }

    private void ApplyBobbing()
    {
        // Lerp between the min and max positions based on the current time
        Vector3 newPosition = Vector3.Lerp(minPosition, maxPosition, time);

        // Update the object's local position
        transform.localPosition = initialLocalPosition + newPosition;
    }

    [Button]
    public void SetSpeeds()
    {
        currentBobSpeed = Random.Range(bobSpeedMin, bobSpeedMax);
    }

    private void OnDrawGizmosSelected()
    {
        // Draw a line showing the path of the bobbing effect
        Gizmos.color = Color.yellow;
        Vector3 minPositionWorld = transform.parent.TransformPoint(initialLocalPosition + minPosition);
        Vector3 maxPositionWorld = transform.parent.TransformPoint(initialLocalPosition + maxPosition);
        Gizmos.DrawLine(minPositionWorld, maxPositionWorld);
    }
}

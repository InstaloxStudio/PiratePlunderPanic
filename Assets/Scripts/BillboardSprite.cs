using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public Camera cam;
    public bool lockXZ = false;

    private void Start()
    {

        if (cam == null)
        {
            cam = Camera.main;
        }

        if (cam == null)
        {
            Debug.LogError("No camera available");
        }
    }

    private void Update()
    {
        //make the sprite face the camera
        if (lockXZ)
        {
            transform.rotation = Quaternion.Euler(0f, cam.transform.rotation.eulerAngles.y, 0f);

        }
        else
        {
            transform.rotation = cam.transform.rotation;
        }
    }
}
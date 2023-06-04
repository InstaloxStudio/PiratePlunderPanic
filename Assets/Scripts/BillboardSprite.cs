using UnityEngine;

public class BillboardSprite : MonoBehaviour
{
    public Camera cam;
    public SpriteRenderer Sprite;
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
        if (Sprite == null)
            Sprite = GetComponent<SpriteRenderer>();

        if (Sprite == null)
        {
            Sprite = GetComponentInChildren<SpriteRenderer>();
        }

        if (Sprite == null)
        {
            Debug.LogError("No sprite renderer available");
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
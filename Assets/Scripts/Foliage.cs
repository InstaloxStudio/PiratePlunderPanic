using UnityEngine;

public class Foliage : MonoBehaviour
{
    public Texture2D _mainTexture;

    private void Awake()
    {
        //get material and set its texture to the main texture in the child object
        Material material = GetComponentInChildren<MeshRenderer>().material;
        material.mainTexture = _mainTexture;

    }
}

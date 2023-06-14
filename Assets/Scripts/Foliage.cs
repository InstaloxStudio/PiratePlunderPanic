using Sirenix.OdinInspector;
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


    [Button]
    public void CreateFoliage()
    {
        //add a child gameobject with a quad mesh and a mesh renderer
        GameObject foliage = new GameObject("Foliage");
        foliage.transform.SetParent(transform);
        foliage.transform.localPosition = Vector3.zero;
        foliage.transform.localRotation = Quaternion.identity;
        foliage.transform.localScale = Vector3.one;

        MeshFilter meshFilter = foliage.AddComponent<MeshFilter>();
        MeshRenderer meshRenderer = foliage.AddComponent<MeshRenderer>();
        
        var billboardcomponent = foliage.AddComponent<BillboardSprite>();
        billboardcomponent.lockXZ = true;
    }
}

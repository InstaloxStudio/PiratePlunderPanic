using Sirenix.OdinInspector;
using UnityEngine;

public class TextureComponent : MonoBehaviour
{
    public Texture2D _mainTexture;

    public Color _tint = Color.white;

    [Button]
    public void SetTexture()
    {
        var renderer = GetComponent<Renderer>();
        renderer.sharedMaterial.SetTexture("_MainTex", _mainTexture);
        renderer.sharedMaterial.SetColor("_Color", _tint);
    }
}
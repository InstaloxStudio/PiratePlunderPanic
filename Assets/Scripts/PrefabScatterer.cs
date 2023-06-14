using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class PrefabScatterer : MonoBehaviour
{
    public List<ScatterPrefabLayers> Layers = new List<ScatterPrefabLayers>();

    private void OnEnable()
    {
        foreach (var layer in Layers)
        {
            layer._transform = transform;
        }
    }

    private Vector3 RandomPointInArea(Vector2 area)
    {
        float halfWidth = area.x / 2;
        float halfHeight = area.y / 2;
        float x = Random.Range(-halfWidth, halfWidth);
        float z = Random.Range(-halfHeight, halfHeight);

        return transform.position + new Vector3(x, 0, z);
    }

    private void OnDrawGizmosSelected()
    {
        foreach (var layer in Layers)
        {
            layer.DrawGizmos();

        }
    }

}

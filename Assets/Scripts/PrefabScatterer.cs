using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;


public class PrefabScatterer : MonoBehaviour
{
    public List<ScatterPrefabProperties> scatterProps;
    public bool autoClear = true;
    public float seed = 0.5f;

    [SerializeField]
    private List<GameObject> scatteredObjects = new List<GameObject>();

    [Button]
    public void Scatter()
    {
        if (autoClear) Clear();

        foreach (var props in scatterProps)
        {
            for (int i = 0; i < props.amountToScatter; i++)
            {
                Vector3 pointInArea = RandomPointInArea(props.scatterArea);
                float noiseValue = 0f;
                if (props.useNoise)
                {
                    noiseValue = Mathf.PerlinNoise(seed + pointInArea.x / props.noiseScale, seed + pointInArea.z / props.noiseScale);
                }

                if (!props.useNoise || noiseValue > props.density)
                {
                    GameObject randomPrefab = props.prefabs[Random.Range(0, props.prefabs.Count)];
                    GameObject instance = Instantiate(randomPrefab, pointInArea, Quaternion.identity);
                    instance.transform.SetParent(transform);

                    float randomScale = Random.Range(props.minScale, props.maxScale);
                    instance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

                    Renderer renderer = instance.GetComponentInChildren<Renderer>();
                    if (renderer != null)
                    {
                        Color randomColor = new Color(
                            Random.Range(props.minColor.r, props.maxColor.r),
                            Random.Range(props.minColor.g, props.maxColor.g),
                            Random.Range(props.minColor.b, props.maxColor.b)
                        );

                        renderer.sharedMaterial.color = randomColor;
                    }

                    scatteredObjects.Add(instance);
                }
            }
        }
    }

    [Button]
    public void Clear()
    {
        while (scatteredObjects.Count > 0)
        {
            GameObject obj = scatteredObjects[0];
            scatteredObjects.RemoveAt(0);
            DestroyImmediate(obj);
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
        foreach (var props in scatterProps)
        {
            Gizmos.color= new Color(0, 1, 0, 0.5f);
            Gizmos.DrawCube(transform.position, new Vector3(props.scatterArea.x, 0, props.scatterArea.y));
            Gizmos.color = Color.black;
            Gizmos.DrawWireCube(transform.position, new Vector3(props.scatterArea.x, 0, props.scatterArea.y));
        }
    }
}

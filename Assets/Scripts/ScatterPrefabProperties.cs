using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ScatterPrefabProperties
{
    public List<GameObject> Prefab;
    public float minScale = 0.9f;
    public float maxScale = 1.1f;
    public Color minColor = new Color(.9f, .9f, .9f, 1f);
    public Color maxColor = Color.white;
    public int amountToScatter = 10;
    public Vector2 scatterArea = new Vector2(185F, 185F);
    public bool useNoise = true;
    public float noiseScale = 1f;
    public float density = .5f;
    public Color AreaColor = new Color(0, 1, 0, .5f);
    public bool showArea = true;
    public bool autoClear = true;
    public float seed = 0.5f;
}

[Serializable]
public class ScatterPrefabLayers
{
    public string Name = "Layer";
    public List<ScatterPrefabProperties> Prefabs = new List<ScatterPrefabProperties>();
    private List<GameObject> SpawnedLayerObjects = new List<GameObject>();
    [HideInInspector]
    public Transform _transform;

    [Button]
    public void Spawn()
    {
        // Clear previously spawned objects if autoClear is set
        foreach (var spawnedObject in SpawnedLayerObjects)
        {
            if (spawnedObject != null)
            {
                if (Prefabs[0].autoClear)
                    UnityEngine.Object.DestroyImmediate(spawnedObject);
            }
        }
        SpawnedLayerObjects.Clear();


        //spawn
        foreach (var prefab in Prefabs)
        {
            for (int i = 0; i < prefab.amountToScatter; i++)
            {
                Vector3 randomPoint = RandomPointInArea(_transform, prefab.scatterArea);
                float noiseValue = 0f;
                if (prefab.useNoise)
                {
                    noiseValue = Mathf.PerlinNoise(prefab.seed + randomPoint.x / prefab.noiseScale, prefab.seed + randomPoint.z / prefab.noiseScale);
                }

                if (!prefab.useNoise || noiseValue > prefab.density)
                {
                    // Choose a random prefab from the list
                    GameObject randomPrefab = prefab.Prefab[UnityEngine.Random.Range(0, prefab.Prefab.Count)];
                    GameObject instance = UnityEngine.Object.Instantiate(randomPrefab, randomPoint, Quaternion.identity);

                    instance.transform.SetParent(_transform);

                    float randomScale = UnityEngine.Random.Range(prefab.minScale, prefab.maxScale);
                    instance.transform.localScale = new Vector3(randomScale, randomScale, 1);

                    Renderer renderer = instance.GetComponentInChildren<Renderer>();

                    if (renderer != null)
                    {
                        Color randomColor = new Color(UnityEngine.Random.Range(prefab.minColor.r, prefab.maxColor.r),
                                                      UnityEngine.Random.Range(prefab.minColor.g, prefab.maxColor.g),
                                                      UnityEngine.Random.Range(prefab.minColor.b, prefab.maxColor.b));
                        renderer.sharedMaterial.color = randomColor;
                    }

                    SpawnedLayerObjects.Add(instance);
                }
            }
        }
    }

    private Vector3 RandomPointInArea(Transform _transform, Vector2 area)
    {
        float halfWidth = area.x / 2;
        float halfHeight = area.y / 2;
        float x = UnityEngine.Random.Range(-halfWidth, halfWidth);
        float z = UnityEngine.Random.Range(-halfHeight, halfHeight);

        return _transform.position + new Vector3(x, 0, z);
    }

    public void DrawGizmos()
    {
        //foreach prefab in prefabs grab the prefabproperties and draw the scatter area

        foreach (var prefab in Prefabs)
        {
            if (prefab.showArea)
            {
                if (_transform == null)
                {
                    _transform = GameObject.FindFirstObjectByType<PrefabScatterer>().transform;
                }
                Gizmos.color = prefab.AreaColor;
                Gizmos.DrawCube(_transform.position, new Vector3(prefab.scatterArea.x, 0, prefab.scatterArea.y));
            }
        }
    }
}
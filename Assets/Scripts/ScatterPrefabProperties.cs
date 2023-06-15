using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[Serializable]
public class ScatterPrefabProperties
{
    [FoldoutGroup("Prefab")]
    public List<GameObject> Prefabs;
    [FoldoutGroup("Prefab")]
    public int amountToSpawn = 10;

    [FoldoutGroup("Scale")]
    public float minScale = 0.9f;
    [FoldoutGroup("Scale")]
    public float maxScale = 1.1f;

    [FoldoutGroup("Color")]
    public Color minColor = new Color(.9f, .9f, .9f, 1f);
    [FoldoutGroup("Color")]
    public Color maxColor = Color.white;

    [FoldoutGroup("Scatter")]
    public Vector2 scatterArea = new Vector2(185F, 185F);
    [FoldoutGroup("Scatter")]
    public bool useNoise = true;
    [FoldoutGroup("Scatter")]
    public float seed = 0.5f;
    [FoldoutGroup("Scatter")]
    public float noiseScale = 1f;
    [FoldoutGroup("Scatter")]
    public float density = .5f;

    [FoldoutGroup("Debug")]
    public bool showDebugArea = true;
    [FoldoutGroup("Debug")]
    public Color AreaColor = new Color(0, 1, 0, .5f);
    [FoldoutGroup("Debug")]
    public bool autoClear = true;

    [FoldoutGroup("Other")]
    public string Tag = "";
    [FoldoutGroup("Other")]
    public bool UseTag = false;
}

[Serializable]
public class ScatterPrefabLayers
{
    [FoldoutGroup("Layer Settings")]
    public string Name = "Layer";
    [FoldoutGroup("Layer Settings/Scatter Properties")]
    public ScatterPrefabProperties ScatterProperties;
    [SerializeField]
    private List<GameObject> SpawnedLayerObjects = new List<GameObject>();
    [HideInInspector]
    public Transform _transform;

    [FoldoutGroup("Layer Settings/Layer Operations"), Button(ButtonSizes.Large)]
    public void Spawn()
    {
        // Clear previously spawned objects if autoClear is set
        foreach (var spawnedObject in SpawnedLayerObjects)
        {
            if (spawnedObject != null)
            {
                if (ScatterProperties.autoClear)
                    UnityEngine.Object.DestroyImmediate(spawnedObject);
            }
        }
        SpawnedLayerObjects.Clear();


        //spawn

        for (int i = 0; i < ScatterProperties.amountToSpawn; i++)
        {
            Vector3 randomPoint = RandomPointInArea(_transform, ScatterProperties.scatterArea);
            float noiseValue = 0f;
            if (ScatterProperties.useNoise)
            {
                noiseValue = Mathf.PerlinNoise(ScatterProperties.seed + randomPoint.x / ScatterProperties.noiseScale, ScatterProperties.seed + randomPoint.z / ScatterProperties.noiseScale);
            }

            if (!ScatterProperties.useNoise || noiseValue > ScatterProperties.density)
            {
                // Choose a random prefab from the list
                GameObject randomPrefab = ScatterProperties.Prefabs[UnityEngine.Random.Range(0, ScatterProperties.Prefabs.Count)];
                GameObject instance = UnityEngine.Object.Instantiate(randomPrefab, randomPoint, Quaternion.identity);
                //set the tag of the object
                if (ScatterProperties.UseTag)
                {
                    instance.tag = ScatterProperties.Tag;
                }
                // Set the parent of the instance to the transform of the object this script is attached to
                instance.transform.SetParent(_transform);

                float randomScale = UnityEngine.Random.Range(ScatterProperties.minScale, ScatterProperties.maxScale);
                instance.transform.localScale = new Vector3(randomScale, randomScale, 1);

                Renderer renderer = instance.GetComponentInChildren<Renderer>();

                if (renderer != null)
                {
                    Color randomColor = new Color(UnityEngine.Random.Range(ScatterProperties.minColor.r, ScatterProperties.maxColor.r),
                                                  UnityEngine.Random.Range(ScatterProperties.minColor.g, ScatterProperties.maxColor.g),
                                                  UnityEngine.Random.Range(ScatterProperties.minColor.b, ScatterProperties.maxColor.b));
                    renderer.sharedMaterial.color = randomColor;
                }

                SpawnedLayerObjects.Add(instance);
            }
        }

    }

    [FoldoutGroup("Layer Settings/Layer Operations"), Button]
    public void ScatterExisting()
    {
        //reposition existing objects in spawnedlayerobjects
        foreach (var spawnedObject in SpawnedLayerObjects)
        {
            if (spawnedObject != null)
            {
                Vector3 randomPoint = RandomPointInArea(_transform, ScatterProperties.scatterArea);
                spawnedObject.transform.position = randomPoint;
            }
        }
    }

    [FoldoutGroup("Layer Settings/Layer Operations"), Button]
    public void SelectObjectsByTag()
    {
        // Create a copy of the list to iterate over
        List<GameObject> objectsToSelect = new List<GameObject>(SpawnedLayerObjects);

        // Clear the list after the objects have been destroyed

        foreach (var spawnedObject in objectsToSelect)
        {
            if (spawnedObject != null)
            {
                if (spawnedObject.tag == ScatterProperties.Tag)
                {
                    SpawnedLayerObjects.Add(spawnedObject);
                    Selection.objects = SpawnedLayerObjects.ToArray();

                }
            }
        }
    }

    [FoldoutGroup("Layer Settings/Layer Operations"), Button]
    public void ClearLayer()
    {
        // Create a copy of the list to iterate over
        List<GameObject> objectsToDestroy = new List<GameObject>(SpawnedLayerObjects);

        // Destroy all objects in spawnedlayerobjects
        foreach (var spawnedObject in objectsToDestroy)
        {
            if (spawnedObject != null)
            {
                UnityEngine.Object.DestroyImmediate(spawnedObject);
            }
        }

        // Clear the list after the objects have been destroyed
        SpawnedLayerObjects.Clear();
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

        if (ScatterProperties.showDebugArea)
        {
            if (_transform == null)
            {
                _transform = GameObject.FindFirstObjectByType<PrefabScatterer>().transform;
            }
            Gizmos.color = ScatterProperties.AreaColor;
            Gizmos.DrawCube(_transform.position, new Vector3(ScatterProperties.scatterArea.x, 0, ScatterProperties.scatterArea.y));
        }

    }
}
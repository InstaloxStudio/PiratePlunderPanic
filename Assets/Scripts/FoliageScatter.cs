using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

public class FoliageScatter : MonoBehaviour
{
    public List<GameObject> foliagePrefabs;
    public float minScale = 0.8f;
    public float maxScale = 1.2f;
    public Color minColor = Color.white;
    public Color maxColor = Color.white;
    public int amountToScatter = 100;
    public Vector2 scatterArea = new Vector2(10, 10);
    public bool autoClear = true;
    [SerializeField]
    private List<GameObject> scatteredFoliage = new List<GameObject>(); // To keep track of the scattered foliage

    //grass settings
    public List<GameObject> GrassPrefabs;
    public int grassAmountToScatter = 100;
    public Vector2 grassScatterArea = new Vector2(10, 10);
    public float grassMinScale = 0.8f;
    public float grassMaxScale = 1.2f;
    public Color grassMinColor = Color.white;
    public Color grassMaxColor = Color.white;
    public bool useNoise = true;
    public float noiseScale = 10f;
    public float foliageDensity = 0.5f;
    public float grassDensity = 0.5f;

    public float seed = 0.5f;

    [SerializeField]
    private List<GameObject> scatteredGrass = new List<GameObject>(); // To keep track of the scattered foliage

    [Button]
    public void ScatterFoliage()
    {
        if (autoClear) ClearFoliage();

        // seed = Random.Range(0f, 1f); // Use a random seed for different noise each time

        for (int i = 0; i < amountToScatter; i++)
        {
            Vector3 pointInArea = RandomPointInArea();
            if (useNoise)
            {
                // Calculate a noise value based on the position
                float noiseValue = Mathf.PerlinNoise(seed + pointInArea.x / noiseScale, seed + pointInArea.z / noiseScale);

                // Use the noise value to determine whether to scatter foliage at this position
                if (noiseValue > foliageDensity) // You can adjust this threshold to control the density
                {
                    GameObject randomFoliagePrefab = foliagePrefabs[Random.Range(0, foliagePrefabs.Count)];
                    GameObject foliageInstance = Instantiate(randomFoliagePrefab, pointInArea, Quaternion.identity);
                    foliageInstance.transform.SetParent(transform);

                    float randomScale = Random.Range(minScale, maxScale);
                    foliageInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

                    Renderer renderer = foliageInstance.GetComponentInChildren<Renderer>();
                    if (renderer != null)
                    {
                        Color randomColor = new Color(
                            Random.Range(minColor.r, maxColor.r),
                            Random.Range(minColor.g, maxColor.g),
                            Random.Range(minColor.b, maxColor.b)
                        );

                        renderer.sharedMaterial.color = randomColor;
                    }

                    scatteredFoliage.Add(foliageInstance);
                }
            }
            else
            {
                GameObject randomFoliagePrefab = foliagePrefabs[Random.Range(0, foliagePrefabs.Count)];
                GameObject foliageInstance = Instantiate(randomFoliagePrefab, pointInArea, Quaternion.identity);
                foliageInstance.transform.SetParent(transform);

                float randomScale = Random.Range(minScale, maxScale);
                foliageInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

                Renderer renderer = foliageInstance.GetComponentInChildren<Renderer>();
                if (renderer != null)
                {
                    Color randomColor = new Color(
                        Random.Range(minColor.r, maxColor.r),
                        Random.Range(minColor.g, maxColor.g),
                        Random.Range(minColor.b, maxColor.b)
                    );

                    renderer.sharedMaterial.color = randomColor;
                }

                scatteredFoliage.Add(foliageInstance);
            }
        }
    }


    [Button]
    public void ScatterGrass()
    {
        if (autoClear) ClearGrass();

        //float seed = Random.Range(0f, 1f); // Use a random seed for different noise each time

        for (int i = 0; i < grassAmountToScatter; i++)
        {
            Vector3 pointInArea = RandomPointInArea();

            float noiseValue = Mathf.PerlinNoise(seed + pointInArea.x / noiseScale, seed + pointInArea.z / noiseScale);

            if (noiseValue > grassDensity)
            {
                GameObject randomGrassPrefab = GrassPrefabs[Random.Range(0, GrassPrefabs.Count)];
                GameObject grassInstance = Instantiate(randomGrassPrefab, pointInArea, Quaternion.identity);
                grassInstance.transform.SetParent(transform);

                float randomScale = Random.Range(grassMinScale, grassMaxScale);
                grassInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

                Renderer renderer = grassInstance.GetComponent<Renderer>();
                if (renderer != null)
                {
                    Color randomColor = new Color(
                                           Random.Range(grassMinColor.r, grassMaxColor.r),
                                           Random.Range(grassMinColor.g, grassMaxColor.g),
                                           Random.Range(grassMinColor.b, grassMaxColor.b)
                                           );

                    renderer.material.color = randomColor;
                }

                scatteredGrass.Add(grassInstance); // Add the instance to our list
            }
        }
    }

    [Button]
    public void ClearFoliage()
    {
        while (scatteredFoliage.Count > 0)
        {
            GameObject foliage = scatteredFoliage[0];
            scatteredFoliage.RemoveAt(0);
            DestroyImmediate(foliage); // Use DestroyImmediate in Edit mode
        }
    }

    [Button]
    public void ClearGrass()
    {
        while (scatteredGrass.Count > 0)
        {
            GameObject grass = scatteredGrass[0];
            scatteredGrass.RemoveAt(0);
            DestroyImmediate(grass); // Use DestroyImmediate in Edit mode
        }

    }

    private Vector3 RandomPointInArea()
    {
        float halfWidth = scatterArea.x / 2;
        float halfHeight = scatterArea.y / 2;
        float x = Random.Range(-halfWidth, halfWidth);
        float z = Random.Range(-halfHeight, halfHeight);

        return transform.position + new Vector3(x, 0, z);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, new Vector3(scatterArea.x, 0, scatterArea.y));
    }



}

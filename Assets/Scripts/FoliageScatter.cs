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
    [SerializeField]
    private List<GameObject> scatteredGrass = new List<GameObject>(); // To keep track of the scattered foliage

    [Button]
    public void ScatterFoliage()
    {
        if (autoClear) ClearFoliage(); // Clear previous foliage if autoClear is enabled

        for (int i = 0; i < amountToScatter; i++)
        {
            GameObject randomFoliagePrefab = foliagePrefabs[Random.Range(0, foliagePrefabs.Count)];
            GameObject foliageInstance = Instantiate(randomFoliagePrefab, RandomPointInArea(), Quaternion.identity);
            foliageInstance.transform.SetParent(transform);

            float randomScale = Random.Range(minScale, maxScale);
            foliageInstance.transform.localScale = new Vector3(randomScale, randomScale, randomScale);

            Renderer renderer = foliageInstance.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color randomColor = new Color(
                    Random.Range(minColor.r, maxColor.r),
                    Random.Range(minColor.g, maxColor.g),
                    Random.Range(minColor.b, maxColor.b)
                );

                renderer.material.color = randomColor;
            }

            scatteredFoliage.Add(foliageInstance); // Add the instance to our list
        }
    }

    [Button]
    public void ScatterGrass()
    {

        for (int i = 0; i < grassAmountToScatter; i++)
        {
            GameObject randomGrassPrefab = GrassPrefabs[Random.Range(0, GrassPrefabs.Count)];
            GameObject grassInstance = Instantiate(randomGrassPrefab, RandomPointInArea(), Quaternion.identity);
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

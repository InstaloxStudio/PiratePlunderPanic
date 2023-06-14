using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ScatterPrefabProperties
{
    public List<GameObject> prefabs;
    public float minScale;
    public float maxScale;
    public Color minColor;
    public Color maxColor;
    public int amountToScatter;
    public Vector2 scatterArea;
    public bool useNoise;
    public float noiseScale;
    public float density;
}

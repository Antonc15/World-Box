using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkGenerator : MonoBehaviour
{
    // Serialized Fields \\
    [SerializeField] private MeshFilter terrainMeshFilter;
    [SerializeField] private MeshCollider terrainMeshCollider;

    [Header("Chunk-Gen Settings")]
    [SerializeField] [Range(0,255)] private int meshAccuracy = 32;
    [SerializeField] private int meshSize = 20;

    [Header("Biome Settings")]
    [SerializeField] private BiomeData[] biomes = new BiomeData[0];

    // Private Fields \\
    private WorldNoise world;

    // Private Methods \\
    private void Start()
    {
        world = new WorldNoise();

        for (int i = 0; i < biomes.Length; i++)
        {
            biomes[i].Initialize();
        }

        Mesh tMesh = GenerateTerrainMesh(meshAccuracy, meshAccuracy, meshSize, meshSize);

        terrainMeshFilter.mesh = tMesh;
        terrainMeshCollider.sharedMesh = tMesh;
    }

    private Mesh GenerateTerrainMesh(int _widthAccuracy, int _heightAccuracy, float _width, float _height)
    {
        float xScale = _width / (_widthAccuracy - 1);
        float yScale = _height / (_heightAccuracy - 1);

        Vector3 bottomLeft = new Vector3(_width / -2, 0, _height / -2);

        float[,] heightMap = GenerateHeightMap(_widthAccuracy, _heightAccuracy, _width, _height);

        MeshData tMeshData = new MeshData(_widthAccuracy, _heightAccuracy);

        int vertexIndex = 0;

        for (int x = 0; x < _widthAccuracy; x++)
        {
            for (int y = 0; y < _heightAccuracy; y++)
            {

                tMeshData.vertices[vertexIndex] = new Vector3(x * xScale, heightMap[x, y], y * yScale) + bottomLeft;

                if (x < _widthAccuracy - 1 && y < _heightAccuracy - 1)
                {
                    tMeshData.AddTriangle(vertexIndex, vertexIndex + _widthAccuracy + 1, vertexIndex + _widthAccuracy);
                    tMeshData.AddTriangle(vertexIndex + _widthAccuracy + 1, vertexIndex, vertexIndex + 1);
                }

                vertexIndex++;
            }
        }

        return tMeshData.CreateMesh();
    }

    private float[,] GenerateHeightMap(int _widthAccuracy, int _heightAccuracy, float _width, float _height)
    {
        float[,] heightMap = new float[_widthAccuracy, _heightAccuracy];

        float xScale = _width / (_widthAccuracy - 1);
        float yScale = _height / (_heightAccuracy - 1);

        float xBottomLeft = _width / -2;
        float yBottomLeft = _height / -2;

        for (int x = 0; x < _widthAccuracy; x++)
        {
            for (int y = 0; y < _heightAccuracy; y++)
            {
                float xSample = xBottomLeft + (x * xScale);
                float ySample = yBottomLeft + (y * yScale);

                heightMap[x, y] = GetHeightFromCoordinate(xSample, ySample);
            }
        }

        return heightMap;
    }

    private float GetHeightFromCoordinate(float _x, float _y)
    {
        TemperatureAndHumidity tempHumidity = world.GetTempHumidityFromCoordinate(_x, _y);
        int index = GetBiomeIndexFromTemperateAndHumidity(tempHumidity);

        float heightValue = biomes[index].GetNoiseFromCoordinate(_x, _y);

        return heightValue;
    }

    private int GetBiomeIndexFromTemperateAndHumidity(TemperatureAndHumidity _tempHumidity)
    {
        float temp = _tempHumidity.temperature;
        float humidity = _tempHumidity.humidity;

        for (int i = 0; i < biomes.Length; i++)
        {
            if (temp > biomes[i].MaxTemp || temp < biomes[i].MinTemp) { continue; }
            if (humidity > biomes[i].MaxHumidity || humidity < biomes[i].MinHumidity) { continue; }

            return i;
        }

        return -1;
    }
}

[System.Serializable]
public class BiomeData
{
    // Serialized Fields \\
    [SerializeField] private BiomeType biomeType;
    [SerializeField] private float minTemperature = 50f;
    [SerializeField] private float maxTemperature = 70f;
    [SerializeField] private float minHumidity = 70f;
    [SerializeField] private float maxHumidity = 80f;

    // Private Fields \\
    private BiomeNoise noise;

    // Public Methods \\
    public void Initialize()
    {
        switch (biomeType)
        {
            case BiomeType.Hills:
                noise = new HillsNoise();
                break;
            case BiomeType.Mountains:
                noise = new MountainNoise();
                break;
            default:
                noise = new BiomeNoise();
                break;
        }
    }

    public float GetNoiseFromCoordinate(float _x, float _y)
    {
        return noise.GetNoiseFromCoordinate(_x, _y);
    }

    public float MinTemp
    {
        get
        {
            return minTemperature;
        }
    }

    public float MaxTemp
    {
        get
        {
            return maxTemperature;
        }
    }

    public float MinHumidity
    {
        get
        {
            return minHumidity;
        }
    }

    public float MaxHumidity
    {
        get
        {
            return maxHumidity;
        }
    }

    // Private Classes \\
    private enum BiomeType
    {
        Hills,
        Mountains
    }
}
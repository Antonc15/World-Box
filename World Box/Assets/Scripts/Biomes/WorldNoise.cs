using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNoise
{
    // Private Fields --- Noises \\
    private FastNoise simplexNoise = new FastNoise();

    private float simplexFrequency = 0.005f;
    private int simplexOctaves = 3;

    // Private Methods \\
    private void Initialize()
    {
        simplexNoise = new FastNoise();

        simplexNoise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);

        simplexNoise.SetFrequency(simplexFrequency);
        simplexNoise.SetFractalOctaves(simplexOctaves);
    }

    // Public Methods \\
    public WorldNoise()
    {
        Initialize();
    }

    public virtual float GetNoiseFromCoordinate(float _x, float _y)
    {
        float biomeValue = simplexNoise.GetNoise(_x, _y); // value is between -1 and 1
        biomeValue += 1; // value is between 0 and 2
        biomeValue /= 2; // value is between 0 and 1
        biomeValue *= 100; // value is between 0 and 100

        return biomeValue;
    }
}

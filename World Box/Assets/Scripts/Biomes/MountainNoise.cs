using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MountainNoise : BiomeNoise
{
    // Private Fields --- Noises \\
    private FastNoise simplexNoise = new FastNoise();

    // Private Fields --- Variables \\
    private float heightMultiplier = 145f;

    private FastNoise.NoiseType noiseType = FastNoise.NoiseType.SimplexFractal;
    private float frequency = 0.003f;
    private int octaves = 8;

    // Protected Methods \\
    protected override void Initialize()
    {
        simplexNoise = new FastNoise();

        simplexNoise.SetNoiseType(noiseType);
        simplexNoise.SetFrequency(frequency);
        simplexNoise.SetFractalOctaves(octaves);
    }

    // Public Methods \\
    public override float GetNoiseFromCoordinate(float _x, float _y)
    {
        return simplexNoise.GetNoise(_x, _y) * heightMultiplier;
    }
}

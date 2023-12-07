using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeNoise
{
    // Protected Methods \\
    protected virtual void Initialize()
    {

    }

    // Public Methods \\
    public BiomeNoise()
    {
        Initialize();
    }

    public virtual float GetNoiseFromCoordinate(float _x, float _y)
    {
        return 1f;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldNoise
{
    // Private Fields -- Variables \\
    private int seed = 0;

    // Private Fields --- Noises \\
    private FastNoise temperatureNoise = new FastNoise();
    private FastNoise humidityNoise = new FastNoise();

    private float temperatureFrequency = 0.005f;
    private int temperatureOctaves = 3;

    private float humidityFrequency = 0.005f;
    private int humidityOctaves = 3;

    // Private Methods \\
    private void Initialize()
    {
        temperatureNoise = new FastNoise(seed);

        temperatureNoise.SetNoiseType(FastNoise.NoiseType.SimplexFractal);
        temperatureNoise.SetFrequency(temperatureFrequency);
        temperatureNoise.SetFractalOctaves(temperatureOctaves);

        humidityNoise = new FastNoise(seed + 1);

        humidityNoise.SetNoiseType(FastNoise.NoiseType.Simplex);
        humidityNoise.SetFrequency(humidityFrequency);
        humidityNoise.SetFractalOctaves(humidityOctaves);
    }

    // Public Methods \\
    public WorldNoise()
    {
        Initialize();
    }

    public virtual TemperatureAndHumidity GetTempHumidityFromCoordinate(float _x, float _y)
    {
        TemperatureAndHumidity tempHumidity = new TemperatureAndHumidity();

        float temperature = temperatureNoise.GetNoise(_x, _y); // value is between -1 and 1
        temperature += 1; // value is between 0 and 2
        temperature /= 2; // value is between 0 and 1
        temperature *= 100; // value is between 0 and 100

        float humidity = humidityNoise.GetNoise(_x, _y); // value is between -1 and 1
        humidity += 1; // value is between 0 and 2
        humidity /= 2; // value is between 0 and 1
        humidity *= 100; // value is between 0 and 100

        tempHumidity.temperature = temperature;
        tempHumidity.humidity = humidity;

        return tempHumidity;
    }
}

public class TemperatureAndHumidity
{
    public float temperature = 70f; // This reflects the temperature at sea level.
    public float humidity = 70f; // This reflects the humidity at sea level.
}
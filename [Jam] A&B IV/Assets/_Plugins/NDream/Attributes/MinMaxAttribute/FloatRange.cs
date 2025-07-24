using UnityEngine;

[System.Serializable]
public class FloatRange 
{
    public float min, max;
    public float currentMin, currentMax;

    public FloatRange(float min = 0, float max = 1, float currentMin = 0, float currentMax = 1)
    {
        this.min = min;
        this.max = max;

        this.currentMin = currentMin;
        this.currentMax = currentMax;
    }

    /// <summary>
    /// Get a random value between min and max
    /// </sumary>
    public float GetRandom()
    {
        return Random.Range(currentMin, currentMax);
    }
}

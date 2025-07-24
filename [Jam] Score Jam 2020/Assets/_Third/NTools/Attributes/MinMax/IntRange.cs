using UnityEngine;

[System.Serializable]
public class IntRange
{
    public int min, max;
    public int currentMin, currentMax;

    public IntRange(int min = 0, int max = 1, int currentMin = 0, int currentMax = 1)
    {
        this.min = min;
        this.max = max;

        this.currentMin = currentMin;
        this.currentMax = currentMax;
    }

    public int GetRandom()
        => Random.Range(currentMin, currentMax);
}
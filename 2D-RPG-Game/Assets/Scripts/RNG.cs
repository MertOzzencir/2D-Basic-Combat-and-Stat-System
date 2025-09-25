using UnityEngine;

public static class RNG 
{
    private static float _rollValue;

    public static bool CalculateRNG(float _chance)
    {
        float baseChance = _chance;
        _rollValue = Random.Range(0, 100);

        return _rollValue <= baseChance ? true : false;

    }
}

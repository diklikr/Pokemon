using UnityEngine;
public class PokemonTypes
{
    public enum TypeList
    {
        Fire,
        Water,
        Air,
        Stone,
        Fairy,
        none
    }

    // 6x6 chart matching the enum order. Adjust values as your design requires.
    public float[,] typeStrengths = new float[,]
    {
        {1f, 0.5f, 1f, 2f, 1f, 1f}, // Fire
        {2f, 1f, 0.5f, 1f, 1f, 1f}, // Water
        {1f, 2f, 1f, 0.5f, 1f, 1f}, // Air
        {0.5f, 1f, 2f, 1f, 1f, 1f}, // Stone
        {1f, 1f, 1f, 1f, 2f, 1f},   // Fairy
        {1f, 1f, 1f, 1f, 1f, 1f}    // none
    };
}
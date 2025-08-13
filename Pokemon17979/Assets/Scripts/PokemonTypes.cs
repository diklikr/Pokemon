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

    public float[,] typeStrenghts = new float[,]
    {
        {1f, 0.5f, 1f, 2f, 1f }, //fire
        {2f, 1f, 0.5f, 1f, 1f }, //Water
        {1f, 2f, 1f, 0.5f, 1f }, //Air
        {0.5f, 1f, 2f, 1f, 1f }, //stone
        {1f, 1f, 1f, 1f, 2f }, //fairy
        {1f, 1f, 1f, 1f, 1f }//none
    };
}

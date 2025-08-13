using UnityEditor.Build;
using UnityEngine;

[CreateAssetMenu(fileName = "Pokemon", menuName = "Scriptable Objects/Pokemon")]
public class Pokemon : ScriptableObject
{
    public string PokemonName => m_PokemonName;
    public string Description => m_description;
    public Sprite Sprite => m_sprite;
    public GameObject Mode1 => m_Mode1;
    public int Health => m_health;
    public int Attack => m_attack;
    public int SpecialAttack => m_specialAttack;
    public int Defense => m_defense;
    public int SpecialDefense => m_specialDefense;
    public int Speed => m_speed;
    public PokemonTypes.TypeList MainType => m_MainType;
    public PokemonTypes.TypeList SecondaryType => m_SecondaryType;
    public int EvolutionLevel => m_Evolutionlevel;
    public Pokemon Evolution => m_Evolution;
    public PokemonMove[] Moves => m_Moves;

    [Header("Pokemon Info")]
    [SerializeField] private string m_PokemonName;
    [SerializeField] private string m_description;
    [SerializeField] private Sprite m_sprite;
    [SerializeField] private GameObject m_Mode1;

    [Header("Pokemon Stats")]
    [SerializeField, Range(0,100)]private int m_health;
    [SerializeField, Range(0,100)] private int m_attack;
    [SerializeField, Range(0,100)] private int m_specialAttack;
    [SerializeField, Range(0,100)] private int m_defense;
    [SerializeField, Range(0,100)] private int m_specialDefense;
    [SerializeField, Range(0,100)] private int m_speed;

    [SerializeField] private PokemonMove[] m_Moves;

    [Header("Pokemon Types")]
    [SerializeField] private PokemonTypes.TypeList m_MainType;
    [SerializeField] private PokemonTypes.TypeList m_SecondaryType;

    [Header("Pokemon Evolution")]
    [SerializeField] private int m_Evolutionlevel;
    [SerializeField] private Pokemon m_Evolution;

    private void OnValidate()
    {
        if (m_Moves.Length > 4)
        {
            Debug.LogWarning("A Pokémon can only have 4 moves. The excess moves will be ignored.");
            System.Array.Resize(ref m_Moves, 4);
        }
    }
}

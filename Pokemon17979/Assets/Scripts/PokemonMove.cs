using UnityEngine;

[CreateAssetMenu(fileName = "NewMove", menuName = "Pokemon/Move")]
public class PokemonMove : ScriptableObject
{
    #region Getters
    public string MoveName => m_MoveName;
    public string MoveDescription => m_MoveDescription;
    public int Power => m_Power;
    public int Accuracy => m_Accuracy;
    public int PowerPoints => m_PowerPoints;
    public bool IsSpecial => m_IsSpecial;
    public PokemonTypes.TypeList MoveType => m_MoveType;
    public string AnimationName => m_AnimationName;
    public Sprite Icon => m_Icon;
    public AudioClip SoundEffect => m_SoundEffect;
    public GameObject Effect => m_Effect;
    #endregion

    [Header("Info")]
    [SerializeField] private string m_MoveName;
    [SerializeField, TextArea] private string m_MoveDescription;

    [Header("Stats")]
    [SerializeField] private int m_Power;
    [SerializeField, Range(0,100)] private int m_Accuracy;
    [SerializeField] private int m_PowerPoints;
    [SerializeField] private bool m_IsSpecial;
    [SerializeField] private PokemonTypes.TypeList m_MoveType;

    [Header("Effects")]
    [SerializeField] private string m_AnimationName;
    [SerializeField] private Sprite m_Icon;
    [SerializeField] private AudioClip m_SoundEffect;
    [SerializeField] GameObject m_Effect;

   
}

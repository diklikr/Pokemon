using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager m_instance;

    [SerializeField] public Pokemon poke1;
    [SerializeField] public Pokemon poke2;
    [SerializeField] public Pokemon poke3;
    [SerializeField] public Pokemon poke4;


    //[SerializeField]
    //float GlobalXPRate = 1;

    GameObject CombatArenaPrefab;
    
    
    public static GameManager GetInstance()
    {
        if(m_instance != null) { return m_instance; }
        m_instance = FindAnyObjectByType<GameManager>();
        if(m_instance != null) { return (m_instance); }
        GameObject gameManagerObject = new GameObject("Game Manager");
        m_instance = gameManagerObject.AddComponent<GameManager>();
        return (m_instance);
    }
    #endregion
    public static GameObject newCombatArena => Instantiate(GetInstance().CombatArenaPrefab);
    [SerializeField] private GameObject m_CombatArenaPrefab;

    [SerializeField] private GameObject PokemonPreFab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_instance = this;
        DontDestroyOnLoad(gameObject);
    }


    // Update is called once per frame
    public static PokemonComponent SpawnPokemon(Pokemon p_Pokemon,Vector3 p_Position)
    {
        PokemonComponent pokemonComponent = Instantiate(GetInstance().PokemonPreFab, p_Position, Quaternion.identity).GetComponent<PokemonComponent>();
        pokemonComponent.Initialize(p_Pokemon);
        return pokemonComponent;
    }
    public Pokemon GetRandomPokemon()
    {
        Pokemon[] pokemons = { poke1, poke2, poke3, poke4 };
        int randomIndex = Random.Range(0, pokemons.Length);
        return pokemons[randomIndex];
    }
    public static void StartCombat(Pokemon playerPoke)
    {
        SceneManager.LoadScene("CombatScene");
        SpawnPokemon(playerPoke, new Vector3(0, 0, 20));
        SpawnPokemon(, new Vector3(10, 0, 20));
    }
}

using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager m_instance;

    [SerializeField] public Pokemon poke1;
    [SerializeField] public Pokemon poke2;
    [SerializeField] public Pokemon poke3;

    GameObject CombatArenaPrefab;

    public static GameManager GetInstance()
    {
        if (m_instance != null) { return m_instance; }
        m_instance = FindAnyObjectByType<GameManager>();
        if (m_instance != null) { return (m_instance); }
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
    public static PokemonComponent SpawnPokemon(Pokemon p_Pokemon, Vector3 p_Position)
    {
        PokemonComponent pokemonComponent = Instantiate(GetInstance().PokemonPreFab, p_Position, Quaternion.identity).GetComponent<PokemonComponent>();
        pokemonComponent.Initialize(p_Pokemon);
        return pokemonComponent;
    }
    public Pokemon GetRandomPokemon()
    {
        Pokemon[] pokemons = { poke1, poke2, poke3 };
        int randomIndex = Random.Range(0, pokemons.Length);
        return pokemons[randomIndex];
    }
    public static async Task StartCombat(Pokemon playerPoke)
    {
        // Load combat scene
        var sceneCombat = SceneManager.LoadSceneAsync("CombatScene");
        while (!sceneCombat.isDone) await Task.Yield();

        // Instantiate the combat arena prefab (if assigned in inspector). 
        // This prefab should contain CombatManager and your UI (MovesUI).
        if (GetInstance().m_CombatArenaPrefab != null)
        {
            Instantiate(GetInstance().m_CombatArenaPrefab);
        }

        // Wait a few frames for the CombatManager (and arena) to initialize
        int attempts = 0;
        while (CombatManager.Instance == null && attempts++ < 30)
            await Task.Yield();

        var cm = CombatManager.Instance;
        if (cm == null)
        {
            Debug.LogWarning("CombatManager instance not found after loading CombatScene. Make sure the CombatArena prefab contains a CombatManager component.");
            return;
        }

        // Spawn player and enemy pokemons
        PokemonComponent playerComp = SpawnPokemon(playerPoke, new Vector3(0, 0, 20));
        Pokemon enemy = GetInstance().GetRandomPokemon();
        PokemonComponent enemyComp = SpawnPokemon(enemy, new Vector3(10, 0, 20));

        // Assign spawned components to the CombatManager so it has valid references
        cm.playerPokemon = playerComp;
        cm.enemyPokemon = enemyComp;

        // Explicitly bind the UI (MovesUI) so sliders and buttons reflect the spawned pokemons
        var movesUI = FindObjectOfType<MovesUI>();
        if (movesUI != null)
        {
            movesUI.Bind(cm.playerPokemon.m_PokemonInfo, cm.enemyPokemon.m_PokemonInfo);
            movesUI.UpdateNow(); // ensure sliders show full HP immediately
            movesUI.Refresh();   // populate move buttons
        }
        else
        {
            Debug.LogWarning("MovesUI not found in scene. Make sure the CombatArena prefab contains a MovesUI component on the HUD Canvas.");
        }

        // Start the combat round (or let UI trigger it)
        cm.StartNewRound();
    }
}

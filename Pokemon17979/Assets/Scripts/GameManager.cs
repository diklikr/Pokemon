// name=GameManager.cs
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager m_instance;

    [Header("Enemy pool")]
    [SerializeField] public Pokemon poke1;
    [SerializeField] public Pokemon poke2;
    [SerializeField] public Pokemon poke3;


    [SerializeField] private GameObject m_PokemonPreFab;     // fallback prefab with PokemonComponent on root

    public static GameManager GetInstance()
    {
        if (m_instance != null) return m_instance;
        m_instance = FindAnyObjectByType<GameManager>();
        if (m_instance != null) return m_instance;
        GameObject gm = new GameObject("Game Manager");
        m_instance = gm.AddComponent<GameManager>();
        return m_instance;
    }
    #endregion

    void Start()
    {
        m_instance = this;
        DontDestroyOnLoad(gameObject);
    }

    // Spawn helper: instantiates Pokemon prefab and initializes PokemonComponent
    public static PokemonComponent SpawnPokemon(Pokemon p_Pokemon, Vector3 p_Position)
    {
        if (GetInstance().m_PokemonPreFab == null)
        {
            Debug.LogError("GameManager.SpawnPokemon: PokemonPreFab is not assigned in the GameManager inspector.");
            return null;
        }

        GameObject go = Instantiate(GetInstance().m_PokemonPreFab, p_Position, Quaternion.identity);
        if (go == null)
        {
            Debug.LogError("GameManager.SpawnPokemon: Instantiate returned null.");
            return null;
        }

        PokemonComponent comp = go.GetComponent<PokemonComponent>();
        if (comp == null)
        {
            Debug.LogError("GameManager.SpawnPokemon: PokemonPreFab does not have a PokemonComponent on the root GameObject.");
            return null;
        }

        comp.Initialize(p_Pokemon);
        Debug.Log($"GameManager.SpawnPokemon: Spawned '{p_Pokemon.PokemonName}' at {p_Position}. Mode1 assigned: {p_Pokemon.Mode1 != null}");
        return comp;
    }

    // Returns a random Pokemon from configured list
    public Pokemon GetRandomPokemon()
    {
        Pokemon[] pokemons = { poke1, poke2, poke3 };
        int randomIndex = Random.Range(0, pokemons.Length);
        return pokemons[randomIndex];
    }
    public static void StartCombat(Pokemon playerPoke)
    {
        GetInstance().StartCoroutine(GetInstance().StartCombatCoroutine(playerPoke));
    }

    private IEnumerator StartCombatCoroutine(Pokemon playerPoke)
    {
        if (playerPoke == null)
        {
            Debug.LogError("StartCombatCoroutine: playerPoke is null.");
            yield break;
        }

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("CombatScene", LoadSceneMode.Single);
        while (!asyncLoad.isDone)
            yield return null;

        yield return null;

        // Get CombatManager in the loaded combat scene
        var cm = CombatManager.Instance;
        if (cm == null)
        {
            Debug.LogWarning("StartCombatCoroutine: CombatManager not found in CombatScene. Make sure CombatScene contains a CombatManager object.");
        }

        // Determine spawn points (prefer transforms on CombatManager if provided)
        Vector3 playerPos = (cm != null && cm.playerSpawn != null) ? cm.playerSpawn.position : new Vector3(-2f, 0f, 0f);
        Vector3 enemyPos = (cm != null && cm.enemySpawn != null) ? cm.enemySpawn.position : new Vector3(2f, 0f, 0f);

        // PLAYER: prefer existing placeholder on CombatManager, else fallback spawn
        PokemonComponent playerComp = null;
        if (cm != null && cm.playerPokemon != null)
        {
            playerComp = cm.playerPokemon;
            playerComp.Initialize(playerPoke);
            Debug.Log("StartCombatCoroutine: Initialized existing player placeholder.");
        }
        else
        {
            if (m_PokemonPreFab != null)
            {
                playerComp = SpawnPokemon(playerPoke, playerPos);
                if (cm != null) cm.playerPokemon = playerComp;
                Debug.Log("StartCombatCoroutine: Spawned player via fallback prefab.");
            }
            else
            {
                Debug.LogError("StartCombatCoroutine: No player placeholder in CombatManager and GameManager.m_PokemonPreFab is null.");
            }
        }

        // ENEMY: use random SO and prefer placeholder if present
        Pokemon enemySO = GetRandomPokemon();
        PokemonComponent enemyComp = null;
        if (cm != null && cm.enemyPokemon != null)
        {
            enemyComp = cm.enemyPokemon;
            enemyComp.Initialize(enemySO);
            Debug.Log("StartCombatCoroutine: Initialized existing enemy placeholder.");
        }
        else
        {
            if (m_PokemonPreFab != null)
            {
                enemyComp = SpawnPokemon(enemySO, enemyPos);
                if (cm != null) cm.enemyPokemon = enemyComp;
                Debug.Log("StartCombatCoroutine: Spawned enemy via fallback prefab.");
            }
            else
            {
                Debug.LogError("StartCombatCoroutine: No enemy placeholder in CombatManager and GameManager.m_PokemonPreFab is null.");
            }
        }

        // If CombatManager provides an InitializeCombat helper, call it for centralized setup
        if (cm != null)
        {
            cm.InitializeCombat(playerComp, enemyComp);
        }

        // Bind MovesUI (UI must exist in CombatScene or arena prefab)
        var movesUI = FindFirstObjectByType<MovesUI>();
        if (movesUI != null && playerComp != null && enemyComp != null)
        {
            movesUI.Bind(playerComp.m_PokemonInfo, enemyComp.m_PokemonInfo);
            movesUI.UpdateNow();
            movesUI.Refresh();
            Debug.Log("StartCombatCoroutine: MovesUI bound and refreshed.");
        }
        else
        {
            if (movesUI == null) Debug.LogWarning("StartCombatCoroutine: MovesUI not found in scene.");
            if (playerComp == null || enemyComp == null) Debug.LogWarning("StartCombatCoroutine: player or enemy PokemonComponent missing after spawn/initialize.");
        }

        // Start the combat flow
        if (cm != null)
        {
            cm.StartNewRound();
            Debug.Log("StartCombatCoroutine: CombatManager.StartNewRound called.");
        }

        yield break;
    }
}
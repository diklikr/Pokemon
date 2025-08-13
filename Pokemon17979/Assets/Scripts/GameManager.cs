using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Singleton
    private static GameManager m_instance;

    [SerializeField]
    float GlobalXPRate = 1;

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

    //public static void StartCombat()
    //{
    //    SceneManager.CreateScene("CombatArena", new CreateSceneParameters(LocalPhysicsMode.Physics3D));
    //    Scene combatArenaScene = SceneManager.GetSceneByName("CombatArena");
    //    if (combatArenaScene.isLoaded)
    //    {
    //        SceneManager.SetActiveScene(combatArenaScene);
    //        newCombatArena.transform.position = Vector3.zero;
    //    }
    //    else
    //    {
    //        Debug.LogError("Combat Arena Scene could not be loaded");
    //    }
    //}

    // Update is called once per frame
   public static PokemonComponent SpawnPokemon(Pokemon p_Pokemon, Vector3 p_Position)
    {
        PokemonComponent pokemonComponent = Instantiate(GetInstance().PokemonPreFab, p_Position, Quaternion.identity).GetComponent<PokemonComponent>();
        pokemonComponent.Initialize(p_Pokemon);
        return pokemonComponent;
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovesUI : MonoBehaviour
{
    [Header("UI")]
    public Button[] moveButtons = new Button[2];
    public TextMeshProUGUI[] moveLabels = new TextMeshProUGUI[2];

    private CombatManager cm;
    [Header("Assign in Inspector")]
    public Slider playerSlider;
    public Slider enemySlider;

    private Pokemoninformation playerInfo;
    private Pokemoninformation enemyInfo;

    private bool _bound = false; // ensures bind happens only once

    private void Start()
    {
        cm = CombatManager.Instance;
        Refresh();
        TryAutoBind(); // attempt immediate bind in case CM already has pokemon
    }

    private void Update()
    {
        // Try bind if not yet bound and CombatManager has spawned pokemons
        if (!_bound)
            TryAutoBind();

        // Update slider values every frame (keeps bars in sync while combat runs)
        if (playerInfo != null && playerSlider != null)
            playerSlider.value = Mathf.Clamp(playerInfo.CurrentHP, 0, playerInfo.MaxHealth);

        if (enemyInfo != null && enemySlider != null)
            enemySlider.value = Mathf.Clamp(enemyInfo.CurrentHP, 0, enemyInfo.MaxHealth);
    }

    // Attempt to bind when CombatManager and its PokemonComponents are ready
    private void TryAutoBind()
    {
        if (_bound) return;
        if (cm == null) cm = CombatManager.Instance;
        if (cm == null) return;

        if (cm.playerPokemon != null && cm.enemyPokemon != null)
        {
            Bind(cm.playerPokemon.m_PokemonInfo, cm.enemyPokemon.m_PokemonInfo);
            UpdateNow(); // ensure sliders show full health immediately
            _bound = true;
            Refresh(); // now populate move buttons based on bound player
        }
    }

    // Bind the two Pokemon informations to the sliders
    public void Bind(Pokemoninformation player, Pokemoninformation enemy)
    {
        playerInfo = player;
        enemyInfo = enemy;

        if (playerSlider != null)
        {
            playerSlider.wholeNumbers = true;
            playerSlider.maxValue = playerInfo != null ? playerInfo.MaxHealth : 1;
            playerSlider.value = playerInfo != null ? playerInfo.CurrentHP : 0;
        }

        if (enemySlider != null)
        {
            enemySlider.wholeNumbers = true;
            enemySlider.maxValue = enemyInfo != null ? enemyInfo.MaxHealth : 1;
            enemySlider.value = enemyInfo != null ? enemyInfo.CurrentHP : 0;
        }
    }

    // Force immediate visual update (use after Bind or after damage/heal)
    public void UpdateNow()
    {
        if (playerInfo != null && playerSlider != null)
        {
            playerSlider.maxValue = playerInfo.MaxHealth;
            playerSlider.value = playerInfo.CurrentHP;
        }

        if (enemyInfo != null && enemySlider != null)
        {
            enemySlider.maxValue = enemyInfo.MaxHealth;
            enemySlider.value = enemyInfo.CurrentHP;
        }
    }

    public void Refresh()
    {
        if (cm == null || cm.playerPokemon == null)
        {
            ClearAll();
            return;
        }

        var moves = cm.playerPokemon.m_PokemonInfo.Moves;
        for (int i = 0; i < moveButtons.Length; i++)
        {
            bool hasMove = moves != null && i < moves.Length && moves[i] != null;
            string label = hasMove ? moves[i].MoveName : "—";
            SetupButton(i, label, hasMove);
        }
    }

    private void ClearAll()
    {
        for (int i = 0; i < moveButtons.Length; i++)
            SetupButton(i, "—", false);
    }

    private void SetupButton(int index, string label, bool interactable)
    {
        if (moveLabels != null && index < moveLabels.Length && moveLabels[index] != null)
            moveLabels[index].text = label;

        if (moveButtons == null || index >= moveButtons.Length || moveButtons[index] == null)
            return;

        Button btn = moveButtons[index];

        btn.onClick.RemoveAllListeners();
        btn.gameObject.SetActive(interactable);

        if (interactable)
        {
            int capture = index;
            btn.onClick.AddListener(() => OnMoveButton(capture));
        }
    }

    public void OnMoveButton(int index)
    {
        if (cm == null)
        {
            Debug.LogWarning("MoveUIController: CombatManager instance not found.");
            return;
        }

        var playerComp = cm.playerPokemon;
        var enemyComp = cm.enemyPokemon;
        if (playerComp == null || enemyComp == null)
        {
            Debug.LogWarning("MoveUIController: player or enemy PokemonComponent is not assigned on CombatManager.");
            return;
        }

        var moves = playerComp.m_PokemonInfo.Moves;
        if (moves == null || index < 0 || index >= moves.Length || moves[index] == null)
        {
            Debug.LogWarning($"MoveUIController: invalid move index {index}.");
            return;
        }

        PokemonMove chosen = moves[index];

        // Enqueue player turn
        cm.turnQueue.Enqueue(new Turn(playerComp.m_PokemonInfo, enemyComp.m_PokemonInfo, chosen));

        // Enemy chooses a random move (simple response)
        PokemonMove enemyMove = enemyComp.UseRandomMove();
        if (enemyMove != null)
            cm.turnQueue.Enqueue(new Turn(enemyComp.m_PokemonInfo, playerComp.m_PokemonInfo, enemyMove));

        // Start processing turns
        cm.PlayNextTurn();

        Refresh();
    }
}
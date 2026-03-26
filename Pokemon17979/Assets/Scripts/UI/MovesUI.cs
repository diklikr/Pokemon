using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MovesUI : MonoBehaviour
{
    [Header("UI")]
    public Button[] moveButtons = new Button[2];
    public TextMeshProUGUI[] moveLabels = new TextMeshProUGUI[2];

    [Header("HP Sliders")]
    public Slider playerSlider;
    public Slider enemySlider;

    private Pokemoninformation playerInfo;
    private Pokemoninformation enemyInfo;

    // Bind the two Pokemon informations to the UI (called by GameManager after spawning/initializing)
    public void Bind(Pokemoninformation player, Pokemoninformation enemy)
    {
        // unsubscribe previous if any (not strictly necessary here)
        if (playerInfo != null)
        {
            playerInfo.OnHPChanged -= OnPlayerHPChanged;
        }
        if (enemyInfo != null)
        {
            enemyInfo.OnHPChanged -= OnEnemyHPChanged;
        }

        playerInfo = player;
        enemyInfo = enemy;

        // subscribe to HP changes
        if (playerInfo != null)
            playerInfo.OnHPChanged += OnPlayerHPChanged;
        if (enemyInfo != null)
            enemyInfo.OnHPChanged += OnEnemyHPChanged;

        // Initialize sliders immediately
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

        Refresh();
    }

    private void OnPlayerHPChanged(int cur, int max)
    {
        if (playerSlider != null)
        {
            playerSlider.maxValue = max;
            playerSlider.value = cur;
        }
    }

    private void OnEnemyHPChanged(int cur, int max)
    {
        if (enemySlider != null)
        {
            enemySlider.maxValue = max;
            enemySlider.value = cur;
        }
    }

    private void Start()
    {
        Refresh();
    }

    private void Update()
    {
        // safety: keep sliders in sync in case of missing events
        if (playerInfo != null && playerSlider != null)
            playerSlider.value = Mathf.Clamp(playerInfo.CurrentHP, 0, playerInfo.MaxHealth);
        if (enemyInfo != null && enemySlider != null)
            enemySlider.value = Mathf.Clamp(enemyInfo.CurrentHP, 0, enemyInfo.MaxHealth);
    }

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
        if (playerInfo == null)
        {
            var cm = CombatManager.Instance;
            if (cm != null && cm.playerPokemon != null)
                playerInfo = cm.playerPokemon.m_PokemonInfo;
        }

        if (playerInfo == null)
        {
            ClearAll();
            return;
        }

        var moves = playerInfo.Moves;
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

    // Called by the button click
    public void OnMoveButton(int index)
    {
        var cm = CombatManager.Instance;
        if (cm == null)
        {
            Debug.LogWarning("MovesUI: CombatManager instance not found.");
            return;
        }

        var playerComp = cm.playerPokemon;
        var enemyComp = cm.enemyPokemon;
        if (playerComp == null || enemyComp == null)
        {
            Debug.LogWarning("MovesUI: player or enemy PokemonComponent is not assigned on CombatManager.");
            return;
        }

        var moves = playerComp.m_PokemonInfo.Moves;
        if (moves == null || index < 0 || index >= moves.Length || moves[index] == null)
        {
            Debug.LogWarning($"MovesUI: invalid move index {index}.");
            return;
        }

        PokemonMove chosen = moves[index];

        // Notify CombatManager of the chosen move (event-driven)
        cm.ChoosePlayerMove(chosen);

        // Disable buttons until next WaitforActionState.Enter re-enables them
        SetButtonsInteractable(false);
    }

    // Allow WaitforActionState to enable/disable buttons
    public void SetButtonsInteractable(bool enabled)
    {
        foreach (var b in moveButtons)
        {
            if (b != null)
                b.interactable = enabled;
        }
    }
}
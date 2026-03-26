using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CombatManager : StateMachine
{
    #region Singleton
    public static CombatManager Instance => GetInstance();
    private static CombatManager m_instance;

    private static CombatManager GetInstance()
    {
        if (m_instance == null)
        {
            m_instance = FindAnyObjectByType<CombatManager>();
            if (m_instance == null)
            {
                GameObject obj = new GameObject("CombatManager");
                m_instance = obj.AddComponent<CombatManager>();
            }
        }
        return m_instance;
    }
    #endregion
    public Transform playerSpawn;
    public Transform enemySpawn;

    [Header("Runtime references (can be placeholders set in the scene)")]
    public PokemonComponent playerPokemon;
    public PokemonComponent enemyPokemon;

    public Queue<Turn> turnQueue = new Queue<Turn>();

    // Event fired when the player chooses a move
    public event Action OnPlayerMoveChosen;

    // Player's chosen move (set by UI via ChoosePlayerMove)
    public PokemonMove playerChosenMove;

    public void InitializeCombat(PokemonComponent player, PokemonComponent enemy)
    {
        playerPokemon = player;
        enemyPokemon = enemy;
        Debug.Log("CombatManager.InitializeCombat: player/enemy assigned.");
    }

    // Called by MovesUI when the player picks a move
    public void ChoosePlayerMove(PokemonMove move)
    {
        if (move == null)
        {
            Debug.LogWarning("ChoosePlayerMove: null move passed.");
            return;
        }
        playerChosenMove = move;
        OnPlayerMoveChosen?.Invoke();
    }

    public void StartNewRound()
    {
        turnQueue.Clear();
        ChangeState(new WaitforActionState());
    }

    public void BuildTurnQueue()
    {
        if (playerPokemon == null || enemyPokemon == null)
        {
            Debug.LogWarning("BuildTurnQueue: missing player or enemy Pokemon.");
            return;
        }

        if (playerChosenMove == null)
        {
            Debug.LogWarning("BuildTurnQueue: no player move chosen.");
            return;
        }

        // Enemy chooses a move
        PokemonMove enemyMove = enemyPokemon.UseRandomMove();

        var pInfo = playerPokemon.m_PokemonInfo;
        var eInfo = enemyPokemon.m_PokemonInfo;

        // Decide ordering by speed
        if (pInfo.Speed >= eInfo.Speed)
        {
            turnQueue.Enqueue(new Turn(pInfo, eInfo, playerChosenMove));
            if (enemyMove != null)
                turnQueue.Enqueue(new Turn(eInfo, pInfo, enemyMove));
        }
        else
        {
            if (enemyMove != null)
                turnQueue.Enqueue(new Turn(eInfo, pInfo, enemyMove));
            turnQueue.Enqueue(new Turn(pInfo, eInfo, playerChosenMove));
        }

        // Clear chosen move so it is not reused
        playerChosenMove = null;
    }

    public void PlayNextTurn()
    {
        if (turnQueue.Count == 0)
        {
            StartNewRound();
        }
        else
        {
            Turn t_NextTurn = turnQueue.Dequeue();
            t_NextTurn.StartTurn();
        }
    }

    public static int CalculateDamage(PokemonMove move, Pokemoninformation p_Attacker, Pokemoninformation p_Defender)
    {
        float damage;
        if (move.IsSpecial)
        {
            damage = 5f + move.Power * ((float)p_Attacker.SpecialAttack / Mathf.Max(1, p_Defender.SpecialDefense));
        }
        else
        {
            damage = 5f + move.Power * ((float)p_Attacker.Attack / Mathf.Max(1, p_Defender.Defense));
        }

        int finalDamage = Mathf.Max(1, Mathf.RoundToInt(damage));
        return finalDamage;
    }

    // Called when a Pokemon hits 0 HP
    public void HandleFainted(Pokemoninformation fainted)
    {
        // prevent further turns while handling
        turnQueue.Clear();

        if (playerPokemon != null && playerPokemon.m_PokemonInfo == fainted)
        {
            Debug.Log("CombatManager: Player fainted. Loading GameOver scene.");
            SceneManager.LoadScene("GameOver");
            return;
        }

        if (enemyPokemon != null && enemyPokemon.m_PokemonInfo == fainted)
        {
            Debug.Log("CombatManager: Enemy fainted. Loading Victory scene.");
            SceneManager.LoadScene("Victory");
            return;
        }

        Debug.LogWarning("CombatManager.HandleFainted: fainted pokemon not recognized.");
    }
}
using UnityEngine;
using System.Collections.Generic;

public class CombatManager : StateMachine
{
    #region Singleton

    public static GameObject newCombatArena;

    public Queue<Turn> turnQueue = new Queue<Turn>();

    public static CombatManager Instance => GetInstance();
    private static CombatManager m_instance;

    public PokemonComponent playerPokemon;
    public PokemonComponent enemyPokemon;

    public PokemonMove PokemonMove;
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

    public void StartCombat(Pokemon p_Pokemon1, Pokemon p_Pokemon2)
    {
        GameObject combatArena = GameManager.newCombatArena;
        Instance.playerPokemon = GameManager.SpawnPokemon(p_Pokemon1, Vector3.zero);
        Instance.enemyPokemon = GameManager.SpawnPokemon(p_Pokemon2, Vector3.zero * 5f);
        StartNewRound();
    }

    public void StartNewRound()
    {
        Instance.turnQueue.Clear();
        Instance.ChangeState(new WaitforActionState());

    }
    public void BuildTurnQueue()
    {
        Pokemoninformation fastestPokemon;
        Pokemoninformation slowestPokemon;

        if (Instance.playerPokemon.m_PokemonInfo.Speed >= Instance.enemyPokemon.m_PokemonInfo.Speed)
        {
            fastestPokemon = Instance.playerPokemon.m_PokemonInfo;
            slowestPokemon = Instance.enemyPokemon.m_PokemonInfo;
        }
        else
        {
            fastestPokemon = Instance.enemyPokemon.m_PokemonInfo;
            slowestPokemon = Instance.playerPokemon.m_PokemonInfo;
        }
        turnQueue.Enqueue(new Turn(fastestPokemon, slowestPokemon));
        turnQueue.Enqueue(new Turn(slowestPokemon, fastestPokemon));
    }

    public float CalculateDamage(PokemonMove move, Pokemoninformation p_Attacker, Pokemoninformation p_Defender)
    {
        if (move.IsSpecial)
        {
            return 5 + move.Power * (p_Attacker.SpecialAttack / p_Defender.Defense);
        }
        else
        {
            return 5 + move.Power * (p_Attacker.Attack / p_Defender.SpecialDefense);
        }

    }
}
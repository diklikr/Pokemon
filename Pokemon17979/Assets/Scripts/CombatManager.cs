using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.SceneManagement;

public class CombatManager : StateMachine
{
    #region Singleton
    public static GameObject newCombatArena;

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
    public Queue<Turn> turnQueue = new Queue<Turn>();
    Pokemon pokemonAleatorio;
    public PokemonComponent playerPokemon;
    public PokemonComponent enemyPokemon;
    public PokemonMove PokemonMove;

    
    public void StartNewRound()
    {
        Instance.turnQueue.Clear();
        Instance.ChangeState(new WaitforActionState());
    }
    public void BuildTurnQueue()
    {
        Pokemoninformation fastestPokemon;
        Pokemoninformation slowestPokemon;
        PokemonMove fastestmove, slowestMove;

        if (Instance.playerPokemon.m_PokemonInfo.Speed >= Instance.enemyPokemon.m_PokemonInfo.Speed)
        {
            fastestPokemon = Instance.playerPokemon.m_PokemonInfo;
            fastestmove = Instance.playerPokemon.UseRandomMove();
            slowestPokemon = Instance.enemyPokemon.m_PokemonInfo;
            slowestMove = Instance.enemyPokemon.UseRandomMove();
        }
        else
        {
            fastestPokemon = Instance.enemyPokemon.m_PokemonInfo;
            fastestmove = Instance.enemyPokemon.UseRandomMove();
            slowestPokemon = Instance.playerPokemon.m_PokemonInfo;
            slowestMove = Instance.playerPokemon.UseRandomMove();
        }
        Instance.turnQueue.Enqueue(new Turn(fastestPokemon, slowestPokemon, fastestmove));
        Instance.turnQueue.Enqueue(new Turn(slowestPokemon, fastestPokemon, slowestMove));
    }

    public void PlayNextTurn()
    {
        if(Instance.turnQueue.Count == 0)
        {
            StartNewRound();
        }
        else
        {
            Turn t_NextTurn = Instance.turnQueue.Dequeue();
            t_NextTurn.StartTurn();
        }
    }

    public static int CalculateDamage(PokemonMove move, Pokemoninformation p_Attacker, Pokemoninformation p_Defender)
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
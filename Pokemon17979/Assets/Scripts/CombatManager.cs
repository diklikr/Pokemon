using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public CombatUI UI;
    public InfoBar m_UI;

    public static void SetPlayerMove(PokemonMove pMove)
    {
        Debug.Log("Chose Move");
        Instance.PokemonMove = pMove;
    }
    
    public void StartNewRound()
    {
        Instance.turnQueue.Clear();
        Instance.ChangeState(new WaitforActionState());
        Instance.UI = GameManager.CombatUI;
        Instance.UI.Initialize(Instance.playerPokemon.m_PokemonInfo);
        
    }
    public void BuildTurnQueue()
    {
        PokemonComponent fastestPokemon;
        PokemonComponent slowestPokemon;
        

        if (Instance.playerPokemon.m_PokemonInfo.Speed >= Instance.enemyPokemon.m_PokemonInfo.Speed)
        {
            fastestPokemon = Instance.playerPokemon;
            slowestPokemon = Instance.enemyPokemon;
        }
        else
        {
            fastestPokemon = Instance.enemyPokemon;
            slowestPokemon = Instance.playerPokemon;
        }
        // Use the move chosen by the player for the fastest Pokemon
        Instance.turnQueue.Enqueue(new Turn(fastestPokemon, slowestPokemon, Instance.PokemonMove));
        Instance.turnQueue.Enqueue(new Turn(slowestPokemon, fastestPokemon, Instance.PokemonMove));
        Instance.m_UI.SetHealth(Instance.playerPokemon.m_PokemonInfo, Instance.enemyPokemon.m_PokemonInfo);
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
            return move.Power + p_Attacker.Attack;
    }

    public class WaitforActionState : State
    {
        public void StartTurn()
        {

        }
        public override void Enter()
        {
            CombatManager.
            Instance.playerPokemon.Animator.CrossFadeInFixedTime("Idle", 0.2f);
            Instance.enemyPokemon.Animator.CrossFadeInFixedTime("Idle", 0.2f);
            Instance.PokemonMove = null;
            CombatManager.Instance.UI.SetTextBox("Choose an action");
            //Instance.m_UI.DisplayMessage($"Your {Instance.playerPokemon.name} is waiting for instructions...", 20f);
            //Instance.m_TurnQueue.Clear
        }
        public override void Exit()
        {

        }
        public override void FixedUpdate()
        {
            if (IsActionChosen())
            {
                CombatManager.Instance.BuildTurnQueue();
                CombatManager.Instance.PlayNextTurn();
                   
            }
        }
        public override void Update()
        {
            
        }
        public bool IsActionChosen() => CombatManager.Instance.PokemonMove != null;
    }
}
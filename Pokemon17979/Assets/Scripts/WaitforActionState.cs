using UnityEngine;

public class WaitforActionState : State
    {
        public void StartTurn()
        {
       
        }
    public override void Enter()
        {
            
        }
        public override void Exit()
        {
            
        }
        public override void FixedUpdate()
        {
            if (IsActionChosen())
            {
                CombatManager.Instance.BuildTurnQueue();
            }
        }
        public override void Update()
        {
            
        }
        public bool IsActionChosen() => CombatManager.Instance.PokemonMove != null;
    }

public class WaitforActionState : State
    {
        public override void Enter()
        {
            throw new System.NotImplementedException();
        }
        public override void Exit()
        {
            throw new System.NotImplementedException();
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
            throw new System.NotImplementedException();
        }
        public bool IsActionChosen() => CombatManager.Instance.PokemonMove != null;
    }

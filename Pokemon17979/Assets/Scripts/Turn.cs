public class Turn
{
        Pokemoninformation m_Attacker;
        Pokemoninformation m_Defender;

        State m_Attack;
        State m_Defend;

        public Turn(Pokemoninformation p_Attacker, Pokemoninformation p_Defender)
        {
            m_Attacker = p_Attacker;
            m_Defender = p_Defender;
        }

        public void StartTurn()
        {
            CombatManager.Instance.ChangeState(m_Attack);

        }

}

public class AttackState : State
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
        throw new System.NotImplementedException();
    }
    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}

public class DefendState : State
{
    public DefendState(PokemonComponent p_Defender)
    {
    }
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
        throw new System.NotImplementedException();
    }
    public override void Update()
    {
        throw new System.NotImplementedException();
    }
}

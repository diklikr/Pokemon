public class Turn
{
    Pokemoninformation m_Attacker;
    Pokemoninformation m_Receiver;
    PokemonMove m_MoveUsed;

    State m_Attack;
    State m_GetDamaged;

    public Turn(Pokemoninformation p_Attacker, Pokemoninformation p_Defender, PokemonMove p_MoveUsed)
    {
        m_Attacker = p_Attacker;
        m_Receiver = p_Defender;
        m_MoveUsed = p_MoveUsed;

        m_Attack = new AttackState(this);
        m_GetDamaged = new DefendState(this);
    }

    public void StartTurn()
    {
        CombatManager.Instance.ChangeState(m_Attack);

    }

    public class AttackState : State
    {
        Turn m_Turn;

        public AttackState(Turn p_Turn)
        {
            m_Turn = p_Turn;
        }
        public override void Enter()
        {

        }
        public override void Exit()
        {

        }
        public override void FixedUpdate()
        {

        }
        public override void Update()
        {

        }
    }

    public class DefendState : State
    {
        Turn m_Turn;
        public DefendState(Turn p_Turn)
        {
            m_Turn = p_Turn;
        }
        public override void Enter()
        {
            int damage = CombatManager.CalculateDamage(m_Turn.m_MoveUsed, m_Turn.m_Attacker, m_Turn.m_Receiver));
            m_Turn.m_Receiver.GetDamaged(damage);
        }
        public override void Exit()
        {

        }
        public override void FixedUpdate()
        {
            if (m_Turn.m_Receiver.Health <= 0)
            {
                //end combat
            }
            else
            {
                CombatManager.PlayNextTurn;
            }
        }
        public override void Update()
        {

        }
    }
}

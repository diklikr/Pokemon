using UnityEngine;
public class Turn
{
    PokemonComponent m_Attacker;
    PokemonComponent m_Receiver;
    PokemonMove m_MoveUsed;
    Pokemoninformation m_PokemonInfo;

    Turn m_Turn;

    State m_Attack;
    State m_GetDamaged;
    State m_death;

    public Turn(PokemonComponent p_Attacker, PokemonComponent p_Defender, PokemonMove p_MoveUsed)
    {
        m_Attacker = p_Attacker;
        m_Receiver = p_Defender;
        m_MoveUsed = p_MoveUsed;

        m_Attack = new AttackState(this);
        m_GetDamaged = new DefendState(this);
        m_death = new DeathState(this);
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
            
            Debug.Log($"{m_Turn.m_Attacker.m_PokemonInfo.Name} is attacking {m_Turn.m_Receiver.m_PokemonInfo.Name} with {m_Turn.m_MoveUsed.MoveName}");
            // Animate Attacker
            m_Turn.m_Attacker.PlayAnimation("Attack");
        }
        public override void Exit()
        {
            m_Turn.m_Attacker.PlayAnimation("Idle");
        }
        public override void FixedUpdate()
        {
             //CombatManager.Instance.ChangeState(m_Turn.m_GetDamage); // Change to the damage state
           
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
            int damage = CombatManager.CalculateDamage(m_Turn.m_MoveUsed, m_Turn.m_Attacker.m_PokemonInfo, m_Turn.m_Receiver.m_PokemonInfo);
            m_Turn.m_Receiver.m_PokemonInfo.GetDamaged(damage);
            Debug.Log($"{m_Turn.m_Receiver.m_PokemonInfo.Name} received {damage} damage from {m_Turn.m_Attacker.m_PokemonInfo.Name}'s {m_Turn.m_MoveUsed.MoveName}");
            // Animate Receiver
            m_Turn.m_Receiver.PlayAnimation("Pain");
        }
        public override void Exit()
        {
            m_Turn.m_Attacker.PlayAnimation("Idle");
        }
        public override void FixedUpdate()
        {
            if (m_Turn.m_Receiver.m_PokemonInfo.Health <= 0)
            {
                Debug.Log($"{m_Turn.m_Receiver.m_PokemonInfo.Name} has fainted!");
                //end combat
            }
            else
            {
                CombatManager.Instance.PlayNextTurn();
            }
        }
        public override void Update()
        {

        }
    }

    public class DeathState : State
    {
        Turn m_Turn;
        public DeathState(Turn p_Turn)
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
}

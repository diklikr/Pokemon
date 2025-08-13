using UnityEngine;

public class Player : StateMachine
{
    public Rigidbody rigidBody;
    public Animator animator;

    public LayerMask groundLayer;

    private InputSystem_Actions m_InputActions;
    public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);
    }
    
    public Vector3 MoveDirection()
    {
        Vector3 direction = new Vector3(m_InputActions.Player.Move.ReadValue<Vector2>().x, 0, m_InputActions.Player.Move.ReadValue<Vector2>().y);
        return direction.normalized;
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        m_InputActions = new InputSystem_Actions();
        m_InputActions.Enable();

        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        m_CurrentState = new PlayerIdle(this);
    }

    public void Rotate(float p_RotationSpeed)
    {
        float rotation = MoveDirection().x;
        animator.InterpolateFloat("Turn", rotation * 90, p_RotationSpeed);
        if (rotation == 0) { return; }
        Quaternion targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + rotation * 90f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * p_RotationSpeed);
        
    }
    public void Move(float p_Speed)
    {
        float move = MoveDirection().z;
        if (move <= 0.0f) { return; }
        rigidBody.linearVelocity = transform.forward * move * p_Speed;
    }
    
    public void ClamptoFloor()
    {
        if (!IsGrounded()) { return; }
        RaycastHit hit;
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out hit, 2f, groundLayer))
        {
        transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }

    }
}

public class PlayerIdle : State
{
    Player m_Player;

    public PlayerIdle(Player player)
    {
        m_Player = player;
    }
    public override void Enter()
    {
        Debug.Log("player is idle");
        m_Player.animator?.CrossFadeInFixedTime("Idle", 0.2f);
        m_Player.rigidBody.linearVelocity = Vector3.zero;
        m_Player.rigidBody.useGravity = false;
    }

    public override void Update()
    {
        
    }

    public override void FixedUpdate()
    {
        if (m_Player.MoveDirection().x > 0f)
        {
            m_Player.ChangeState(new PlayerMove(m_Player));
        }
        m_Player.Rotate(5f); 
        if(!m_Player.IsGrounded())
        {
            m_Player.ChangeState(new PlayerFalling(m_Player));
        }
    }

    public override void Exit()
    {
        Debug.Log("Player no longer idle");
    }
}

public class PlayerMove : State
{
    Player m_Player;
    public PlayerMove(Player player)
    {
        m_Player = player;
    }

    public override void Enter()
    {
        Debug.Log("Player is now walking");
        m_Player.animator?.CrossFadeInFixedTime("Move", 0.2f);
    }

    public override void Exit()
    {
        Debug.Log("Player entering idle");
    }

    public override void FixedUpdate()
    {
        if (m_Player.MoveDirection().z <= 0f)
        {
            m_Player.ChangeState(new PlayerIdle(m_Player));
        }
        m_Player.Move(2f);
        m_Player.Rotate(5f);
        if (!m_Player.IsGrounded())
        {
            m_Player.ChangeState(new PlayerFalling(m_Player));
        }
    }

    public override void Update()
    {
        
    }
}

public class PlayerFalling : State
{
    Player m_Player;
    public PlayerFalling(Player player)
    {
        m_Player = player;
    }
    public override void Enter()
    {
        m_Player.animator?.CrossFadeInFixedTime("Falling", 0.2f);
        m_Player.rigidBody.useGravity = true;
    }

    public override void Exit()
    {
        m_Player.ClamptoFloor();
    }

    public override void FixedUpdate()
    {
        if (m_Player.IsGrounded())
        {
            m_Player.ChangeState(new PlayerIdle(m_Player));
        }
    } 

    public override void Update()
    {
        
    }
}

public static class MyExtentions
{
    public static void InterpolateFloat(this Animator animator, string parameter, float value, float speed)
    {
        float current = animator.GetFloat(parameter);
        current = Mathf.MoveTowards(current, value, speed * Time.deltaTime);
        animator.SetFloat(parameter, current);
    }

    public static void Interpolate(this float me, float target, float speed)
    {
        me = Mathf.MoveTowards(me, target, speed * Time.deltaTime);
    }
}

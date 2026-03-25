using UnityEngine;

public class Player : StateMachine
{
    public Rigidbody rigidBody;
    public Animator animator;

    public Pokemon poke;

    public LayerMask groundLayer;

    private InputSystem_Actions m_InputActions;

    public bool IsGrounded()
    {
       
        const float originOffset = 0.1f;
        const float rayDistance = 0.25f;
        return Physics.Raycast(transform.position + Vector3.up * originOffset, Vector3.down, rayDistance, groundLayer);
    }

    public Vector3 MoveDirection()
    {
        Vector2 raw = m_InputActions.Player.Move.ReadValue<Vector2>();
        return new Vector3(raw.x, 0f, raw.y);
    }

    void Start()
    {
        m_InputActions = new InputSystem_Actions();
        m_InputActions.Enable();

        rigidBody = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        m_CurrentState = new PlayerIdle(this);
        m_CurrentState?.Enter(); // ensure initial state's Enter is called
    }

    public void Rotate(float p_RotationSpeed)
    {
        float rotation = MoveDirection().x;
        animator.InterpolateFloat("Turn", rotation * 90f, p_RotationSpeed);
        if (Mathf.Approximately(rotation, 0f)) { return; }

        Quaternion targetRotation = Quaternion.Euler(0f, transform.eulerAngles.y + rotation * 90f, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * p_RotationSpeed);
    }

    public void Move(float p_Speed)
    {
        float move = MoveDirection().z;
        if (move <= 0.0f) { return; }
        // Preserve vertical velocity (gravity/jumps)
        Vector3 newVel = transform.forward * move * p_Speed;
        newVel.y = rigidBody.linearVelocity.y;
        rigidBody.linearVelocity = newVel;
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
        m_Player.animator?.CrossFadeInFixedTime("Idle", 0.2f);
        m_Player.rigidBody.linearVelocity = Vector3.zero;
        m_Player.rigidBody.useGravity = false;
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {
        // Check forward input (z) — movement uses z axis
        if (m_Player.MoveDirection().z > 0f)
        {
            m_Player.ChangeState(new PlayerMove(m_Player));
            return;
        }

        m_Player.Rotate(5f);

        if (!m_Player.IsGrounded())
        {
            m_Player.ChangeState(new PlayerFalling(m_Player));
        }
    }

    public override void Exit()
    {
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
        m_Player.animator?.CrossFadeInFixedTime("Move", 0.2f);
        m_Player.rigidBody.useGravity = false;
    }

    public override void Exit()
    {
    }

    public override void FixedUpdate()
    {
        if (m_Player.MoveDirection().z <= 0f)
        {
            m_Player.ChangeState(new PlayerIdle(m_Player));
            return;
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

    // Note: this extension does not change the caller variable because float is a value type.
    public static float Interpolate(this float me, float target, float speed)
    {
        return Mathf.MoveTowards(me, target, speed * Time.deltaTime);
    }
}
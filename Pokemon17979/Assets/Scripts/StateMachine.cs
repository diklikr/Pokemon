using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    protected State m_CurrentState;

    private void Start()
    {
        
    }

    private void Update()
    {
        m_CurrentState?.Update();
    }

    private void FixedUpdate()
    {
        m_CurrentState?.FixedUpdate();
    }
    public void ChangeState(State newState)
    {
        m_CurrentState?.Exit();
        m_CurrentState = newState;
        m_CurrentState.Enter();
    }
}

public abstract class State
{
    public abstract void Enter();
    public abstract void Exit();
    public abstract void Update();
    public abstract void FixedUpdate();
}

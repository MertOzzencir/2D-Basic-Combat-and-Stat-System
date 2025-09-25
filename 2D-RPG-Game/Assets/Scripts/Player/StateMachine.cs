using UnityEngine;

public class StateMachine
{

    public EntityState CurrentState { get; private set; }



    public void Initialize(EntityState setupState)
    {
        CurrentState = setupState;
        CurrentState.Enter();
    }
    public void ChangeState(EntityState toChangedState)
    {
        CurrentState.Exit();
        CurrentState = toChangedState;
        CurrentState.Enter();
        //Debug.Log(CurrentState);
    }

    public void UpdateState()
    {
        CurrentState.Update();
    }
    public void FixedUpdateState()
    {
        CurrentState.FixedUpdate();
    }

}


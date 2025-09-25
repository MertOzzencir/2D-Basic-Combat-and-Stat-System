using System;
using UnityEngine;

public abstract class EntityState
{
    public static event Action<bool> OnDashChange;
    protected StateMachine StateMachine;
    protected Entity Entity;
    protected static bool _airDashControl;

    public EntityState(StateMachine stateMachine, Entity player)
    {
        Entity = player;
        StateMachine = stateMachine;
    }

    public abstract void Enter();
    public abstract void Update();

    public abstract void Exit();
    public virtual void FixedUpdate()
    {
    }
    
    
    public void OnDashChangeFunction()
    {
        OnDashChange?.Invoke(_airDashControl);
    }
      public void ToDashState()
    {
        if (_airDashControl && Timer.DashTimer > 1)
        {
            StateMachine.ChangeState(Entity.DashState);
            Timer.ResetDashTimer();
        }
    }
    public void ToJumpState()
    {
        if (Entity.IsGrounded || StateMachine.CurrentState == Entity.SlideState || Entity.JumpCornerTimer < Entity.JumpCornerThreshold)
            StateMachine.ChangeState(Entity.JumpState);
    }
     public void ToAttackState()
    {
        StateMachine.ChangeState(Entity.AttackState);
    }
}

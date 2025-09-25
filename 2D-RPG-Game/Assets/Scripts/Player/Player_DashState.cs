using UnityEngine;

public class Player_DashState : EntityState
{
    private float Timer;
    public Player_DashState(StateMachine stateMachine, Entity player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        _airDashControl = false;
        OnDashChangeFunction();
        Entity.PlayerAnimator.SetTrigger("canDash");
        Entity.RB.linearVelocity = Vector2.zero;
        Timer = Time.time + 0.25f;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (Entity.IsSlided)
            StateMachine.ChangeState(Entity.SlideState);

        if (Time.time > Timer)
        {
               if (Entity.IsGrounded)
                StateMachine.ChangeState(Entity.IdleState);
            
            StateMachine.ChangeState(Entity.AirState);
        }
         

    }
    public override void FixedUpdate()
    {
        Entity.RB.linearVelocity = Entity.transform.right * Entity.DashPower;
        
    }

    
}

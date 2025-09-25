using UnityEngine;

public class Player_AirState : EntityState
{
    private float groundCheckTimer = 0.1f;
    public Player_AirState(StateMachine stateMachine, Entity player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        Entity.PlayerAnimator.SetBool("canJump", true);
        groundCheckTimer = Time.time + groundCheckTimer;
        InputManager.OnDash += ToDashState;

    }

    public override void Exit()
    {
        Entity.PlayerAnimator.SetBool("canJump", false);
        groundCheckTimer = 0.1f;
        InputManager.OnDash -= ToDashState;

    }
    public override void Update()
    {
        Entity.FlipCheck(Entity.InputVector);
        if (Entity.IsGrounded && Time.time > groundCheckTimer)
        {
            StateMachine.ChangeState(Entity.IdleState);
        }
        if (Entity.IsSlided && Time.time > groundCheckTimer)
            StateMachine.ChangeState(Entity.SlideState);
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
            Entity.RB.linearVelocity = new Vector2(Entity.InputVector.x * Entity.AirSpeed * Entity.Speed, Entity.RB.linearVelocity.y);
    }

   
}

using UnityEngine;

public class Player_WallJumpState : EntityState
{
    private float wallMovementTimer;
    public Player_WallJumpState(StateMachine stateMachine, Entity player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        Entity.PlayerAnimator.SetBool("canJump", true);
        Entity.RB.linearVelocity = (Entity.SlideWallPosition.normal * 5 + Vector2.up * 10).normalized * Entity.JumpPower;
        wallMovementTimer = Time.time + 0.1f;
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (Entity.RB.linearVelocity.y < 0)
            StateMachine.ChangeState(Entity.AirState);
        if (Entity.IsSlided && Time.time > wallMovementTimer + 0.05f)
            StateMachine.ChangeState(Entity.SlideState);
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if(Time.time > wallMovementTimer)
            Entity.RB.linearVelocity = new Vector2(Entity.InputVector.x * Entity.AirSpeed * Entity.Speed, Entity.RB.linearVelocity.y);
    }
}

using System;
using UnityEngine;

public class Player_SlideState : EntityState
{
    private float slideCheckTimer = 0.1f;
    public Player_SlideState(StateMachine stateMachine, Entity player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        _airDashControl = true;
        OnDashChangeFunction();
        Entity.PlayerAnimator.SetBool("canSlide", true);
        slideCheckTimer = Time.time + .1f;
        InputManager.OnJump += ToWallJumpStage;
        Vector3 x = Entity.flipDirection.x > 0.5f ? Vector2.zero : new Vector3(0, 180, 0);
        Entity.FlipCharacter(x);


    }


    public override void Exit()
    {
        Entity.PlayerAnimator.SetBool("canSlide", false);
        slideCheckTimer = 0.1f;
        InputManager.OnJump -= ToWallJumpStage;

    }
    public override void FixedUpdate()
    {
        if (Entity.InputVector.y >= 0)
            Entity.RB.linearVelocity = new Vector2(Entity.InputVector.x, -Entity.SlidePower);
        else
            Entity.RB.linearVelocity = new Vector2(Entity.InputVector.x, -Entity.SlidePower * 3);
    }

    public override void Update()
    {
        if (Entity.IsGrounded && Time.time > slideCheckTimer)
        {
            Debug.Log("InGrounded");
            StateMachine.ChangeState(Entity.IdleState);
        }
        else if (!Entity.IsSlided && Time.time > slideCheckTimer)
        {
            StateMachine.ChangeState(Entity.IdleState);
            Debug.Log("InGrounded");
        }
        
    }
    
    private void ToWallJumpStage()
    {
        StateMachine.ChangeState(Entity.WallJumpState);
    }
}

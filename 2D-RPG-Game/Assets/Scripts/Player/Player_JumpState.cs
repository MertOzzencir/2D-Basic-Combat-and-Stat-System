using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;

public class Player_JumpState : EntityState
{
    public Player_JumpState(StateMachine stateMachine, Entity player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        Entity.PlayerAnimator.SetBool("canJump", true);
        Entity.RB.linearVelocity = new Vector2(Entity.RB.linearVelocity.x, Entity.JumpPower);
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        StateMachine.ChangeState(Entity.AirState);
    }
    public override void FixedUpdate()
    {
        Entity.VerticalAndHorizontalAnimationSet();
    }
  
}

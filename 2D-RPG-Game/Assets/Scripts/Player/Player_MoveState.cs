using System;
using UnityEngine;

public class Player_MoveState : EntityState
{
    public Player_MoveState(StateMachine stateMachine, Entity player) : base(stateMachine, player)
    {
    }
    public override void Enter()
    {
        Entity.EnterMove();
    }
    public override void Exit()
    {
        Entity.ExitMove();   
    }
    public override void Update()
    {
        Entity.UpdateMove();
    }
    public override void FixedUpdate()
    {
        Entity.VerticalAndHorizontalAnimationSet();
        Entity.MoveEntity();
    }
}

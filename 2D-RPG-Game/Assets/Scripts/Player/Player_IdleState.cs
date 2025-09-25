using System;
using UnityEngine;

public class Player_IdleState : EntityState
{
    public Player_IdleState(StateMachine stateMachine, Entity player)
    : base(stateMachine, player)
    {
    }

    public override void Enter()
    {

        _airDashControl = true;
        OnDashChangeFunction();
        Entity.EnterIdle();
    }


    public override void Exit()
    {
        Entity.ExitIdle();
    }
    public override void Update()
    {
        Entity.UpdateIdle();
    }
    public override void FixedUpdate()
    {
        Entity.VerticalAndHorizontalAnimationSet();
    }
    

}

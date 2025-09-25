using System;
using Unity.VisualScripting;
using UnityEngine;

public class Player_AttackState : EntityState
{
    
    public Player_AttackState(StateMachine stateMachine, Entity player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        Entity.EnterAttack();
    }
    public override void Exit()
    {
        Entity.ExitAttack();
    }
    public override void Update()
    {
        Entity.UpdateAttack();
    }
}

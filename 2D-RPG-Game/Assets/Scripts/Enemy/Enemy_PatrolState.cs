using UnityEngine;

public class Enemy_PatrolState : EntityState
{
    public Enemy_PatrolState(StateMachine stateMachine, Entity player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        Entity.EnterPatrol();
    }

    public override void Exit()
    {
        Entity.ExitPatrol();
    }
    public override void Update()
    {
    }

    public override void FixedUpdate()
    {
        Entity.VerticalAndHorizontalAnimationSet();
        Entity.UpdatePatrol();
    }

    
}

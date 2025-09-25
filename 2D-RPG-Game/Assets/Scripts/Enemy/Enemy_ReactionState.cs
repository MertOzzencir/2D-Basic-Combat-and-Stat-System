using UnityEngine;

public class Enemy_ReactionState : EntityState
{
    public Enemy_ReactionState(StateMachine stateMachine, Entity player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        Entity.ReactionEnter();
    }

    public override void Exit()
    {
        Entity.ReactionExit();
    }

    public override void Update()
    {
        Entity.ReactionUpdate();
        Entity.VerticalAndHorizontalAnimationSet();
    }

    
   
}

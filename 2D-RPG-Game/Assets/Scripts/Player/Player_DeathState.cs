using UnityEngine;

public class Player_DeathState : EntityState
{
    public Player_DeathState(StateMachine stateMachine, Entity player) : base(stateMachine, player)
    {
    }

    public override void Enter()
    {
        Entity.EnterDeathState();
    }

    public override void Exit()
    {
        Entity.ExitDeathState();
    }

    public override void Update()
    {
        Entity.UpdateDeathState();
    }
}

using UnityEngine;

public class EnemyDeath : EntityDeath
{

    public override void TakeDamage(float damage, Transform from,bool isCrit)
    {
        base.TakeDamage(damage, from,isCrit);
        if (entity.stats.CurrentHealth.GetValue() <= 0 && !entity._isDead)
        {
            entity.StateMachine.ChangeState(entity.DeathState);
            entityUI.DisableHealthUI();
        }
    }

}

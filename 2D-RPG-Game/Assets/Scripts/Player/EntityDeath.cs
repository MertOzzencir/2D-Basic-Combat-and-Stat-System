using System.Collections;
using UnityEngine;

public class EntityDeath : MonoBehaviour, IDamageable
{
    public Entity entity => GetComponent<Entity>();
    public EntityVFX vfx => GetComponent<EntityVFX>();
    public Entity_WorldUI entityUI => GetComponentInChildren<Entity_WorldUI>();

    public virtual void TakeDamage(float damage, Transform from,bool isCrit)
    {
        vfx.StartDamageEffect(transform.position,isCrit,from);
        if (entity._isDead || from == this)
            return;
        if (DodgeChanceCalculation())
        {
            return;
        }
        AttackPhysicalEffect(from);
        float health = entity.stats.CurrentHealth.GetValue();
        health -= damage *(1 -entity.stats.GetArmorMitigation());
        entityUI.EntityHealthUIUpdate((float)health/ (float)entity.stats.GetMaxHealth());
        entity.stats.CurrentHealth._baseValue = health;
    }

    private void AttackPhysicalEffect(Transform from)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        Vector2 forceDirection = (transform.position - from.position).normalized;
        rb.AddForce(forceDirection * 6f, ForceMode2D.Impulse);
    }

    private bool DodgeChanceCalculation()
    {
        if (RNG.CalculateRNG(entity.stats.GetEvasion()))
        {
            Debug.Log("Dodged");
            return true;
        }

        return false;
    }
}

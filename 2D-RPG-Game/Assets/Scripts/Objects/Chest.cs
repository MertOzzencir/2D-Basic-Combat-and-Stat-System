using UnityEngine;

public class Chest : MonoBehaviour, IDamageable
{
    Animator anim => GetComponentInChildren<Animator>();

    [SerializeField] private Vector2 animationVelocity;
    [SerializeField] private GameObject dropItem;


    private bool _isOpen;
    public void TakeDamage(float damage, Transform from,bool isCrit)
    {
        Player player = from.GetComponent<Player>();
        if ( player == null || _isOpen)
            return;
        Instantiate(dropItem, transform.position + new Vector3(0,.5f), Quaternion.identity);
        GetComponent<Rigidbody2D>().linearVelocity = animationVelocity;
        GetComponent<Rigidbody2D>().angularVelocity= Random.Range(-200f,200f);
        Debug.Log("Attacked to Chest");
        anim.SetBool("canOpen", true);
        _isOpen = true;
        GetComponent<EntityVFX>().StartDamageEffect(transform.position,isCrit,from);

    }
}

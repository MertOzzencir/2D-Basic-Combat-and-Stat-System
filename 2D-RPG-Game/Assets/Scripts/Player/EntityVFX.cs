using System.Collections;
using UnityEngine;

public class EntityVFX : MonoBehaviour
{
    [SerializeField] private Material DamagedMaterial;
    [SerializeField] private GameObject CritHitVFX;
    [SerializeField] private GameObject HitVFX;
    [SerializeField] private Color HitColor;
    private SpriteRenderer SR;
    private Entity entity;
    private Material MainMaterial;
    void Start()
    {
        SR = GetComponentInChildren<SpriteRenderer>();
        MainMaterial = SR.material;
        entity = GetComponent<Entity>();
    }
    public void StartDamageEffect(Vector3 hitPosition,bool isCrit,Transform from)
    {
        StartCoroutine(DamageEffect());
        Vector2 randomOffSet = new Vector2(Random.Range(-.5f, .5f), Random.Range(-.5f, .5f));
        GameObject GeneralHitVFX = isCrit ? CritHitVFX : HitVFX;
        GameObject hitVFX = Instantiate(GeneralHitVFX, (Vector2)hitPosition + randomOffSet, Quaternion.Euler(0,0,Random.Range(0,250)));
        hitVFX.GetComponentInChildren<SpriteRenderer>().color = HitColor;
        hitVFX.transform.eulerAngles = from.right.x >= 0 ? Vector2.zero : new Vector2(0, 180); 
        Destroy(hitVFX, 1f);
    }
     public IEnumerator DamageEffect()
    {

        SR.material = DamagedMaterial;
        yield return new WaitForSeconds(0.1f);
        GetComponent<Rigidbody2D>().linearVelocity = Vector2.zero;
        SR.material = MainMaterial;

    }
}

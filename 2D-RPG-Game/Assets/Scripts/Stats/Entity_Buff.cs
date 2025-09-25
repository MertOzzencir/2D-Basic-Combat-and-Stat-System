using UnityEngine;

public class Entity_Buff : MonoBehaviour
{
    [SerializeField] private float buffValue;
    [SerializeField] private string buffName;
    [SerializeField] private StatType buffType;

   
    void OnTriggerEnter2D(Collider2D collision)
    {
        
        Player player = collision.gameObject.GetComponent<Player>();
        if (player == null)
            return;
        player.stats.GetStatType(buffType)?.Buff(buffName, buffValue);
        Destroy(gameObject, 1f);
    }
}

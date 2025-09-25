using Unity.VisualScripting;
using UnityEngine;

public class ParallaxBackGround : MonoBehaviour
{
    [SerializeField] private ParallaxlLayer[] backgrounds;
    [SerializeField] private Transform CameraNow;

    void Update()
    {
        foreach (ParallaxlLayer a in backgrounds)
        {
            a.background.transform.position = new Vector2(CameraNow.transform.position.x * a.multiplier, a.background.transform.position.y); 

        }
    }
}

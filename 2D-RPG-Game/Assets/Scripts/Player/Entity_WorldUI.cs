using System;
using UnityEngine;
using UnityEngine.UI;

public class Entity_WorldUI : MonoBehaviour
{
    [SerializeField] private Slider _hpSlider;
    void Start()
    {
        Entity entity = GetComponentInParent<Entity>();
        entity.OnFlip += DontRotate;
    }

    private void DontRotate()
    {
        transform.rotation = Quaternion.identity;
    }

    public void EntityHealthUIUpdate(float currentHealthAmount)
    {

        _hpSlider.value = currentHealthAmount;
    }
    public void DisableHealthUI()
    {
        _hpSlider.gameObject.SetActive(false);
    }


}

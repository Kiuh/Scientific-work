using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class DublicateEnergyBorder : MonoBehaviour, IProperty, IValue
{
    [SerializeField]
    float dublicate_value;
    public DublicateEnergyBorder(float dublicate_value)
    {
        this.dublicate_value = dublicate_value;
    }
    public float Value { get => dublicate_value; set => dublicate_value = value; }
    public void SetRandomValue()
    {
        Value = Random.Range(3f, 5f);
    }

    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Herbivory : MonoBehaviour, IProperty, IValue
{
    [SerializeField]
    float herbivory;
    public Herbivory(float herbivory)
    {
        this.herbivory = herbivory;
    }
    public float Value { get => herbivory; set => herbivory = value; }
    public void SetRandomValue()
    {
        Value = Random.Range(0.5f, 1f);
    }
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}

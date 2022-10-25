using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class Herbivory : MonoBehaviour, IProperty, IValue
{
    float herbivory;
    public Herbivory(float herbivory)
    {
        this.herbivory = herbivory;
    }
    public float Value { get => herbivory; set => herbivory = value; }
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}

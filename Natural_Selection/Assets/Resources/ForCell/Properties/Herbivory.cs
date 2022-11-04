using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivory : MonoBehaviour, IProperty, IValue
{
    [SerializeField]
    float herbivory;
    public float Value { get => herbivory; set => herbivory = value; }
    public void SetRandomValue()
    {
        Value = Random.Range(0.5f, 0.8f);
    }
    public void FindNeededPropertys(List<Component> properties)
    {
        return;
    }
}

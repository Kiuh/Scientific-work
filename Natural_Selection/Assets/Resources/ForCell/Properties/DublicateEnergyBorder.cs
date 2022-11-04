using System.Collections.Generic;
using UnityEngine;

public class DublicateEnergyBorder : MonoBehaviour, IProperty, IValue
{
    [SerializeField]
    float dublicate_value;
    public float Value { get => dublicate_value; set => dublicate_value = value; }
    public void SetRandomValue()
    {
        Value = Random.Range(3f, 5f);
    }
    public void FindNeededPropertys(List<Component> properties)
    {
        return;
    }
}

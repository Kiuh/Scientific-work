using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour, IProperty, IValue
{
    [SerializeField]
    float energy;
    public float Value { get => energy; set => energy = value; }
    public void SetRandomValue()
    {
        Value = Random.Range(2f, 3f);
    }
    public void FindNeededPropertys(List<Component> properties)
    {
        return;
    }
}

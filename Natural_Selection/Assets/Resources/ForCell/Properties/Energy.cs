using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Energy : MonoBehaviour, IProperty
{
    float energy;
    public Energy(float energy)
    {
        this.energy = energy;
    }
    public float Value { get => energy; set => energy = value; }
    public void SetRandomValue()
    {
        Value = Random.Range(2f, 3f);
    }
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}

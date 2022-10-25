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
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}

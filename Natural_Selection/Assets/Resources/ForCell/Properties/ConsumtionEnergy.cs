using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ConsumtionEnergy : MonoBehaviour, IProperty, IValue
{
    float energy;
    public ConsumtionEnergy(float energy)
    {
        this.energy = energy;
    }

    public float Value { get => energy; set => energy = value; }

    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}

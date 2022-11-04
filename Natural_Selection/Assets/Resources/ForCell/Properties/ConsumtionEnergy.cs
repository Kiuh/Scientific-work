using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class ConsumtionEnergy : MonoBehaviour, IProperty, IValue
{
    [SerializeField]
    float energy;
    public float Value { get => energy; set => energy = value; }
    public void SetRandomValue()
    {
        Value = Random.Range(0.005f, 0.01f);
    }
    public void FindNeededPropertys(List<Component> properties)
    {
        return;
    }
}

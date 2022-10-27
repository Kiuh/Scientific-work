using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRadius : MonoBehaviour, IProperty, IValue
{
    float radius;
    public VisionRadius(float radius)
    {
        this.radius = radius;
    }
    public float Value { get => radius; set => radius = value; }

    public void SetRandomValue()
    {
        Value = Random.Range(2, 4);
    }

    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}


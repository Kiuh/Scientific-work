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
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}


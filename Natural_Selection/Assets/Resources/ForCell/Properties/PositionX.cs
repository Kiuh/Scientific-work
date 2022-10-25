using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionX : MonoBehaviour, IProperty, IValue
{
    float x;
    public PositionX(float x)
    {
        this.x = x;
    }

    public float Value { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}

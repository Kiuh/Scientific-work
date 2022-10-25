using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentSpeed : MonoBehaviour, IProperty, IValue
{
    float speed;
    public MovmentSpeed(float speed)
    {
        this.speed = speed;
    }
    public float Value { get => speed; set => speed = value; }
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionY : MonoBehaviour, IProperty, IValue
{
    public float Value { 
        get => transform.position.y;
        set => transform.position.Set(transform.position.y, value, transform.position.z);
    }
    public void SetRandomValue()
    {
        return;
    }
    public void FindNeededPropertys(List<Component> properties)
    {
        return;
    }
}

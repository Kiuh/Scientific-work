using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionY : MonoBehaviour, IProperty, IValue
{
    public float Value { 
        get => transform.position.y;
        set => transform.position = new Vector3(transform.position.y, value, transform.position.z);
    }
    public void SetRandomValue()
    {
        return;
    }
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}

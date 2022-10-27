using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PositionX : MonoBehaviour, IProperty, IValue
{
    public float Value
    {
        get => transform.position.x;
        set => transform.position = new Vector3(value, transform.position.y, transform.position.z);
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

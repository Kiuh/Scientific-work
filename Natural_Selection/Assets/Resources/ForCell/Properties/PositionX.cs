using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PositionX : MonoBehaviour, IProperty, IValue
{
    public float Value
    {
        get => transform.position.x;
        set => transform.position.Set(value, transform.position.y, transform.position.z);
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

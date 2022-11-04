using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRadius : MonoBehaviour, IProperty, IValue
{
    [SerializeField]
    float radius;
    public float Value { get => radius; set => radius = value; }

    public void SetRandomValue()
    {
        Value = Random.Range(1, 2.5f);
    }

    public void FindNeededPropertys(List<Component> properties)
    {
        return;
    }
}


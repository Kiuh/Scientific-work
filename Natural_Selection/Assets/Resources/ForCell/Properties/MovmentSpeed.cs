using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovmentSpeed : MonoBehaviour, IProperty, IValue
{
    [SerializeField]
    float speed;
    public float Value { get => speed; set => speed = value; }
    public void SetRandomValue()
    {
        Value = Random.Range(1f, 2f);
    }
    public void FindNeededPropertys(List<Component> properties)
    {
        return;
    }
}

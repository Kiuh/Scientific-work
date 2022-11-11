using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceX : MonoBehaviour, IProperty, IValue
{
    [SerializeField]
    Rigidbody2D m_Rigidbody;
    public float Value
    {
        get => m_Rigidbody.velocity.x;
        set => m_Rigidbody.velocity.Set(value, m_Rigidbody.velocity.y);
    }
    public void SetRandomValue()
    {
        return;
    }
    public void FindNeededPropertys(List<Component> properties)
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
        return;
    }
}

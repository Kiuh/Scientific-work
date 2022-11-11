using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceY : MonoBehaviour, IProperty, IValue
{
    [SerializeField]
    Rigidbody2D m_Rigidbody;
    public void Reset()
    {
        Debug.Log("Hello");
    }
    public float Value
    {
        get => m_Rigidbody.velocity.y;
        set => m_Rigidbody.velocity.Set(m_Rigidbody.velocity.x, value);
    }
    public void SetRandomValue()
    {
        return;
    }
    public void FindNeededPropertys(List<Component> properties)
    {
        Debug.Log("Hello2");
        m_Rigidbody = GetComponent<Rigidbody2D>();
        return;
    }
}

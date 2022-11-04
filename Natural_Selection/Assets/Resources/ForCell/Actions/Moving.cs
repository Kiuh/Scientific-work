using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Moving : MonoBehaviour, IAction
{
    [SerializeField]
    Component movement_speed;
    [SerializeField]
    Rigidbody2D rb = null;
    [SerializeField]
    float maxSpeed = 15f;
    public int queue_number { get; set; }

    public void DoAction(List<float> floats)
    {
        Vector2 direction = new(floats[0], floats[1]);
        direction.Normalize();
        rb.AddForce(floats[2] * (movement_speed as MovmentSpeed).Value * direction, ForceMode2D.Force);
        Vector3 vector3 = floats[2] * (movement_speed as MovmentSpeed).Value * direction;
        Debug.DrawLine(transform.position, transform.position + vector3, Color.red);
    }

    public void FixedUpdate()
    {
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxSpeed);
    }

    public int GetInputCount()
    {
        return 3;
    }

    public void FindNeededPropertys(List<Component> properties)
    {
        movement_speed = properties.Find((x) => x is MovmentSpeed);
        rb = GetComponent<Rigidbody2D>();
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class FoodSmell : MonoBehaviour, IReceptor
{
    [SerializeField]
    Component vision;
    public int queue_number { get; set; }
    public List<float> GetInformation()
    {
        List<Vector2> points = new();
        List<Collider2D> colliders = (vision as IVision).GetObjectsInVision(transform);
        List<float> result = new() { 0, 0, 0 };
        if (colliders.Count == 0)
            return result;
        foreach (Collider2D collider in colliders)
        {
            collider.TryGetComponent(out IFood buffer);
            if (buffer != null)
            {
                points.Add(collider.GetComponent<Transform>().position);
            }
        }
        if(points.Count == 0)
            return result;
        foreach (Vector2 point in points)
        {
            result[0] += point.x;
            result[1] += point.y;
        }
        result[0] /= points.Count;
        result[1] /= points.Count;

        Debug.DrawLine(transform.position, new Vector3(result[0], result[1], 0));

        result[2] = Vector2.Distance(new Vector2(result[0], result[1]), transform.position) / (vision as IVision).GetVisionRadius();

        Vector2 vector2 = new Vector2(result[0] - transform.position.x, result[1] - transform.position.y);
        vector2.Normalize();
        result[0] = vector2.x;
        result[1] = vector2.y;
        return result;
    }

    public int GetOutputCount()
    {
        return 3;
    }

    void IReceptor.FindNeededPropertys(List<Component> properties)
    {
        vision = properties.Find((x) => x is IVision);
    }
}

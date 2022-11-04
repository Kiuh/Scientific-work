using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleVision : MonoBehaviour, IProperty , IVision
{
    [SerializeField]
    Component vision_radius;

    public List<Collider2D> GetObjectsInVision(Transform transform)
    {
        Vector2 position = transform.position;
        float max_radius = GetVisionRadius();
        return Physics2D.OverlapCircleAll(position, max_radius).ToList();
    }

    public float GetVisionRadius()
    {
        return (vision_radius as VisionRadius).Value;
    }

    public void FindNeededPropertys(List<Component> properties)
    {
        vision_radius = properties.Find((x) => x is VisionRadius);
    }
}

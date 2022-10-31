using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CircleVision : MonoBehaviour, IProperty , IVision
{
    [SerializeField]
    List<VisionRadius> vision_radius;

    public List<Collider2D> GetObjectsInVision(Transform transform)
    {
        Vector2 position = transform.position;
        float max_radius = GetVisionRadius();
        return Physics2D.OverlapCircleAll(position, max_radius).ToList();
    }

    public float GetVisionRadius()
    {
        return vision_radius.Select(x => x.Value).Max();
    }

    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        vision_radius = properties.Where(x => x is VisionRadius).Cast<VisionRadius>().ToList();
    }
}

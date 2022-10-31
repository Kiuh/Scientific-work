using System.Collections.Generic;
using UnityEngine;

public interface IVision
{
    public float GetVisionRadius();
    public List<Collider2D> GetObjectsInVision(Transform transform);
}


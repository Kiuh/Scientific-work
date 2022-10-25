using System.Collections.Generic;
using UnityEngine;

internal interface IVision
{
    public float GetVisionRadius();
    public List<Collider2D> GetObjectsInVision(Transform transform);
}


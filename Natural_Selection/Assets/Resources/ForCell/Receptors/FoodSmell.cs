using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FoodSmell : MonoBehaviour, IReceptor
{
    [SerializeField]
    public int queue_number { get; set; }

    public FoodSmell(int queue_number)
    {
        this.queue_number = queue_number;
    }
    [SerializeField]
    IVision vision;
    public List<float> GetInformation()
    {
        List<Vector2> points = new List<Vector2>();
        foreach (Collider2D collider in vision.GetObjectsInVision(transform))
        {
            collider.TryGetComponent(out IFood buffer);
            if (buffer != null)
            {
                points.Add(collider.GetComponent<Transform>().position);
            }
        }
        List<float> result = new List<float>() { 0, 0, 0 };
        foreach (Vector2 point in points)
        {
            result[0] += point.x;
            result[1] += point.y;
        }
        result[0] /= points.Count;
        result[1] /= points.Count;
        result[2] = Vector2.Distance(new Vector2(result[0], result[1]), transform.position)/ vision.GetVisionRadius();
        return result;
    }

    public int GetOutputCount()
    {
        return 3;
    }

    void IReceptor.FindNeededPropertys(List<IProperty> properties)
    {
        vision = properties.Where(x => x is IVision).Cast<IVision>().First();
        queue_number = 0;
    }
}

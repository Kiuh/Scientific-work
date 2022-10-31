using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class Moving : MonoBehaviour, IAction
{
    [SerializeField]
    MovmentSpeed movmentSpeed;
    [SerializeField]
    Rigidbody2D rb;
    [SerializeField]
    public int queue_number { get; set; }

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public Moving(int queue_number)
    {
        this.queue_number = queue_number;
    }

    public void DoAction(List<float> floats)
    {
        Vector2 direction = new Vector2(floats[0], floats[1]);
        direction.Normalize();
        rb.AddForce(floats[2] * movmentSpeed.Value * direction, ForceMode2D.Force);
    }

    public int GetInputCount()
    {
        return 3;
    }

    void IAction.FindNeededPropertys(List<IProperty> properties)
    {
        movmentSpeed = properties.Where(x => x is MovmentSpeed).Cast<MovmentSpeed>().ToList().First();
    }
}

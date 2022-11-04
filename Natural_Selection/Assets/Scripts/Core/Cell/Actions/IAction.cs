using System.Collections.Generic;
using UnityEngine;

public interface IAction
{
    public int queue_number { get; set; }
    public void DoAction(List<float> floats);
    public void FindNeededPropertys(List<Component> properties);
    public int GetInputCount();
}


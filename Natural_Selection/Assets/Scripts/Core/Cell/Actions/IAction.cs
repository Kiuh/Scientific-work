using System.Collections.Generic;

public interface IAction
{
    public int queue_number { get; set; }
    public void DoAction(List<float> floats);
    public void FindNeededPropertys(List<IProperty> properties);
    public int GetInputCount();
}


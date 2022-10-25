using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
internal interface IAction
{
    public int queue_number { get; }
    public void DoAction(List<float> floats);
    public void FindNeededPropertys(List<IProperty> properties);
    public int GetInputCount();
}


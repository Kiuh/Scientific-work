using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal interface IReceptor
{
    public int queue_number { get; }
    public List<float> GetInformation();
    public void FindNeededPropertys(List<IProperty> properties);
    public int GetOutputCount();
}


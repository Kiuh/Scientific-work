using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IReceptor
{
    public int queue_number { get; set; }
    public List<float> GetInformation();
    public void FindNeededPropertys(List<IProperty> properties);
    public int GetOutputCount();
}


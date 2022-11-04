using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface IReceptor
{
    public int queue_number { get; set; }
    public List<float> GetInformation();
    public void FindNeededPropertys(List<Component> properties);
    public int GetOutputCount();
}


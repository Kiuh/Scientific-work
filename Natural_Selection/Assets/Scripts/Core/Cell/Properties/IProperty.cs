using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal interface IProperty
{
    public void FindNeededPropertys(List<IProperty> properties);
}


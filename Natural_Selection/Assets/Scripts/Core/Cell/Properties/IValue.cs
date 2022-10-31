using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IValue
{
    public float Value { get; set; }
    public void SetRandomValue();
}


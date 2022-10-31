using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public interface IReproduction
{
    public void SetBirthDelegate(Action<CreateCellParameters> birthNew);
}


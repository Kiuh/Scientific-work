using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal interface ILifeType
{
    public void CreateNewCells(Action<Component> death, Action<Component> birth);
}


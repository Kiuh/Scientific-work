using System;
using UnityEngine;
interface IDeath
{
    public Action<Component> Death { set; }
}


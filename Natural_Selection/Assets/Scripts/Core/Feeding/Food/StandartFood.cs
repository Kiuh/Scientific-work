using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandartFood : MonoBehaviour, IFood
{
    public float Energy = 1;
    public float GetEnergy()
    {
        return Energy;
    }
}

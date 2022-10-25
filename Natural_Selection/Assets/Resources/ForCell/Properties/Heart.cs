using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Heart : MonoBehaviour, IProperty
{
    ConsumtionEnergy consumtionEnergy;
    Energy energy;
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        consumtionEnergy = properties.Where(x => x is ConsumtionEnergy).Cast<ConsumtionEnergy>().ToList().First();
        energy = properties.Where(x => x is Energy).Cast<Energy>().ToList().First();
    }
    public void FixedUpdate()
    {
        energy.Value -= consumtionEnergy.Value;
    }
}

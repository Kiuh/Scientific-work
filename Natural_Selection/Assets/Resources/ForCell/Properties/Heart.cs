using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Heart : MonoBehaviour, IProperty
{
    [SerializeField]
    ConsumtionEnergy consumtionEnergy;
    [SerializeField]
    Energy energy;
    [SerializeField]
    Cell cell;
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        consumtionEnergy = properties.Where(x => x is ConsumtionEnergy).Cast<ConsumtionEnergy>().ToList().First();
        energy = properties.Where(x => x is Energy).Cast<Energy>().ToList().First();
        cell = properties.Where(x => x is Cell).Cast<Cell>().ToList().First();
    }
    public void FixedUpdate()
    {
        if (!cell.cell_created)
            return;
        energy.Value -= consumtionEnergy.Value;
    }
}

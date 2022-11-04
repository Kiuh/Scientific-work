using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour, IProperty
{
    [SerializeField]
    Component consumtion_energy;
    [SerializeField]
    Component energy;
    [SerializeField]
    Component cell;
    public void FindNeededPropertys(List<Component> properties)
    {
        consumtion_energy = properties.Find((x) => x is ConsumtionEnergy);
        energy = properties.Find((x) => x is Energy);
        cell = properties.Find((x) => x is Cell);
    }
    public void FixedUpdate()
    {
        if (!(cell as Cell).IsCellCreated || cell == null)
            return;
        (energy as Energy).Value -= (consumtion_energy as ConsumtionEnergy).Value;
    }
}

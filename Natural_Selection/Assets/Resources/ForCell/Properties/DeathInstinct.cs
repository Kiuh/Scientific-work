using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeathInstinct : MonoBehaviour, IProperty, IDeath
{
    [SerializeField]
    Component cell;
    [SerializeField]
    Component death_border;
    [SerializeField]
    Component energy;
    [SerializeField]
    Action<Component> death;

    public Action<Component> Death { set => death = value; }

    public void FindNeededPropertys(List<Component> properties)
    {
        cell = properties.Find((x) => x is Cell);
        death_border = properties.Find((x) => x is EnergyDeathBorder);
        energy = properties.Find((x) => x is Energy);
    }
    public void FixedUpdate()
    {
        if ((energy as Energy).Value <= (death_border as EnergyDeathBorder).Value)
        {
            death(cell);
            Destroy(gameObject);
        }
    }
}

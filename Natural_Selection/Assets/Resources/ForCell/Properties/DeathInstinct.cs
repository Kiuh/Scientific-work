using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DeathInstinct : MonoBehaviour, IProperty
{
    [SerializeField]
    EnergyDeathBorder deathBorder;
    [SerializeField]
    Energy energy;
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        deathBorder = properties.Where(x => x is EnergyDeathBorder).Cast<EnergyDeathBorder>().First();
        energy = properties.Where(x => x is Energy).Cast<Energy>().First();
    }
    public void FixedUpdate()
    {
        if (energy.Value <= deathBorder.Value)
        {
            Destroy(gameObject);
        }
    }
}

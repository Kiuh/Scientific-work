using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class EnergyDeathBorder : MonoBehaviour, IProperty, IValue
{
    float death_value;
    public EnergyDeathBorder(float death_value)
    {
        this.death_value = death_value;
    }
    public float Value { get => death_value; set => death_value = value; }
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }
}

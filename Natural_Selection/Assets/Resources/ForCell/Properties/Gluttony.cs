using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gluttony : MonoBehaviour, IProperty
{
    Energy energy;
    Herbivory herbivory;
    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        energy = properties.Where(x => x is Energy).Cast<Energy>().First();
        herbivory = properties.Where(x => x is Herbivory).Cast<Herbivory>().First();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.TryGetComponent(out IFood food);
        if (food != null)
        {
            energy.Value += food.GetEnergy() * herbivory.Value;
            Destroy(collision.gameObject);
        }
    }
}

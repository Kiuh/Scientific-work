using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gluttony : MonoBehaviour, IProperty
{
    [SerializeField]
    Component energy;
    [SerializeField]
    Component herbivory;
    [SerializeField]
    Component cell;
    public void FindNeededPropertys(List<Component> properties)
    {
        energy = properties.Find((x) => x is Energy);
        herbivory = properties.Find((x) => x is Herbivory);
        cell = properties.Find((x) => x is Cell);
    }
    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!(cell as Cell).IsCellCreated || cell == null)
            return;
        List<Component> components = collision.gameObject.GetComponents<Component>().ToList();
        List<Component> food = components.Where(x => x is IFood).ToList();
        if (food.Count != 0)
        {
            foreach (var item in food)
            {
                (energy as Energy).Value += (item as IFood).GetEnergy() * (herbivory as Herbivory).Value;
            }
            Destroy(collision.gameObject);
        }
    }
}

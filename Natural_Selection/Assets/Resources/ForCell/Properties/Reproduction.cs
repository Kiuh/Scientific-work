using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Reproduction : MonoBehaviour, IProperty, IReproduction
{
    DublicateEnergyBorder dublicateEnergy;
    Energy energy;

    CreateCellParameters cellParameters;
    Cell.BirthNew birthNew;

    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        dublicateEnergy = properties.Where(x => x is DublicateEnergyBorder).Cast<DublicateEnergyBorder>().First();
        energy = properties.Where(x => x is Energy).Cast<Energy>().First();

        PositionX positionX = properties.Where(x => x is PositionX).Cast<PositionX>().First();
        PositionY positionY = properties.Where(x => x is PositionY).Cast<PositionY>().First();
        Cell cell = properties.Where(x => x is Cell).Cast<Cell>().First();
        
        List<ModulesData> modulesDatas = new List<ModulesData>();
        List<Component> components = gameObject.GetComponents<Component>().Where(x => x is IProperty).ToList();
        foreach (var component in components)
        {
            modulesDatas.Add(new ModulesData(component.name, component is IValue ? (component as IValue).Value: null));
        }

        cellParameters = new CreateCellParameters(
            new Vector2(positionX.Value, positionY.Value),
            CellCreateMode.FullCopy,
            cell.Intellect,
            modulesDatas
        );
    }
    public void FixedUpdate()
    {
        if (energy.Value >= dublicateEnergy.Value)
        {
            birthNew(cellParameters);
        }
    }

    public void SetBirthDelegate(Cell.BirthNew birthNew)
    {
        this.birthNew = birthNew;
    }
}

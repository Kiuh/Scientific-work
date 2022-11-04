using System.Collections.Generic;
using UnityEngine;

public class Reproduction : MonoBehaviour, IProperty
{
    [SerializeField]
    Component dublicateEnergy;
    [SerializeField]
    Component energy;
    [SerializeField]
    Component positionX;
    [SerializeField]
    Component positionY;
    [SerializeField]
    Component cell;

    public void FindNeededPropertys(List<Component> properties)
    {
        dublicateEnergy = properties.Find((x) => x is DublicateEnergyBorder);
        energy = properties.Find((x) => x is Energy);

        positionX = properties.Find((x) => x is PositionX);
        positionY = properties.Find((x) => x is PositionY);
        cell = properties.Find((x) => x is Cell);
    }
    public void FixedUpdate()
    {
        if(!(cell as Cell).IsCellCreated || cell == null)
            return;
        if ((energy as Energy).Value >= (dublicateEnergy as DublicateEnergyBorder).Value)
        {
            (energy as Energy).Value /= 2;
            //CellCreator.CreateChild(new Vector2((positionX as PositionX).Value, (positionY as PositionY).Value), cell);
            CellCreator.CreateStandartCell(new Vector2((positionX as PositionX).Value, (positionY as PositionY).Value), (cell as Cell).Death, (cell as Cell).Birth);
        }
    }
}

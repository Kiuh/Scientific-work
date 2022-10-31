using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public enum CellCreateMode
{
    Random = 0,
    FullCopy
}
public class CellCreator
{
    public Cell CreateCell(CreateCellParameters parameters)
    {
        return parameters.mode switch
        {
            CellCreateMode.Random => Random_Mode(parameters),
            CellCreateMode.FullCopy => FullCopy_Mode(parameters),
            _ => null,
        };
    }
    private Cell Random_Mode(CreateCellParameters parameters)
    {
        // Набрать самому
        List<Type> types_empty = new() { typeof(Moving), typeof(FoodSmell), typeof(CircleVision),
            typeof(DeathInstinct), typeof(Heart), typeof(Reproduction)};
        /*
        List<Type> types_empty = parameters.parameters
            .Where(x => x.value == null)
            .Select(x => Type.GetType(x.name))
            .ToList();
        */
        List<Type> types_with_value = new() { typeof(ConsumtionEnergy), typeof(DublicateEnergyBorder), typeof(Energy),
            typeof(EnergyDeathBorder), typeof(Herbivory), typeof(MovmentSpeed), typeof(PositionX),
            typeof(PositionY), typeof(VisionRadius)};
        /*
        List<Type> types_with_value = parameters.parameters
            .Where(x => x.value != null)
            .Select(x => Type.GetType(x.name))
            .ToList();
        */
        List <Type> all_types = new List<Type>();
        all_types.AddRange(types_empty);
        all_types.AddRange(types_with_value);

        GameObject cell_go = Resources.Load("CellPrefab/Cell") as GameObject;
        foreach (var item in all_types)
        {
            cell_go.AddComponent(item);
        }
        for (int i = 0; i < types_with_value.Count; i++)
        {
            IValue value_class = cell_go.GetComponent(types_with_value[i]) as IValue;
            value_class.SetRandomValue();
        }
        int k = 0;
        foreach(var item in cell_go.GetComponents<Component>().ToList().Where(x => x is IAction).Cast<IAction>())
        {
            item.queue_number = k;
            k++;
        }
        k = 0;
        foreach (var item in cell_go.GetComponents<Component>().ToList().Where(x => x is IReceptor).Cast<IReceptor>())
        {
            item.queue_number = k;
            k++;
        }
        Cell cell = cell_go.GetComponent(typeof(Cell)) as Cell;
        cell.InitializeCell(15, 15);
        GameObject.Instantiate(cell_go, parameters.position, new Quaternion());
        return cell;
    }
    private Cell FullCopy_Mode(CreateCellParameters parameters)
    {
        List<Type> types_empty = parameters.parameters
            .Where(x => x.value == null)
            .Select(x => Type.GetType(x.name))
            .ToList();

        List<Type> types_with_value = parameters.parameters
            .Where(x => x.value != null)
            .Select(x => Type.GetType(x.name))
            .ToList();

        List<float?> values = parameters.parameters
            .Where(x => x.value != null)
            .Select(x => x.value)
            .ToList();

        List<Type> all_types = new List<Type>();
        all_types.AddRange(types_empty);
        all_types.AddRange(types_with_value);

        GameObject cell_go = Resources.Load("CellPrefab/Cell") as GameObject;
        foreach (var item in all_types)
        {
            cell_go.AddComponent(item);
        }
        for (int i = 0; i < types_with_value.Count; i++)
        {
            IValue value_class = cell_go.GetComponent(types_with_value[i]) as IValue;
            value_class.Value = (float)values[i];
        }
        Cell cell = (cell_go.GetComponent(typeof(Cell)) as Cell);
        cell.InitializeCell(parameters.intellect);
        GameObject.Instantiate(cell_go, parameters.position, new Quaternion());
        return cell;
    }
}
public class CreateCellParameters
{
    public Vector2 position;
    public CellCreateMode mode;
    public Intellect intellect;
    public List<ModulesData> parameters;
    public CreateCellParameters(Vector2 position, CellCreateMode mode, Intellect intellect, List<ModulesData> parameters)
    {
        this.position = position;
        this.mode = mode;
        this.intellect = intellect;
        this.parameters = parameters;
    }
}
public class ModulesData
{
    public string name;
    public float? value;
    public ModulesData(string name, float? value)
    {
        this.name = name;
        this.value = value;
    }
}
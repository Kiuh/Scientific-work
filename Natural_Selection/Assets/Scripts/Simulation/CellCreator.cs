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
        List<Type> types_empty = parameters.parameters
            .Where(x => x.value == null)
            .Select(x => Type.GetType(x.name))
            .ToList();

        List<Type> types_with_value = parameters.parameters
            .Where(x => x.value != null)
            .Select(x => Type.GetType(x.name))
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
            value_class.SetRandomValue();
        }
        Cell cell = (cell_go.GetComponent(typeof(Cell)) as Cell);
        cell.InitializeCell(parameters.intellect.AllNeuronsCount, parameters.intellect.AllGensCount);
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
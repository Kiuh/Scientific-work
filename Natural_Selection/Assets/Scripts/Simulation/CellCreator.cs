using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class CellCreator
{
    public static Component CreateStandartCell(Vector2 position, Action<Component> death, Action<Component> birth)
    {
        List<Type> types_empty = new() { typeof(Moving), typeof(FoodSmell), typeof(CircleVision),
            typeof(DeathInstinct), typeof(Heart), typeof(Reproduction), typeof(Gluttony), typeof(OwnID), typeof(ParentID)
        };

        List<Type> types_with_value = new() { typeof(ConsumtionEnergy), typeof(DublicateEnergyBorder), typeof(Energy),
            typeof(EnergyDeathBorder), typeof(Herbivory), typeof(MovmentSpeed), typeof(PositionX),
            typeof(PositionY), typeof(VisionRadius), typeof(ForceX), typeof(ForceY)
        };

        List<Type> all_types = new();
        all_types.AddRange(types_empty);
        all_types.AddRange(types_with_value);

        GameObject cell_go = GameObject.Instantiate(Resources.Load("CellPrefab/Cell") as GameObject, position, new Quaternion());

        foreach (var item in all_types)
            cell_go.AddComponent(item);

        for (int i = 0; i < types_with_value.Count; i++)
        {
            IValue value_class = cell_go.GetComponent(types_with_value[i]) as IValue;
            value_class.SetRandomValue();
        }
        int k = 0; int input_N = 0; int output_N = 0;
        foreach (var item in cell_go.GetComponents<Component>().ToList().FindAll((x) => x is IAction))
        {
            (item as IAction).queue_number = k;
            output_N += (item as IAction).GetInputCount();
            k++;
        }
        k = 0;
        foreach (var item in cell_go.GetComponents<Component>().ToList().FindAll((x) => x is IReceptor))
        {
            (item as IReceptor).queue_number = k;
            input_N += (item as IReceptor).GetOutputCount();
            k++;
        }
        (cell_go.GetComponents<Component>().ToList().Find((x) => x is ParentID) as ParentID).ID = -1;
        Component cell = cell_go.GetComponents<Component>().Where(x => x is Cell).First();
        (cell as Cell).InitializeCell(new Intellect(input_N, 3, output_N, 6), death, birth, all_types.Select(x => x.Name).ToList());
        return cell;
    }
    public static Component CreateChild(Vector2 position, Component cell)
    {
        List<Type> all_types = (cell as Cell).AllModules.Select(x => Type.GetType(x)).ToList();

        List<Type> types_with_value = all_types.Where(x => x.GetInterface(typeof(IValue).Name) != null).ToList();

        List<float?> values = new();
        types_with_value.ForEach(x => values.Add(((cell as Cell).Properties.Find((y) => y.GetType() == x) as IValue).Value));

        GameObject cell_go = GameObject.Instantiate(Resources.Load("CellPrefab/Cell") as GameObject, position, new Quaternion());
        foreach (var item in all_types)
        {
            cell_go.AddComponent(item);
        }
        for (int i = 0; i < types_with_value.Count; i++)
        {
            IValue value_class = cell_go.GetComponent(types_with_value[i]) as IValue;
            value_class.Value = (float)values[i];
        }
        Component n_cell = cell_go.GetComponents<Component>().ToList().Find((x) => x is Cell);
        (cell_go.GetComponents<Component>().ToList().Find((x) => x is ParentID) as ParentID).ID = (cell as Cell).ID;
        (n_cell as Cell).InitializeCell(new Intellect((cell as Cell).Intellect), (cell as Cell).Death, (cell as Cell).Birth, all_types.Select(x => x.Name).ToList());
        return n_cell;
    }
    public static Component CreateCellFromData(ServerSpeaker.CellData cellData, Action<Component> death, Action<Component> birth, Action<Component> load)
    {
        //List<Type> all_types = (cellData as Cell).AllModules.Select(x => Type.GetType(x)).ToList();
        List<Type> all_types = cellData.modules.Select(x => Type.GetType(x.name)).ToList();

        List <Type> types_with_value = all_types.Where(x => x.GetInterface(typeof(IValue).Name) != null).ToList();

        List<float?> values = new();
        //types_with_value.ForEach(x => values.Add(((cell as Cell).Properties.Find((y) => y.GetType() == x) as IValue).Value));
        types_with_value.ForEach(x => values.Add(cellData.modules.Find((y) => y.name == x.Name).value));

        Vector2 pos = new((float)cellData.modules.Find((y) => y.name == typeof(PositionX).Name).value, (float)cellData.modules.Find((y) => y.name == typeof(PositionY).Name).value);
        GameObject cell_go = GameObject.Instantiate(Resources.Load("CellPrefab/Cell") as GameObject, pos, new Quaternion());
        foreach (var item in all_types)
        {
            cell_go.AddComponent(item);
        }
        for (int i = 0; i < types_with_value.Count; i++)
        {
            IValue value_class = cell_go.GetComponent(types_with_value[i]) as IValue;
            value_class.Value = (float)values[i];
        }
        Component n_cell = cell_go.GetComponents<Component>().ToList().Find((x) => x is Cell);
        (cell_go.GetComponents<Component>().ToList().Find((x) => x is ParentID) as ParentID).ID = cellData.parent_id;
        (cell_go.GetComponents<Component>().ToList().Find((x) => x is OwnID) as OwnID).ID = cellData.own_id;
        Intellect intellect = new Intellect(
            cellData.intellect.neurons.Select(x => new Neuron(x.bias)).ToList(),
            cellData.intellect.gens.Select(x => new Synaps(x.el_neur_number, x.fin_neur_number, x.weight)).ToList(),
            cellData.intellect.input_neurons_count,
            cellData.intellect.output_neurons_count
        );
        (n_cell as Cell).InitializeCell_FromData(intellect, death, birth, load, all_types.Select(x => x.Name).ToList());
        return n_cell;
    }
}
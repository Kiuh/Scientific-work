using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell : MonoBehaviour, IProperty
{
    [SerializeField]
    List<Component> receptors;
    [SerializeField]
    List<Component> properties;
    [SerializeField]
    List<Component> actions;
    [SerializeField]
    List<string> all_modules;
    [SerializeField]
    Intellect intellect;
    [SerializeField]
    bool cell_created = false;
    [SerializeField]
    Action<Component> death;
    [SerializeField]
    Action<Component> birth;

    public void InitializeCell(Intellect intellect, Action<Component> death, Action<Component> birth, List<string> all_modules)
    {
        receptors = gameObject.GetComponents<Component>().ToList().Where(x => x is IReceptor).ToList();
        properties = gameObject.GetComponents<Component>().ToList().Where(x => x is IProperty).ToList();
        actions = gameObject.GetComponents<Component>().ToList().Where(x => x is IAction).ToList();

        receptors.ForEach(x => (x as IReceptor).FindNeededPropertys(properties));
        properties.ForEach(x => (x as IProperty).FindNeededPropertys(properties));
        actions.ForEach(x => (x as IAction).FindNeededPropertys(properties));

        this.intellect = intellect;
        this.death = death;
        this.birth = birth;
        this.all_modules = all_modules;

        properties.Where(x => x is IDeath).ToList().ForEach(x => (x as IDeath).Death = death);

        birth(gameObject.GetComponents<Component>().ToList().Find((x) => x is Cell));

        cell_created = true;
    }
    public void InitializeCell_FromData(Intellect intellect, Action<Component> death, Action<Component> birth, Action<Component> load, List<string> all_modules)
    {
        receptors = gameObject.GetComponents<Component>().ToList().Where(x => x is IReceptor).ToList();
        properties = gameObject.GetComponents<Component>().ToList().Where(x => x is IProperty).ToList();
        actions = gameObject.GetComponents<Component>().ToList().Where(x => x is IAction).ToList();

        receptors.ForEach(x => (x as IReceptor).FindNeededPropertys(properties));
        properties.ForEach(x => (x as IProperty).FindNeededPropertys(properties));
        actions.ForEach(x => (x as IAction).FindNeededPropertys(properties));

        this.intellect = intellect;
        this.death = death;
        this.birth = birth;
        this.all_modules = all_modules;

        properties.Where(x => x is IDeath).ToList().ForEach(x => (x as IDeath).Death = death);

        load(gameObject.GetComponents<Component>().ToList().Find((x) => x is Cell));

        cell_created = true;
    }
    void FixedUpdate()
    {
        if (!cell_created)
            return;

        List<float> info = new();

        foreach (var item in receptors)
            info.AddRange((item as IReceptor).GetInformation());

        //Debug.Log("Input info:  (" + info.Select(x => Convert.ToString(x)).Aggregate((x,y) => x + ")  (" + y) + ")");

        List<float> great_info = intellect.Think(info);

        //Debug.Log("Output info:  (" + great_info.Select(x => Convert.ToString(x)).Aggregate((x, y) => x + ")  (" + y ) + ")");

        foreach (var item in actions)
            (item as IAction).DoAction(great_info);
    }
    public void FindNeededPropertys(List<Component> properties)
    {
        return;
    }
    public List<Component> Receptors { get => receptors; }
    public List<Component> Properties { get => properties; }
    public List<Component> Actions { get => actions; }
    public Action<Component> Death { get => death; }
    public Action<Component> Birth { get => birth; }
    public Intellect Intellect { get => intellect; }
    public bool IsCellCreated { get => cell_created; }
    public List<string> AllModules { get => all_modules; }
    public long ID {
        get 
        {
            return (properties.Find((x) => x is OwnID) as OwnID).ID; 
        }
        set
        {
            (properties.Find((x) => x is OwnID) as OwnID).ID = value;
        }
    }
    public ServerSpeaker.CellData GetCellData()
    {
        Component own_id = properties.Find((x) => x is OwnID);
        Component parent_id = properties.Find((x) => x is ParentID);

        List<ServerSpeaker.ModuleData> list = new();
        foreach (var item in gameObject.GetComponents<Component>().Where(x => x is IReceptor || x is IAction || x is IProperty))
        {
            list.Add(new ServerSpeaker.ModuleData(item.GetType().Name, item is IValue ? (item as IValue).Value : null));
        }

        ServerSpeaker.IntellectData intellectData = new(
            intellect.neurons.Count,
            intellect.synapses.Count,
            intellect.input_neurons,
            intellect.output_neurons,
            intellect.neurons.Select(x => new ServerSpeaker.NeuronData(x.bias)).ToList(),
            intellect.synapses.Select(x => new ServerSpeaker.SynapsData(x.start_neuron_number, x.finish_neuron_number, x.weight)).ToList()
        );

        return new ServerSpeaker.CellData((parent_id as ParentID).ID, (own_id as OwnID).ID, list, intellectData);
    }
    public List<ServerSpeaker.ModuleData> GetPositionAndForceData()
    {
        List<ServerSpeaker.ModuleData> list = new();

        PositionX posx = gameObject.GetComponents<Component>().ToList().Find((x) => x is PositionX) as PositionX;
        list.Add(new ServerSpeaker.ModuleData(posx.GetType().Name, (posx as IValue).Value));

        PositionY posy = gameObject.GetComponents<Component>().ToList().Find((x) => x is PositionY) as PositionY;
        list.Add(new ServerSpeaker.ModuleData(posy.GetType().Name, (posy as IValue).Value));

        ForceX forcex = gameObject.GetComponents<Component>().ToList().Find((x) => x is ForceX) as ForceX;
        list.Add(new ServerSpeaker.ModuleData(forcex.GetType().Name, (forcex as IValue).Value));

        ForceY forcey = gameObject.GetComponents<Component>().ToList().Find((x) => x is ForceY) as ForceY;
        list.Add(new ServerSpeaker.ModuleData(forcey.GetType().Name, (forcey as IValue).Value));

        return list;
    }
}

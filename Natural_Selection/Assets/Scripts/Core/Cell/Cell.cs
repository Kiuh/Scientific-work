using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell : MonoBehaviour, IProperty
{
    [SerializeField]
    List<IReceptor> receptors;
    [SerializeField]
    List<IProperty> properties;
    [SerializeField]
    List<IAction> actions;
    [SerializeField]
    Intellect intellect;

    public Action<CreateCellParameters> birth_trigger;

    public bool cell_created = false;

    void SetComponents()
    {
        receptors = gameObject.GetComponents<IReceptor>().ToList();
        properties = gameObject.GetComponents<IProperty>().ToList();
        actions = gameObject.GetComponents<IAction>().ToList();

        receptors.ForEach(receptor => receptor.FindNeededPropertys(properties));
        properties.ForEach(receptor => receptor.FindNeededPropertys(properties));
        actions.ForEach(receptor => receptor.FindNeededPropertys(properties));
    }
    public void InitializeCell(Intellect intellect)
    {
        SetComponents();
        this.intellect = new Intellect(intellect);
        SetOrderAndAnother();

    }
    public void InitializeCell(int neurons, int gens)
    {
        SetComponents();
        int input_neurons = receptors.Select(x => x.GetOutputCount()).Sum();
        int output_neurons = actions.Select(x => x.GetInputCount()).Sum();
        int free_neurons = neurons - input_neurons - output_neurons;
        intellect = new Intellect(input_neurons, free_neurons, output_neurons, gens);
        SetOrderAndAnother();
    }
    void SetOrderAndAnother()
    {
        // Maybe wrong
        //receptors.Sort((x1, x2) => x1.queue_number - x2.queue_number);
        //actions.Sort((x1, x2) => x1.queue_number - x2.queue_number);

        receptors.Where(x => x is IReproduction)
            .Cast<IReproduction>().ToList().ForEach(x => x.SetBirthDelegate(birth_trigger));

        cell_created = true;
    }

    public void LiveMoment()
    {
        if (!cell_created)
            return;

        List<float> info = new List<float>();
        foreach (var item in receptors)
            info.AddRange(item.GetInformation());
        Debug.Log(info);
        List<float> great_info = intellect.Think(info);
        Debug.Log(great_info);
        foreach (var item in actions)
            item.DoAction(great_info);
    }

    public void FixedUpdate()
    {
        LiveMoment();
    }

    void IProperty.FindNeededPropertys(List<IProperty> properties)
    {
        return;
    }

    public Intellect Intellect { get => intellect; }

    public List<ServerSpeaker.ModuleData> modulesData {
        get {
            List<ServerSpeaker.ModuleData> list = new();
            foreach(var item in gameObject.GetComponents<Component>().Where(x => x is IReceptor || x is IAction || x is IProperty)){
                list.Add(new ServerSpeaker.ModuleData(item.GetType().Name, item is IValue ? (item as IValue).Value : null));
            }
            return list;
        } 
    }

    public List<ServerSpeaker.ModuleData> GetPositionsData()
    {
        List<ServerSpeaker.ModuleData> list = new();

        PositionX posx = gameObject.GetComponents<Component>().ToList().Find((x) => x is PositionX) as PositionX;
        list.Add(new ServerSpeaker.ModuleData(posx.GetType().Name, (posx as IValue).Value));

        PositionY posy = gameObject.GetComponents<Component>().ToList().Find((x) => x is PositionY) as PositionY;
        list.Add(new ServerSpeaker.ModuleData(posy.GetType().Name, (posy as IValue).Value));

        return list;
    }
}

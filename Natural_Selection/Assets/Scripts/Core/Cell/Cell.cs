using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell : MonoBehaviour, IProperty
{
    public delegate void BirthNew(CreateCellParameters cellParameters);

    List<IReceptor> receptors = new List<IReceptor>();
    List<IProperty> properties = new List<IProperty>();
    List<IAction> actions = new List<IAction>();

    Intellect intellect;
    public BirthNew birth_trigger;

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
        receptors.Sort((x1, x2) => x1.queue_number - x2.queue_number);
        actions.Sort((x1, x2) => x1.queue_number - x2.queue_number);

        receptors.Where(x => x is IReproduction)
            .Cast<IReproduction>().ToList().ForEach(x => x.SetBirthDelegate(birth_trigger));
    }

    public void LiveMoment()
    {
        List<float> info = new List<float>();
        foreach (var item in receptors)
            info.AddRange(item.GetInformation());
        
        List<float> great_info = intellect.Think(info);

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
}

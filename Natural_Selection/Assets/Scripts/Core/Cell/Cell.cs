using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Cell : MonoBehaviour
{
    List<IReceptor> receptors = new List<IReceptor>();
    List<IProperty> properties = new List<IProperty>();
    List<IAction> actions = new List<IAction>();

    Intellect intellect;

    public Cell(int neurons, int gens)
    {
        InitializeCell(neurons, gens);
    }

    public void InitializeCell(int neurons, int gens)
    {
        receptors = gameObject.GetComponents<IReceptor>().ToList();
        properties = gameObject.GetComponents<IProperty>().ToList();
        actions = gameObject.GetComponents<IAction>().ToList();

        receptors.ForEach(receptor => receptor.FindNeededPropertys(properties));
        properties.ForEach(receptor => receptor.FindNeededPropertys(properties));
        actions.ForEach(receptor => receptor.FindNeededPropertys(properties));

        int input_neurons = receptors.Select(x => x.GetOutputCount()).Sum();
        int output_neurons = actions.Select(x => x.GetInputCount()).Sum();
        int free_neurons = neurons - input_neurons - output_neurons;

        intellect = new Intellect(input_neurons, free_neurons, output_neurons, gens);

        // Maybe wrong
        receptors.Sort((x1, x2) => x1.queue_number - x2.queue_number);
        actions.Sort((x1, x2) => x1.queue_number - x2.queue_number);
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
}

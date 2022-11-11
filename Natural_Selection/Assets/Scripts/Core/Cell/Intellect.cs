using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
[SerializeField]
public class Intellect
{
    public List<Neuron> neurons = new();
    public List<Synaps> synapses = new();

    public int input_neurons = 0;
    public int output_neurons = 0;
    public List<int> calculate_queue = new();
    #region Constants
    // Gen borders
    const float LeftGenBorder = -1f;
    const float RightGenBorder = 1f;
    // Bias borders
    const float LeftBiasBorder = -0.05f;
    const float RightBiasBorder = 0.05f;
    #endregion
    public Intellect(int input_neurons, int inner_neurons, int output_neurons, int synapses_count)
    {
        this.input_neurons = input_neurons;
        this.output_neurons = output_neurons;
        int all_neurons = input_neurons + inner_neurons + output_neurons;

        // Create neurons and synapses
        for (int i = 0; i < all_neurons; i++)
            neurons.Add(new Neuron((float)(new System.Random()).NextDouble() * (RightBiasBorder - LeftBiasBorder) + LeftBiasBorder));
        for (int i = 0; i < synapses_count; i++)
            synapses.Add(new Synaps((float)(new System.Random()).NextDouble() * (LeftGenBorder - RightGenBorder) + LeftGenBorder));

        // Bind synapses to input neurons (for each input neuron)
        for (int i = 0; i < input_neurons; i++)
        {
            synapses[i].start_neuron_number = i;
            synapses[i].finish_neuron_number = (new System.Random()).Next(input_neurons, all_neurons);
        }

        // Bind synapses to output neurons(for each output neuron)
        for (int i = input_neurons; i < input_neurons + output_neurons; i++)
        {
            do
            {
                synapses[i].start_neuron_number = (new System.Random()).Next(0, all_neurons);
                synapses[i].finish_neuron_number = i;
            } while (IsCorrectSynaps(i));
        }

        // Bind synapses to free neurons
        for (int i = input_neurons + output_neurons; i < synapses_count; i++)
        {
            do
            {
                synapses[i].start_neuron_number = (new System.Random()).Next(0, all_neurons);
                synapses[i].finish_neuron_number = (new System.Random()).Next(input_neurons, all_neurons); ;
            } while (IsCorrectSynaps(i));
        }

        CreateCalculateQueue();
    }
    public Intellect(Intellect to_copy)
    {
        foreach (var item in to_copy.neurons)
            neurons.Add(new Neuron(item.bias));
        foreach (var item in to_copy.synapses)
        {
            synapses.Add(new Synaps(item.weight));
            synapses.Last().start_neuron_number = item.start_neuron_number;
            synapses.Last().finish_neuron_number = item.finish_neuron_number;
        }
        input_neurons = to_copy.input_neurons;
        output_neurons = to_copy.output_neurons;
        foreach (var item in to_copy.calculate_queue)
            calculate_queue.Add(item);
    }
    public Intellect(List<Neuron> neurons, List<Synaps> synapses, int input_neurons, int output_neurons)
    {
        this.neurons = neurons;
        this.synapses = synapses;
        this.input_neurons = input_neurons;
        this.output_neurons = output_neurons;
        CreateCalculateQueue();
    }

    private bool IsCorrectSynaps(int synaps_number)
    {
        List<int> ChackedNeuron = new();
        List<int> Stack = new();
        Stack.Add(synapses[synaps_number].finish_neuron_number);

        while (Stack.Count > 0)
        {
            int current_neuron = Stack.Last();
            Stack.Remove(current_neuron);

            foreach (int syn in synapses.Where(x => x.finish_neuron_number == current_neuron).Select(x => x.start_neuron_number))
            {
                if (syn == synapses[synaps_number].finish_neuron_number) return true;
                if (!ChackedNeuron.Contains(syn))
                {
                    ChackedNeuron.Add(syn);
                    Stack.Add(syn);
                }
            }
        }
        return false;
    }
    private void CreateCalculateQueue()
    {
        List<int> calculated = new();

        for (int i = 0; i < input_neurons; i++)
            calculated.Add(i);

        for (int i = 0; i < input_neurons; i++)
            CalculateUP(i, calculated);
    }
    private void CalculateUP(int neuron_number, List<int> calculated)
    {
        foreach (int item in synapses.Where(x => x.start_neuron_number == neuron_number).Select(x => x.finish_neuron_number))
        {
            CalculateDown(item, calculated);
            CalculateUP(item, calculated);
        }
    }
    private void CalculateDown(int neuron_number, List<int> calculated)
    {
        if (!calculated.Contains(neuron_number))
        {
            foreach (int item in synapses.Where(x => x.finish_neuron_number == neuron_number).Select(x => x.start_neuron_number))
            {
                CalculateDown(item, calculated);
            }
            calculate_queue.Add(neuron_number);
            calculated.Add(neuron_number);
        }
    }
    private void CalculateNeuron(int neuron_number)
    {
        foreach (Synaps syn in synapses.Where(x => x.finish_neuron_number == neuron_number))
            neurons[neuron_number].container += neurons[syn.start_neuron_number].container * syn.weight;
        neurons[neuron_number].container += neurons[neuron_number].bias;
        neurons[neuron_number].Activate();
    }
    public List<float> Think(List<float> information)
    {
        for (int i = 0; i < neurons.Count; i++)
            neurons[i].container = 0;

        for (int i = 0; i < information.Count; i++)
            neurons[i].container = information[i];

        foreach (int i in calculate_queue)
            CalculateNeuron(i);

        List<float> ansver = new();

        for (int i = input_neurons; i < input_neurons + output_neurons; i++)
            ansver.Add(neurons[i].container);
        return ansver;
    }
}
[Serializable]
[SerializeField]
public class Neuron
{
    public float container = 0;
    public float bias = 0;

    public int activation_function = 0;

    public Neuron(float bias)
    {
        this.bias = bias;
        activation_function = 0;
    }
    public void Activate()
    {
        //container = (float)Math.Tanh(container);
        switch (activation_function)
        {
            case 0:
            default: // Гладкая сигмоида (-1, 1)
                container /= (1 + (float)Math.Abs(container));
                break;
            case 1: // Ступенька 0 или 1
                container = container >= 0 ? 1 : 0;
                break;
            case 2: // Логичтическая сигмоида, (0, 1)
                container = 1 / (1 + (float)Math.Exp(-container));
                break;
            case 3: // Обраткая ступенька -1 или 0
                container = container <= 0 ? -1 : 0;
                break;
            case 4: // Бинарная ступенька -1 или 0
                container = container <= 0 ? -1 : 1;
                break;
            case 5: // Синусоида [-1, 1]
                container = (float)Math.Sin(container);
                break;
            case 6: // Косинусоида [-1, 1]
                container = (float)Math.Cos(container);
                break;
            case 7: // Гауссова (0, 1]
                container = (float)Math.Exp(-container * container);
                break;
            case 8: // Обратная гауссова [-1, 0)
                container = -(float)Math.Exp(-container * container);
                break;
            case 9: // Обратная гладкая сигмоида (-1, 1)
                container /= -(1 + (float)Math.Abs(container));
                break;
        }
    }
}
[Serializable]
[SerializeField]
public class Synaps
{
    public int start_neuron_number;
    public int finish_neuron_number;
    public float weight;
    public Synaps(float weight)
    {
        this.weight = weight;
    }
    public Synaps(int start_neuron_number, int finish_neuron_number, float weight)
    {
        this.start_neuron_number = start_neuron_number;
        this.finish_neuron_number = finish_neuron_number;
        this.weight = weight;
    }
}

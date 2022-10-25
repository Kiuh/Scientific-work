using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Intellect
{
    int AllNeuronsCount;
    int AllGensCount;
    int InputNeuronsCount;
    int OutputNeuronsCount;

    #region Private variebles
    List<Neuron> neurons = new List<Neuron>();
    List<Gen> gens = new List<Gen>();
    List<Neuron> inputneurons = new List<Neuron>();
    List<Neuron> outputneurons = new List<Neuron>();
    List<Neuron> calculatequeue = new List<Neuron>();
    #endregion
    #region Constants
    // Gen borders
    const float LeftGenBorder = -1;
    const float RightGenBorder = 1;
    // Bias borders
    const float LeftBiasBorder = -0.05f;
    const float RightBiasBorder = 0.05f;
    #endregion
    #region Constructors
    public Intellect(int InputNeurons, int InnerNeurons, int OutputNeurons, int GensCount)
    {
        InputNeuronsCount = InputNeurons;
        OutputNeuronsCount = OutputNeurons;
        AllGensCount = GensCount;
        AllNeuronsCount = InputNeurons + InnerNeurons + OutputNeurons;
        for (int i = 0; i < AllNeuronsCount; i++) // Создать все нейроны
        {
            neurons.Add(new Neuron(Random.Range(LeftBiasBorder, RightBiasBorder)));
        }
        for (int i = 0; i < GensCount; i++) // Создать все гены
        {
            gens.Add(new Gen(Random.Range(LeftGenBorder, RightGenBorder)));
        }
        for (int i = 0; i < InputNeurons; i++) // Найти входные нейроны и привязять гены к ним(по кол-ву нейронов)
        {
            inputneurons.Add(neurons[i]);

            gens[i].ElementaryNeuron = neurons[i];
            gens[i].ElementaryNeuronNumberInList = i;

            int rand = Random.Range(InputNeurons, AllNeuronsCount - 1);
            gens[i].FinitieNeuron = neurons[rand];
            gens[i].FinitieNeuronNumberInList = rand;
        }
        for (int i = InputNeurons; i < InputNeurons + OutputNeurons; i++) // Привязать гены (по количеству выходных нейронов) к выходным нейронам
        {
            do
            {
                outputneurons.Add(neurons[i]);

                int rand = Random.Range(InputNeurons, AllNeuronsCount - 1);
                gens[i].ElementaryNeuron = neurons[rand];
                gens[i].ElementaryNeuronNumberInList = rand;

                gens[i].FinitieNeuron = neurons[i];
                gens[i].FinitieNeuronNumberInList = i;
            } while (IsBuildsWithHimself(gens[i]));
        }
        for (int i = InputNeurons + OutputNeurons; i < GensCount; i++) // Привязать рандомные гены
        {
            do
            {
                int rand = Random.Range(InputNeurons, AllNeuronsCount - 1);
                gens[i].ElementaryNeuron = neurons[rand];
                gens[i].ElementaryNeuronNumberInList = rand;

                rand = Random.Range(InputNeurons, AllNeuronsCount - 1);
                gens[i].FinitieNeuron = neurons[rand];
                gens[i].FinitieNeuronNumberInList = rand;
            } while (IsBuildsWithHimself(gens[i]));
        }
        FillCalculateQueue();
    }
    public Intellect(Intellect parentintellect)
    {
        neurons = new List<Neuron>();
        foreach (Neuron neuron in parentintellect.neurons)
        {
            neurons.Add(new Neuron(neuron));
        }
        gens = new List<Gen>();
        foreach (Gen gen in parentintellect.gens)
        {
            gens.Add(new Gen(gen));
        }
        AllNeuronsCount = parentintellect.AllNeuronsCount;
        AllGensCount = parentintellect.AllGensCount;
        InputNeuronsCount = parentintellect.InputNeuronsCount;
        OutputNeuronsCount = parentintellect.OutputNeuronsCount;
        ReloadAfterBirth();
    }
    #endregion
    #region Public metods
    public List<float> Think(List<float> information)
    {
        for (int i = 0; i < inputneurons.Count; i++)
        {
            inputneurons[i].Container = information[i];
        }
        foreach (Neuron neuron in calculatequeue)
        {
            CalculateNeuron(neuron);
        }
        List<float> OutList = new List<float>();
        foreach (Neuron neuron in outputneurons)
        {
            OutList.Add(neuron.Container);
        }
        return OutList;
    }
    #endregion
    #region Private methods
    private void ReloadAfterBirth()
    {
        inputneurons = new List<Neuron>();
        outputneurons = new List<Neuron>();
        calculatequeue = new List<Neuron>();
        for (int i = 0; i < InputNeuronsCount; i++)
        {
            inputneurons.Add(neurons[i]);
        }
        for (int i = InputNeuronsCount; i < InputNeuronsCount + OutputNeuronsCount; i++)
        {
            outputneurons.Add(neurons[i]);
        }
        foreach (Gen gen in gens)
        {
            gen.ElementaryNeuron = neurons[gen.ElementaryNeuronNumberInList];
            gen.FinitieNeuron = neurons[gen.FinitieNeuronNumberInList];
        }
        FillCalculateQueue();
    }
    private bool IsBuildsWithHimself(Gen gen)
    {
        List<Neuron> IWasHere = new List<Neuron>();
        List<Neuron> Stack = new List<Neuron> { gen.FinitieNeuron };
        while (Stack.Count != 0)
        {
            Neuron Buff = Stack.Last();
            Stack.Remove(Stack.Last());
            foreach (Gen walkergen in gens.Where(x => x.FinitieNeuron == Buff))
            {
                if (walkergen.ElementaryNeuron == gen.FinitieNeuron) return true;
                if (!IWasHere.Contains(walkergen.ElementaryNeuron))
                {
                    IWasHere.Add(walkergen.ElementaryNeuron);
                    Stack.Add(walkergen.ElementaryNeuron);
                }
            }
        }
        return false;
    }
    private void CalculateNeuron(Neuron neuron) // Посчитать данный нейрон
    {
        foreach (Gen gen in gens.Where(x => x.FinitieNeuron == neuron))
        {
            neuron.Container += gen.ElementaryNeuron.Container * gen.Weight;
        }
        neuron.AddBias();
        ActivateNeuron(neuron);
    }
    private void FillCalculateQueue() // Создать очередь для подсчета нейронной сети
    {
        List<Neuron> Calculated = inputneurons.GetRange(0, inputneurons.Count);
        for (int i = 0; i < inputneurons.Count; i++)
        {
            CalculateUP(inputneurons[i], Calculated);
        }
    }
    private void CalculateUP(Neuron neuron, List<Neuron> Calculated)
    {
        foreach (Gen gen in gens.Where(x => x.ElementaryNeuron == neuron))
        {
            CalculateDown(gen.FinitieNeuron, Calculated);
            CalculateUP(gen.FinitieNeuron, Calculated);
        }
    }
    private void CalculateDown(Neuron neuron, List<Neuron> Calculated)
    {
        if (!Calculated.Contains(neuron))
        {
            foreach (Gen gen in gens.Where(x => x.FinitieNeuron == neuron))
            {
                CalculateDown(gen.ElementaryNeuron, Calculated);
            }
            calculatequeue.Add(neuron);
            Calculated.Add(neuron);
        }
    }
    private void ActivateNeuron(Neuron neuron) // Функция активации
    {
        neuron.Container = 2 / (1 + Mathf.Exp(-2 * neuron.Container)) - 1;
    }
    #endregion
}
public class Neuron
{
    public float Container = 0;
    public float Bias = 0;
    public Neuron(float Bias)
    {
        this.Bias = Bias;
    }
    public Neuron(Neuron neuron)
    {
        Bias = neuron.Bias;
    }
    public void AddBias()
    {
        Container += Bias;
    }
}
public class Gen
{
    public int ElementaryNeuronNumberInList;
    public int FinitieNeuronNumberInList;

    public Neuron ElementaryNeuron;
    public Neuron FinitieNeuron;

    public float Weight;

    public Gen(float Weight)
    {
        this.Weight = Weight;
    }
    public Gen(Gen gen)
    {
        ElementaryNeuronNumberInList = gen.ElementaryNeuronNumberInList;
        FinitieNeuronNumberInList = gen.FinitieNeuronNumberInList;
        Weight = gen.Weight;
    }
    public void BindToNeurons(List<Neuron> neurons)
    {
        ElementaryNeuron = neurons[ElementaryNeuronNumberInList];
        FinitieNeuron = neurons[FinitieNeuronNumberInList];
    }
}

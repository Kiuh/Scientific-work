using Newtonsoft.Json;
using System;
using System.Linq;
using UnityEngine;

public class Random_Generation : MonoBehaviour, ISceneSetup
{
    public RandomGenerationSetupData randomGenerationSetupData;

    public void CreateFirstCells(Action<Component> death, Action<Component> birth)
    {
        Component map = FindObjectsOfType<Component>().ToList().Find((x) => x is IMap);
        for (int i = 0; i < randomGenerationSetupData.start_cells_count; i++)
        {
            CellCreator.CreateStandartCell((map as IMap).GetRandomPositionInArea(), death, birth);
        }
    }
    public void FillWithJson(string json)
    {
        randomGenerationSetupData = JsonConvert.DeserializeObject<RandomGenerationSetupData>(json);
    }
}
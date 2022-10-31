using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Random_Generation : MonoBehaviour, ISceneSetup
{
    public RandomGenerationSetupData randomGenerationSetupData;
    IMap map;
    public List<Cell> CreateFirstCells()
    {
        map = FindObjectsOfType<Component>().ToList().Where(x => x is IMap).Cast<IMap>().First();
        List<Cell> new_cells = new List<Cell>();
        CreateCellParameters cellParameters = new(
            map.GetRandomPositionInArea(),
            CellCreateMode.Random,
            null,
            null
        );
        // Костыль
        randomGenerationSetupData = new();
        randomGenerationSetupData.start_cells_count = 1;
        for (int i = 0; i < randomGenerationSetupData.start_cells_count; i++)
        {
            new_cells.Add(new CellCreator().CreateCell(cellParameters));
            cellParameters.position = map.GetRandomPositionInArea();
        }
        return new_cells;
    }

    public void FillWithJson(string json)
    {
        randomGenerationSetupData = JsonConvert.DeserializeObject<RandomGenerationSetupData>(json);
    }
}
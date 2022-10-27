using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Random_Generation : MonoBehaviour, ISceneSetup
{
    RandomGenerationSetupData randomGenerationSetupData;
    IMap map;
    public void Reset()
    {
        map = FindObjectsOfType<Component>().ToList().Where(x => x is IMap).Cast<IMap>().First();
    }
    public List<Cell> CreateFirstCells()
    {
        List<Cell> new_cells = new List<Cell>();
        CreateCellParameters cellParameters = new(
            map.GetRandomPositionInArea(),
            CellCreateMode.Random,
            null,
            null
        );

        for (int i = 0; i < randomGenerationSetupData.start_cells_count; i++)
        {
            new_cells.Add(new CellCreator().CreateCell(cellParameters));
        }
        return new_cells;
    }

    public void FillWithJson(string json)
    {
        randomGenerationSetupData = JsonConvert.DeserializeObject<RandomGenerationSetupData>(json);
    }
}
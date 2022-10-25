using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Randon_Generation_Input : MonoBehaviour, ISceneSetup
{
    RandomGenerationSetupDataOut randomGenerationSetupDataOut;
    public List<Cell> CreateFirstCells()
    {
        List<Cell> new_cells = new List<Cell>();
        for (int i = 0; i < randomGenerationSetupDataOut.start_cells_count; i++)
        {
            //
        }
        return new_cells;
    }

    public void FillWithJson(string json)
    {
        randomGenerationSetupDataOut = JsonConvert.DeserializeObject<RandomGenerationSetupDataOut>(json);
    }
}

using System;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;

public class RandomGenerationSetup : MonoBehaviour, ISetup
{
    [SerializeField]
    TMP_Text input_text;
    [SerializeField]
    TMP_Text descriptions;
    public void PushInformation(string json)
    {
        descriptions.text = JsonConvert.DeserializeObject<RandomGenerationSetupDataIn>(json).description;
    }
    public string GetNewInformation()
    {
        RandomGenerationSetupDataOut rgsd = new()
        {
            start_cells_count = Convert.ToInt32(input_text.text)
        };
        return JsonConvert.SerializeObject(rgsd);
    }
}

public class RandomGenerationSetupDataIn
{
    public string description;
}

public class RandomGenerationSetupDataOut
{
    public int start_cells_count;
}
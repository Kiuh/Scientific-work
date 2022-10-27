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
        descriptions.text = JsonConvert.DeserializeObject<RandomGenerationSetupData>(json).description;
    }
    public string GetNewInformation()
    {
        RandomGenerationSetupData rgsd = new()
        {
            start_cells_count = Convert.ToInt32(input_text.text),
            description = descriptions.text
        };
        return JsonConvert.SerializeObject(rgsd);
    }
}

public class RandomGenerationSetupData
{
    public int start_cells_count;
    public string description;
}

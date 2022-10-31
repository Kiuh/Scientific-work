using System;
using TMPro;
using UnityEngine;
using Newtonsoft.Json;

public class RandomGenerationSetup : MonoBehaviour, ISetup
{
    [SerializeField]
    TMP_InputField input_text;
    [SerializeField]
    TMP_Text descriptions;
    public void PushInformation(string json)
    {
        descriptions.text = JsonConvert.DeserializeObject<RandomGenerationSetupData>(json).description;
    }
    public string GetNewInformation()
    {
        RandomGenerationSetupData rgsd = new();

        rgsd.start_cells_count = int.Parse(input_text.text);
        rgsd.description = descriptions.text;

        return JsonConvert.SerializeObject(rgsd);
    }
}
[Serializable]
public class RandomGenerationSetupData
{
    public int start_cells_count;
    public string description;
}

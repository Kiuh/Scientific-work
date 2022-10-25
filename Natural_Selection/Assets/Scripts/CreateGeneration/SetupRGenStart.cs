using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.Search;
using UnityEngine;

public class SetupRGenStart : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown maps_dd;
    [SerializeField]
    TMP_Dropdown life_rule_dd;
    [SerializeField]
    TMP_Dropdown feeding_rule_dd;
    [SerializeField]
    TMP_Dropdown ticks_dd;
    [SerializeField]
    TMP_Dropdown start_options_dd;

    delegate void Show();
    List<Show> shows = new List<Show>();

    [SerializeField]
    RectTransform info_rt;

    public GameObject current_show;

    public void Start()
    {
        List<string> maps = ServerSpeaker.GetMapsNames();
        List<string> life_rules = ServerSpeaker.GetLifeRulesNames();
        List<string> feeding_rules = ServerSpeaker.GetFeedingRulesNames();
        List<string> ticks = ServerSpeaker.GetTicksNames().Select((x) => Convert.ToString(x)).ToList();

        maps_dd.ClearOptions();
        maps_dd.AddOptions(maps);

        life_rule_dd.ClearOptions();
        life_rule_dd.AddOptions(life_rules);

        feeding_rule_dd.ClearOptions();
        feeding_rule_dd.AddOptions(feeding_rules);

        ticks_dd.ClearOptions();
        ticks_dd.AddOptions(ticks);

        start_options_dd.ClearOptions();
        List<ServerSpeaker.StartGenerationRule> start_options = ServerSpeaker.GetGenerationSetupsAndJsons();
        foreach (var item in start_options)
        {
            start_options_dd.AddOptions(new List<string>() { item.name });
            GameObject go = Resources.Load("GenerationSetups/" + item.name) as GameObject;
            shows.Add(() => {
                current_show = Instantiate(go, info_rt);
                current_show.GetComponent<ISetup>().PushInformation(item.json);
                }
            );
        }
        shows[0]();
    }

    public void OnValueChange(int val)
    {
        Destroy(current_show);
        shows[val]();
    }
}

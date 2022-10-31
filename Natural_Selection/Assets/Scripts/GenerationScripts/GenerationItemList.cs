using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class GenerationItemList : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    TMP_Text gen_name;
    [SerializeField]
    TMP_Text gen_time;
    [SerializeField]
    TMP_Text end_count;
    [SerializeField]
    TMP_Text gen_type;

    public GenerationInfo InfoPanel;

    string gen_name_string;
    string gen_type_string;

    ServerSpeaker ss;

    private void Start()
    {
        ss = FindObjectOfType<ServerSpeaker>();
    }

    public void InitListItem(string name, string type)
    {
        gen_name_string = name;
        gen_type_string = type;

        gen_name.text = name;
        
        gen_type.text = type;
        
    }

    public void SetTime(string time)
    {
        gen_time.text = time;
    }

    public void Setcount(string count)
    {
        end_count.text = count;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ss.GetGenerations(Click);
    }

    public void Click(ServerSpeaker.GenerationsResponse Data)
    {
        var data = Data.generations.Where(x => x.name == gen_name_string).FirstOrDefault();
        InfoPanel.InitInfo(gen_name_string, data.map, gen_type_string, Convert.ToString(data.tick), data.description);

        FindObjectOfType<ContonueGeneratin>().choosedgeneration = gen_name_string;
        FindObjectOfType<DeleteChoosedGeneration>().choosedgeneration = gen_name_string;
    }
}

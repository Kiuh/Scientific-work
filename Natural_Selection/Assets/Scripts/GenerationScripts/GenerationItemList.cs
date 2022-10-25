using System.Collections;
using System.Collections.Generic;
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

    public void InitListItem(string name, string time, string count, string type)
    {
        gen_name_string = name;
        gen_type_string = type;

        gen_name.text = name;
        gen_time.text = time;
        gen_type.text = type;
        end_count.text = count;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ServerSpeaker.GenerationData2 data = ServerSpeaker.GetGeneration_Map_Tick_Comments(gen_name_string);
        InfoPanel.InitInfo(gen_name_string, data.map_name, gen_type_string, data.tick, data.comment);
    }
}

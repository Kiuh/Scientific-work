using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerationMenuStarter : MonoBehaviour
{
    [SerializeField]
    GameObject GenerationListItem;
    [SerializeField]
    GameObject content;
    [SerializeField]
    GenerationInfo InfoPanel;
    void Start()
    {
        FillListWithItems();
    }

    public void DeleteAllItemsInContent()
    {
        List<RectTransform> list = content.GetComponentsInChildren<RectTransform>().ToList();
        foreach (var item in list)
        {
            Destroy(item.gameObject);
        }
    }

    public void FillListWithItems()
    {
        List<ServerSpeaker.GenerationData1> gens = ServerSpeaker.GetGenerations_Names_Types();
        foreach (var g in gens)
        {
            GameObject go = Instantiate(GenerationListItem, content.transform);
            go.GetComponent<GenerationItemList>().InitListItem(
                g.name,
                Convert.ToString(ServerSpeaker.GetGeneration_Time(g.name).time),
                Convert.ToString(ServerSpeaker.GetGeneration_LifeEnds(g.name).LifeEnds),
                g.life_type
            );
            go.GetComponent<GenerationItemList>().InfoPanel = InfoPanel;
        }
    }
}

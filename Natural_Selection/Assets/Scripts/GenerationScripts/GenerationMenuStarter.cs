using System;
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
        var generations = ServerSpeaker.GetGenerations();
        foreach (var g in generations.generations)
        {
            GameObject go = Instantiate(GenerationListItem, content.transform);
            go.GetComponent<GenerationItemList>().InitListItem(
                g.name,
                Convert.ToString(ServerSpeaker.GetGenerationTime(g.name).time),
                Convert.ToString(ServerSpeaker.GetGenerationLifeEnds(g.name).life_ends),
                g.life_type
            );
            go.GetComponent<GenerationItemList>().InfoPanel = InfoPanel;
        }
    }
}

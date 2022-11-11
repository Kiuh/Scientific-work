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

    ServerSpeaker ss;
    void Start()
    {
        ss = FindObjectOfType<ServerSpeaker>();
        FillListWithItems();
    }

    public void DeleteAllItemsInContent()
    {
        List<RectTransform> list = content.GetComponentsInChildren<RectTransform>().ToList();
        list.Remove(content.GetComponent<RectTransform>());
        foreach (var item in list)
        {
            Destroy(item.gameObject);
        }
    }

    public void FillListWithItems()
    {
        ss.GetGenerations(FillListWithItems_WW);
    }

    public void DeleteCurrentInfoPanel()
    {
        InfoPanel.gameObject.SetActive(false);
    }

    void FillListWithItems_WW(ServerSpeaker.GenerationsResponse generations)
    {
        if(generations.generations == null)
            return;
        foreach (var g in generations.generations)
        {
            FillOneItem(g);
        }
    }

    void FillOneItem(ServerSpeaker.GenerationData g)
    {
        GameObject go = Instantiate(GenerationListItem, content.transform);

        GenerationItemList GIL = go.GetComponent<GenerationItemList>();

        GIL.InitListItem(g.name, g.life_type.tp_name);
        ss.GetGenerationTime(g.name, (ServerSpeaker.GenerationTimeResponse var) => GIL.SetTime(Convert.ToString(var.time)));
        ss.GetGenerationLifeEnds(g.name, (ServerSpeaker.GenerationLifeEndsResponse var) => GIL.Setcount(Convert.ToString(var.life_ends)));
        GIL.InfoPanel = InfoPanel;
    }
}

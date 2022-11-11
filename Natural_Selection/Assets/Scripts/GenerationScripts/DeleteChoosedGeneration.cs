using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DeleteChoosedGeneration : MonoBehaviour, IPointerClickHandler
{
    public string choosedgeneration;
    [SerializeField]
    Massager MGR;
    [SerializeField]
    GenerationMenuStarter menuStarter;

    ServerSpeaker ss;

    public void Start()
    {
        ss = FindObjectOfType<ServerSpeaker>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (choosedgeneration != "")
        {
            ss.DeleteGeneration(choosedgeneration, Reqest);
        }
        else
        {
            MGR.ShowMassage("Choose generation");
        }
    }

    void Reqest(bool value)
    {
        if (value)
        {
            menuStarter.DeleteAllItemsInContent();
            menuStarter.FillListWithItems();
            menuStarter.DeleteCurrentInfoPanel();
        }
        else
        {
            MGR.ShowMassage("Fail to delete generation.");
        }
    }
}

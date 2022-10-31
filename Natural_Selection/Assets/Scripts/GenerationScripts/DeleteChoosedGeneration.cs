using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class DeleteChoosedGeneration : MonoBehaviour, IPointerClickHandler
{
    public string choosedgeneration;
    [SerializeField]
    TMP_Text massage;
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
            massage.text = "Выберете генерацию.";
        }
    }

    void Reqest(bool value)
    {
        if (value)
        {
            menuStarter.DeleteAllItemsInContent();
            menuStarter.FillListWithItems();
        }
        else
        {
            massage.text = "Не удалось удалить.";
        }
    }
}

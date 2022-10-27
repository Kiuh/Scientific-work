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
    public void OnPointerClick(PointerEventData eventData)
    {
        if (choosedgeneration != "")
        {
            ServerSpeaker.DeleteGeneration(choosedgeneration);
            menuStarter.DeleteAllItemsInContent();
            menuStarter.FillListWithItems();
        }
        else
        {
            massage.text = "Выберете генерацию.";
        }
    }
}

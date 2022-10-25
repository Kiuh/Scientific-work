using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ContonueGeneratin : MonoBehaviour, IPointerClickHandler
{
    public string choosedgeneration;
    [SerializeField]
    TMP_Text massage;

    [SerializeField]
    GameObject Gen_Info;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (choosedgeneration != "")
        {
            Instantiate(Gen_Info);
            Gen_Info.GetComponent<GenInfo>().Generation_name = choosedgeneration;
            DontDestroyOnLoad(Gen_Info);
            SceneManager.LoadScene("Simulation");
        }
        else
        {
            massage.text = "Выберете генерацию.";
        }
    }
}

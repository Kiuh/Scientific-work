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
    Massager MGR;

    [SerializeField]
    GameObject Gen_Info;
    public void OnPointerClick(PointerEventData eventData)
    {
        if (choosedgeneration != "")
        {
            GameObject go = Instantiate(Gen_Info);
            go.GetComponent<GenInfo>().Generation_name = choosedgeneration;
            DontDestroyOnLoad(go);
            SceneManager.LoadScene("Simulation");
        }
        else
        {
            MGR.ShowMassage("Choose generation.");
        }
    }
}

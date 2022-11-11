using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ToMainMenuGenMenu : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>().LoadScene("MainMenu");
    }
}

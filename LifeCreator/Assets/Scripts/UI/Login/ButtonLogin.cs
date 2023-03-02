using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonLogin : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    TMP_Text login;
    [SerializeField]
    TMP_Text password;
    SceneChanger changer;
    ServerSpeaker ss;
    public void Start()
    {
        changer = FindObjectOfType<SceneChanger>();
        ss = FindObjectOfType<ServerSpeaker>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        var data = new ServerSpeaker.LogInData(login.text, password.text);
        ss.LogIn(data, AfterLogin);
    }
    void AfterLogin(long status_code)
    {
        if(status_code == 200)
        {
            changer.LoadScene("Menu");
            Debug.Log("All good");
        }
        else
        {
            Debug.LogWarning(status_code);
            
        }
    }
}

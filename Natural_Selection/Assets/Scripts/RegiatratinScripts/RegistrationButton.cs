using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RegistrationButton : MonoBehaviour
{
    [SerializeField]
    TMP_Text login;
    [SerializeField]
    TMP_Text email;
    [SerializeField]
    TMP_Text password;
    [SerializeField]
    TMP_Text error_massage;
    public void OnPointerClick(PointerEventData eventData)
    {
        string error = "";
        ServerSpeaker.RegistrationData registrationData = new()
        {
            login = login.text,
            email = email.text,
            password = password.text
        };
        if (ServerSpeaker.Registration(registrationData , ref error))
            SceneManager.LoadScene("Login");
        else
            error_massage.text = error;
    }
}

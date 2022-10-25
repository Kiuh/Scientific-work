using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class LoginButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    TMP_Text login;
    [SerializeField]
    TMP_Text password;
    [SerializeField]
    TMP_Text error_massage;
    public void OnPointerClick(PointerEventData eventData)
    {
        string error = "";
        ServerSpeaker.LoginData loginData = new()
        {
            login = login.text,
            password = password.text,
        };
        if (ServerSpeaker.Login(loginData, ref error))
            SceneManager.LoadScene("MainMenu");
        else
            error_massage.text = error;
    }
}

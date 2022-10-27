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
        if (ServerSpeaker.Login(new ServerSpeaker.LoginData(login.text, password.text)))
            SceneManager.LoadScene("MainMenu");
        else
            error_massage.text = "Something Wrong";
    }
}

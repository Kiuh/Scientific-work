using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class RegistrationButton : MonoBehaviour, IPointerClickHandler
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
        if (ServerSpeaker.Registration(new ServerSpeaker.RegistrationData(login.text, email.text, password.text)))
            SceneManager.LoadScene("Login");
        else
            error_massage.text = "Something wrong";
    }
}

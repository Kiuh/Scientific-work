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
    Massager MGR;

    ServerSpeaker ss;

    private void Start()
    {
        ss = FindObjectOfType<ServerSpeaker>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ss.Registration(new ServerSpeaker.RegistrationData(login.text, email.text, password.text), Registration);
    }

    public void Registration(bool value)
    {
        if (value)
            GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>().LoadScene("Login");
        else
            MGR.ShowMassage("Something wrong");
    }
}

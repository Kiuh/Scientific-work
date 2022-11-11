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
    Massager MGR;

    ServerSpeaker ss;

    private void Start()
    {
        ss = FindObjectOfType<ServerSpeaker>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ss.Login(new ServerSpeaker.LoginData(login.text, password.text), Login);
    }

    public void Login(bool input)
    {
        if (input)
            GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>().LoadScene("MainMenu");
        else
            MGR.ShowMassage("Something Wrong");
    }

}

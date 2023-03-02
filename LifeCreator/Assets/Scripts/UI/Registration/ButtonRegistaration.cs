using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonRegistaration : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    TMP_Text login;
    [SerializeField]
    TMP_Text email;
    [SerializeField]
    TMP_Text password;
    [SerializeField]
    TMP_Text repeat_password;
    [SerializeField]
    GameObject SuccessRegister;
    ServerSpeaker ss;
    public void Start()
    {
        ss = FindObjectOfType<ServerSpeaker>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (password.text != repeat_password.text)
        {
            Debug.Log("Not match.");
            return;
        }
        Debug.Log(login.text + " " + email.text + " " + password.text);
        var data = new ServerSpeaker.RegistrationOpenData(login.text, email.text, password.text);
        ss.Registration(data, AfterClick);
    }
    void AfterClick(long status_code)
    {
        if (status_code == 200)
        {
            SuccessRegister.SetActive(true);
        }
        else
        {
            Debug.LogWarning(status_code);
        }
    }
}

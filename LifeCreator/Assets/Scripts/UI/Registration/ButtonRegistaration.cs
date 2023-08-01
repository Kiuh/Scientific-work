using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonRegistaration : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text login;

    [SerializeField]
    private TMP_Text email;

    [SerializeField]
    private TMP_Text password;

    [SerializeField]
    private TMP_Text repeat_password;

    [SerializeField]
    private GameObject successRegister;
    private ServerSpeaker ss;

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
        ServerSpeaker.RegistrationOpenData data = new(login.text, email.text, password.text);
        ss.Registration(data, AfterClick);
    }

    private void AfterClick(long status_code)
    {
        if (status_code == 200)
        {
            successRegister.SetActive(true);
        }
        else
        {
            Debug.LogWarning(status_code);
        }
    }
}

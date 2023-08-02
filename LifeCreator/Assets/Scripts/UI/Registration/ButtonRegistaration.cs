using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonRegistration : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TextMeshPro login;

    [SerializeField]
    private TMP_Text email;

    [SerializeField]
    private TMP_Text password;

    [SerializeField]
    private TMP_Text repeatPassword;

    [SerializeField]
    private GameObject successRegister;
    private ServerSpeaker serverSpeaker;

    public void Start()
    {
        serverSpeaker = FindObjectOfType<ServerSpeaker>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (password.text != repeatPassword.text)
        {
            Debug.Log("Not match.");
            return;
        }
        Debug.Log(login.text + " " + email.text + " " + password.text);
        ServerSpeaker.RegistrationOpenData data = new(login.text, email.text, password.text);
        serverSpeaker.Registration(data, AfterClick);
    }

    private void AfterClick(long statusCode)
    {
        if (statusCode == 200)
        {
            successRegister.SetActive(true);
        }
        else
        {
            Debug.LogWarning(statusCode);
        }
    }
}

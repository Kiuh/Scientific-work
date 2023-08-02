using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSendNewPassword : MonoBehaviour, IPointerClickHandler
{
    public string Email;

    [SerializeField]
    private TMP_Text accessCode;

    [SerializeField]
    private TMP_Text newPassword;

    [SerializeField]
    private GameObject myself;

    [SerializeField]
    private GameObject successChange;
    private ServerSpeaker serverSpeaker;

    public void Start()
    {
        serverSpeaker = FindObjectOfType<ServerSpeaker>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (accessCode.text == "" || newPassword.text == "new_password")
        {
            Debug.Log("Loh!");
            return;
        }
        ServerSpeaker.ChangePasswordOpenData data = new(accessCode.text, newPassword.text, Email);
        serverSpeaker.ChangePassword(data, AfterClick);
    }

    public void AfterClick(long status_code)
    {
        if (status_code == 200)
        {
            successChange.SetActive(true);
            myself.SetActive(false);
        }
        else
        {
            Debug.LogWarning(status_code);
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSendCode : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private ButtonSendNewPassword buttonSendNewPassword;

    [SerializeField]
    private TMP_Text email;

    [SerializeField]
    private GameObject codeInput;

    [SerializeField]
    private GameObject myself;
    private ServerSpeaker serverSpeaker;

    public void Start()
    {
        serverSpeaker = FindObjectOfType<ServerSpeaker>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (email.text == "")
        {
            Debug.Log("Loh!");
            return;
        }
        ServerSpeaker.RequestToChangePasswordOpenData data = new(email.text);
        serverSpeaker.RequestToChangePassword(data, AfterClick);
    }

    public void AfterClick(long status_code)
    {
        if (status_code == 200)
        {
            buttonSendNewPassword.Email = email.text;
            codeInput.SetActive(true);
            myself.SetActive(false);
        }
        else
        {
            Debug.LogWarning(status_code);
        }
    }
}

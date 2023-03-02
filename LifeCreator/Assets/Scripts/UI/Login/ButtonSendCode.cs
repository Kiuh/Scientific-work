using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSendCode : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    ButtonSendNewPassword BSNP;
    [SerializeField]
    TMP_Text email;
    [SerializeField]
    GameObject code_input;
    [SerializeField]
    GameObject myself;
    ServerSpeaker ss;
    public void Start()
    {
        ss = FindObjectOfType<ServerSpeaker>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if(email.text == "")
        {
            Debug.Log("Loh!");
            return;
        }
        var data = new ServerSpeaker.WhantChangePasswordOpenData(email.text);
        ss.WhantChangePassword(data, AfterClick);
    }
    public void AfterClick(long status_code)
    {
        if(status_code == 200)
        {
            BSNP.email = email.text;
            code_input.SetActive(true);
            myself.SetActive(false);
        }
        else
        {
            Debug.LogWarning(status_code);
        }
    }
}

using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSendNewPassword : MonoBehaviour, IPointerClickHandler
{
    public string email;
    [SerializeField]
    TMP_Text access_code;
    [SerializeField]
    TMP_Text new_password;
    [SerializeField]
    GameObject myself;
    [SerializeField]
    GameObject success_change;
    ServerSpeaker ss;
    public void Start()
    {
        ss = FindObjectOfType<ServerSpeaker>();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (access_code.text == "" || new_password.text == "new_password")
        {
            Debug.Log("Loh!");
            return;
        }
        var data = new ServerSpeaker.ChangePasswordOpenData(access_code.text, new_password.text, email);
        ss.ChangePassword(data, AfterClick);
    }
    public void AfterClick(long status_code)
    {
        if (status_code == 200)
        {
            success_change.SetActive(true);
            myself.SetActive(false);
        }
        else
        {
            Debug.LogWarning(status_code);
        }
    }
}

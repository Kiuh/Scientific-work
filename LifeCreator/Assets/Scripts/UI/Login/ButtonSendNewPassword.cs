using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSendNewPassword : MonoBehaviour, IPointerClickHandler
{
    public string Email;

    [SerializeField]
    private TMP_Text access_code;

    [SerializeField]
    private TMP_Text new_password;

    [SerializeField]
    private GameObject myself;

    [SerializeField]
    private GameObject success_change;
    private ServerSpeaker ss;

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
        ServerSpeaker.ChangePasswordOpenData data = new(access_code.text, new_password.text, Email);
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

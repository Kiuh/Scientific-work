using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonLogin : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private TMP_Text login;

    [SerializeField]
    private TMP_Text password;
    private SceneChanger changer;
    private ServerSpeaker ss;

    public void Start()
    {
        changer = FindObjectOfType<SceneChanger>();
        ss = FindObjectOfType<ServerSpeaker>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        ServerSpeaker.LogInData data = new(login.text, password.text);
        ss.LogIn(data, AfterLogin);
    }

    private void AfterLogin(long statusCode)
    {
        if (statusCode == 200)
        {
            changer.LoadScene("Menu");
            Debug.Log("All good");
        }
        else
        {
            Debug.LogWarning(statusCode);
        }
    }
}

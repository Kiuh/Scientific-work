using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class BackToLoginButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>().LoadScene("Login");
    }
}

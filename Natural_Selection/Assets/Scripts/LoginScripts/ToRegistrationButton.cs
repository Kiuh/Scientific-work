using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ToRegistrationButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>().LoadScene("Registration");
    }
}

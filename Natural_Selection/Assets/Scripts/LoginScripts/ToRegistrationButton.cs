using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ToRegistrationButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        SceneManager.LoadScene("Registration");
    }
}
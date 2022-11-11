using UnityEngine;
using UnityEngine.EventSystems;

public class PauseButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        //GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>().LoadScene("Login");
    }
}


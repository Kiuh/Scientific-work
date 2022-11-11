using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class OpenGenerationsButton : MonoBehaviour, IPointerClickHandler
{
    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject.FindGameObjectWithTag("SceneChanger").GetComponent<SceneChanger>().LoadScene("Generations");
    }
}

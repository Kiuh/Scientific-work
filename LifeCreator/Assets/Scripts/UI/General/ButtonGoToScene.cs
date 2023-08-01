using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonGoToScene : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private string scene_name = "";
    private SceneChanger changer;

    public void Start()
    {
        changer = FindObjectOfType<SceneChanger>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        changer.LoadScene(scene_name);
    }
}

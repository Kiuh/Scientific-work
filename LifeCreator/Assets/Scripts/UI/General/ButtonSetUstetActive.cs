using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSetUstetActive : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    GameObject target;
    [SerializeField]
    bool state;
    public void OnPointerClick(PointerEventData eventData)
    {
        target.SetActive(state);
    }
}

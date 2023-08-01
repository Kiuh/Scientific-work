using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonSetUstetActive : MonoBehaviour, IPointerClickHandler
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private bool state;

    public void OnPointerClick(PointerEventData eventData)
    {
        target.SetActive(state);
    }
}

using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    float normal_size = 1f;
    [SerializeField]
    float hovered_size =  1.1f;
    [SerializeField]
    float current_size = 1f;
    [SerializeField]
    float animation_speed = 4f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        current_size = hovered_size;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        current_size = normal_size;
    }

    private void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(current_size, current_size, transform.localScale.z), Time.deltaTime * animation_speed);
    }


}

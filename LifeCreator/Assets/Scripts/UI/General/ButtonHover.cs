using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float start_scale = 1f;

    [SerializeField]
    private float hover_scale = 1.05f;

    [SerializeField]
    private float change_speed = 7f;

    [SerializeField]
    private bool hovered = false;

    [Space]
    [SerializeField]
    private float true_start_scale;

    [SerializeField]
    private float true_hover_scale;
    private RectTransform rt;

    public void Start()
    {
        rt = GetComponent<RectTransform>();
        true_start_scale = rt.localScale.x * start_scale;
        true_hover_scale = rt.localScale.x * hover_scale;
    }

    public void Update()
    {
        rt.localScale = hovered
            ? Vector3.Lerp(
                rt.localScale,
                new Vector3(true_hover_scale, true_hover_scale, true_hover_scale),
                Time.deltaTime * change_speed
            )
            : Vector3.Lerp(
                rt.localScale,
                new Vector3(true_start_scale, true_start_scale, true_start_scale),
                Time.deltaTime * change_speed
            );
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered = false;
    }
}

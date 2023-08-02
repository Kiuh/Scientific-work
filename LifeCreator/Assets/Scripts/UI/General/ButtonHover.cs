using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    private float startScale = 1f;

    [SerializeField]
    private float hoverScale = 1.05f;

    [SerializeField]
    private float changeSpeed = 7f;

    [SerializeField]
    private bool hovered = false;

    [Space]
    [SerializeField]
    private float trueStartScale;

    [SerializeField]
    private float trueHoverScale;
    private RectTransform rectTransform;

    public void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        trueStartScale = rectTransform.localScale.x * startScale;
        trueHoverScale = rectTransform.localScale.x * hoverScale;
    }

    public void Update()
    {
        rectTransform.localScale = hovered
            ? Vector3.Lerp(
                rectTransform.localScale,
                new Vector3(trueHoverScale, trueHoverScale, trueHoverScale),
                Time.deltaTime * changeSpeed
            )
            : Vector3.Lerp(
                rectTransform.localScale,
                new Vector3(trueStartScale, trueStartScale, trueStartScale),
                Time.deltaTime * changeSpeed
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

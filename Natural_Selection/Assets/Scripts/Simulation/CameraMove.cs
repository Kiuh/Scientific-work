using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    Vector3 startMousePosition, startTransformPos;
    float lerpSpeed = 1f;
    Camera cam;
    const float maxScale = 200f;
    const float minScale = 1f;
    float defaultScale, scaleChange = 0;
    float cameraMoveSpeed = 100f;
    float zoomSpeed = 1f;
    float scrollInput;

    void Start()
    {
        cam = GetComponent<Camera>();
        defaultScale = cam.orthographicSize;
    }
    void Update()
    {
        scrollInput = Input.mouseScrollDelta.y;
        if ((scrollInput < 0 && cam.orthographicSize < maxScale) || (scrollInput > 0 && cam.orthographicSize > minScale))
        {
            scaleChange -= scrollInput * zoomSpeed;
            cam.orthographicSize -= scrollInput * zoomSpeed;
        }
        if (cam.orthographicSize < minScale)
        {
            cam.orthographicSize = minScale;
            scaleChange = minScale - defaultScale;
        }
        if (cam.orthographicSize > maxScale)
        {
            cam.orthographicSize = maxScale;
            scaleChange = maxScale - defaultScale;
        }
        if (Input.GetMouseButtonDown(0))
        {
            startMousePosition = Input.mousePosition;
            startTransformPos = transform.position;
        }
        if (!Input.GetMouseButton(0)) return;
        if (Input.mousePosition != startMousePosition)
        {
            Vector3 move = Camera.main.ScreenToViewportPoint(Input.mousePosition - startMousePosition);
            move *= cameraMoveSpeed * (defaultScale + scaleChange) / defaultScale;
            transform.position = Vector3.Lerp(transform.position, -move + startTransformPos, lerpSpeed);
        }

    }
}

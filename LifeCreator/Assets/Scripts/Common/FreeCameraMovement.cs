using UnityEngine;

namespace Assets.Scripts.Common
{
    [AddComponentMenu("Common.FreeCameraMovement")]
    public class FreeCameraMovement : MonoBehaviour
    {
        [SerializeField]
        private float shiftMultiplier = 2f;

        [SerializeField]
        private float panSensitivity = 0.5f;

        [SerializeField]
        private float mouseWheelZoomSpeed = 1.0f;

        private bool isPanning;

        private void Update()
        {
            MousePanning();
            if (isPanning)
            {
                return;
            }

            MouseWheeling();

            if (Input.mouseScrollDelta == Vector2.down)
            {
                Camera.main.orthographicSize += 1;
            }
            else if (Input.mouseScrollDelta == Vector2.up)
            {
                Camera.main.orthographicSize -= 1;
            }

            if (Camera.main.orthographicSize < 4)
            {
                Camera.main.orthographicSize = 4;
            }
            else if (Camera.main.orthographicSize > 50)
            {
                Camera.main.orthographicSize = 50;
            }
        }

        //Zoom with mouse wheel
        private void MouseWheeling()
        {
            float speed =
                10
                * (
                    mouseWheelZoomSpeed
                    * (Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f)
                    * Time.deltaTime
                    * 9.1f
                );

            Vector3 pos = transform.position;
            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                pos -= transform.forward * speed;
                transform.position = pos;
            }
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                pos += transform.forward * speed;
                transform.position = pos;
            }
        }

        private float pan_x;
        private float pan_y;
        private Vector3 panComplete;

        private void MousePanning()
        {
            pan_x = -Input.GetAxis("Mouse X") * panSensitivity;
            pan_y = -Input.GetAxis("Mouse Y") * panSensitivity;
            panComplete = new Vector3(pan_x, pan_y, 0);

            if (Input.GetMouseButtonDown(2))
            {
                isPanning = true;
            }

            if (Input.GetMouseButtonUp(2))
            {
                isPanning = false;
            }

            if (isPanning)
            {
                transform.Translate(panComplete);
            }
        }
    }
}

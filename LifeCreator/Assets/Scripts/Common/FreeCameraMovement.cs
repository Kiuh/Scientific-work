using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common
{
    /// <summary> A modular and easily customizable Unity MonoBehaviour for handling swipe and pinch motions on mobile. </summary>
    public class PanAndZoom : MonoBehaviour
    {
        [Header("Tap")]
        [Tooltip("The maximum movement for a touch motion to be treated as a tap")]
        public float MaxDistanceForTap = 40;

        [Tooltip("The maximum duration for a touch motion to be treated as a tap")]
        public float MaxDurationForTap = 0.4f;

        [Header("Desktop debug")]
        [Tooltip("Use the mouse on desktop?")]
        public bool UseMouse = true;

        [Tooltip("The simulated pinch speed using the scroll wheel")]
        public float MouseScrollSpeed = 2;

        [Header("Camera control")]
        [Tooltip("Does the script control camera movement?")]
        public bool ControlCamera = true;

        [Tooltip("The controlled camera, ignored of controlCamera=false")]
        public Camera Cam;

        [Header("UI")]
        [Tooltip("Are touch motions listened to if they are over UI elements?")]
        public bool IgnoreUI = false;

        [Header("Bounds")]
        [Tooltip("Is the camera bound to an area?")]
        public bool UseBounds;

        public float BoundMinX = -150;
        public float BoundMaxX = 150;
        public float BoundMinY = -150;
        public float BoundMaxY = 150;
        private Vector2 touch0StartPosition;
        private Vector2 touch0LastPosition;
        private float touch0StartTime;
        private bool cameraControlEnabled = true;
        private bool canUseMouse;

        /// <summary> Has the player at least one finger on the screen? </summary>
        public bool IsTouching { get; private set; }

        /// <summary> The point of contact if it exists in Screen space. </summary>
        public Vector2 TouchPosition => touch0LastPosition;

        private void Start()
        {
            canUseMouse =
                Application.platform != RuntimePlatform.Android
                && Application.platform != RuntimePlatform.IPhonePlayer
                && Input.mousePresent;
        }

        private void Update()
        {
            if (UseMouse && canUseMouse)
            {
                UpdateWithMouse();
            }
            else
            {
                UpdateWithTouch();
            }
        }

        private void LateUpdate()
        {
            CameraInBounds();
        }

        private void UpdateWithMouse()
        {
            if (Input.GetMouseButtonDown(1))
            {
                if (IgnoreUI || !IsPointerOverUIObject())
                {
                    touch0StartPosition = Input.mousePosition;
                    touch0StartTime = Time.time;
                    touch0LastPosition = touch0StartPosition;

                    IsTouching = true;
                }
            }

            if (Input.GetMouseButton(1) && IsTouching)
            {
                Vector2 move = (Vector2)Input.mousePosition - touch0LastPosition;
                touch0LastPosition = Input.mousePosition;

                if (move != Vector2.zero)
                {
                    OnSwipe(move);
                }
            }

            if (Input.GetMouseButtonUp(1) && IsTouching)
            {
                IsTouching = false;
                cameraControlEnabled = true;
            }

            if (Input.mouseScrollDelta.y != 0)
            {
                OnPinch(
                    Input.mousePosition,
                    1,
                    Input.mouseScrollDelta.y < 0 ? (1 / MouseScrollSpeed) : MouseScrollSpeed,
                    Vector2.right
                );
            }
        }

        private void UpdateWithTouch()
        {
            int touchCount = Input.touches.Length;

            if (touchCount == 1)
            {
                Touch touch = Input.touches[0];

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        {
                            if (IgnoreUI || !IsPointerOverUIObject())
                            {
                                touch0StartPosition = touch.position;
                                touch0StartTime = Time.time;
                                touch0LastPosition = touch0StartPosition;

                                IsTouching = true;
                            }

                            break;
                        }
                    case TouchPhase.Moved:
                        {
                            touch0LastPosition = touch.position;

                            if (touch.deltaPosition != Vector2.zero && IsTouching)
                            {
                                OnSwipe(touch.deltaPosition);
                            }
                            break;
                        }
                    case TouchPhase.Ended:
                        {
                            IsTouching = false;
                            cameraControlEnabled = true;
                            break;
                        }
                    case TouchPhase.Stationary:
                    case TouchPhase.Canceled:
                        break;
                }
            }
            else if (touchCount == 2)
            {
                Touch touch0 = Input.touches[0];
                Touch touch1 = Input.touches[1];

                if (touch0.phase == TouchPhase.Ended || touch1.phase == TouchPhase.Ended)
                {
                    return;
                }

                IsTouching = true;

                float previousDistance = Vector2.Distance(
                    touch0.position - touch0.deltaPosition,
                    touch1.position - touch1.deltaPosition
                );

                float currentDistance = Vector2.Distance(touch0.position, touch1.position);

                if (previousDistance != currentDistance)
                {
                    OnPinch(
                        (touch0.position + touch1.position) / 2,
                        previousDistance,
                        currentDistance,
                        (touch1.position - touch0.position).normalized
                    );
                }
            }
            else
            {
                if (IsTouching)
                {
                    IsTouching = false;
                }

                cameraControlEnabled = true;
            }
        }

        private void OnSwipe(Vector2 deltaPosition)
        {
            if (ControlCamera && cameraControlEnabled)
            {
                if (Cam == null)
                {
                    Cam = Camera.main;
                }

                Cam.transform.position -=
                    Cam.ScreenToWorldPoint(deltaPosition) - Cam.ScreenToWorldPoint(Vector2.zero);
            }
        }

        private void OnPinch(
            Vector2 center,
            float oldDistance,
            float newDistance,
            Vector2 touchDelta
        )
        {
            if (ControlCamera && cameraControlEnabled)
            {
                if (Cam == null)
                {
                    Cam = Camera.main;
                }

                if (Cam.orthographic)
                {
                    Vector3 currentPinchPosition = Cam.ScreenToWorldPoint(center);

                    Cam.orthographicSize = Mathf.Max(
                        0.1f,
                        Cam.orthographicSize * oldDistance / newDistance
                    );

                    Vector3 newPinchPosition = Cam.ScreenToWorldPoint(center);

                    Cam.transform.position -= newPinchPosition - currentPinchPosition;
                }
                else
                {
                    Cam.fieldOfView = Mathf.Clamp(
                        Cam.fieldOfView * oldDistance / newDistance,
                        0.1f,
                        179.9f
                    );
                }
            }
        }

        /// <summary> Checks if the the current input is over canvas UI </summary>
        public bool IsPointerOverUIObject()
        {
            if (EventSystem.current == null)
            {
                return false;
            }

            PointerEventData eventDataCurrentPosition =
                new(EventSystem.current)
                {
                    position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
                };
            List<RaycastResult> results = new();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        }

        /// <summary> Cancels camera movement for the current motion. Resets to use camera at the end of the touch motion.</summary>
        public void CancelCamera()
        {
            cameraControlEnabled = false;
        }

        private void CameraInBounds()
        {
            if (ControlCamera && UseBounds && Cam != null && Cam.orthographic)
            {
                Cam.orthographicSize = Mathf.Min(
                    Cam.orthographicSize,
                    ((BoundMaxY - BoundMinY) / 2) - 0.001f
                );
                Cam.orthographicSize = Mathf.Min(
                    Cam.orthographicSize,
                    (Screen.height * (BoundMaxX - BoundMinX) / (2 * Screen.width)) - 0.001f
                );

                Vector2 margin =
                    Cam.ScreenToWorldPoint(
                        (Vector2.up * Screen.height / 2) + (Vector2.right * Screen.width / 2)
                    ) - Cam.ScreenToWorldPoint(Vector2.zero);

                float marginX = margin.x;
                float marginY = margin.y;

                float camMaxX = BoundMaxX - marginX;
                float camMaxY = BoundMaxY - marginY;
                float camMinX = BoundMinX + marginX;
                float camMinY = BoundMinY + marginY;

                float camX = Mathf.Clamp(Cam.transform.position.x, camMinX, camMaxX);
                float camY = Mathf.Clamp(Cam.transform.position.y, camMinY, camMaxY);

                Cam.transform.position = new Vector3(camX, camY, Cam.transform.position.z);
            }
        }
    }
}

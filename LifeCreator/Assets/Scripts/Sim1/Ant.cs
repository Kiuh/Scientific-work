using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Scripts.Sim1
{
    public enum AntState
    {
        SearchingFood,
        RandomMoving,
        SearchingHome
    }

    public enum MarkerType
    {
        PathToFood,
        PathToHome
    }

    [SelectionBase]
    public class Ant : MonoBehaviour
    {
        [Header("Main Properties")]
        [SerializeField]
        private AntState antState = AntState.SearchingFood;

        [SerializeField]
        [InspectorReadOnly]
        private Marker previousProducedMarker = null;

        [SerializeField]
        [InspectorReadOnly]
        private Home home;
        public Home Home
        {
            get => home;
            set => home = value;
        }

        [SerializeField]
        [InspectorReadOnly]
        private string antName;

        [Header("About markers")]
        [SerializeField]
        private Marker markerPrototype;

        [Min(0)]
        [SerializeField]
        private float markerSpawnInterval;

        [Min(0)]
        [SerializeField]
        private float markersDetectionRadius;

        [SerializeField]
        [InspectorReadOnly]
        private float timeToSpawnMarker;

        [Header("FOOD")]
        [Min(0)]
        [SerializeField]
        private float foodGrabDistance;

        [Min(0)]
        [SerializeField]
        private float foodDetectionRadius;

        [SerializeField]
        private Vector3 foodDetectionRadiusDelta;

        [SerializeField]
        private float haltDistance = 0.2f;

        [SerializeField]
        [InspectorReadOnly]
        private Food haltFood = null;

        [Header("FOOD marker")]
        [Min(0)]
        [SerializeField]
        private float foodMarkerLifeTime;

        [Header("HOME marker")]
        [Min(0)]
        [SerializeField]
        [InspectorReadOnly]
        private float homeMarkerLifeTime;

        [Header("About moving")]
        [SerializeField]
        [InspectorReadOnly]
        private Vector3 walkDirection;

        [Min(0)]
        [SerializeField]
        private float walkSpeed;

        [Range(0, 1)]
        [SerializeField]
        private float rotationSpeed;

        [Range(0, 5)]
        [SerializeField]
        private float randomDirectionDelta;

        [Range(0, 1)]
        [SerializeField]
        private float randomRotationSpeed;

        [Range(0, 1)]
        [SerializeField]
        private float centerMassCorrectionSpeed;

        [SerializeField]
        [InspectorReadOnly]
        private Vector3? centerDirection = null;

        [Header("Wall Avoid")]
        [SerializeField]
        private Transform leftDetectionPoint;

        [SerializeField]
        private Transform rightDetectionPoint;

        public event Action<Marker> OnMarkerCreated;

        public bool ShowWallAvoidGizmo { get; set; } = true;
        public bool ShowFoodDetectionArea { get; set; } = true;
        public bool ShowMarkersDetectionArea { get; set; } = true;
        public bool ShowMovementVectors { get; set; } = true;

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.green;
            if (ShowMarkersDetectionArea)
            {
                UnityEditor.Handles.DrawWireDisc(
                    transform.position,
                    Vector3.back,
                    markersDetectionRadius
                );
            }
            UnityEditor.Handles.color = Color.yellow;
            if (ShowFoodDetectionArea)
            {
                UnityEditor.Handles.DrawWireDisc(
                    transform.position + foodDetectionRadiusDelta,
                    Vector3.back,
                    foodDetectionRadius
                );
            }
            if (ShowMovementVectors)
            {
                UnityEditor.Handles.DrawLine(
                    transform.position,
                    transform.position + walkDirection
                );
            }
            UnityEditor.Handles.color = Color.blue;
            if (ShowMovementVectors && centerDirection != null)
            {
                UnityEditor.Handles.DrawLine(
                    transform.position,
                    transform.position + centerDirection.Value
                );
            }
            if (ShowWallAvoidGizmo)
            {
                UnityEditor.Handles.DrawWireDisc(leftDetectionPoint.position, Vector3.back, 0.05f);
                UnityEditor.Handles.DrawWireDisc(rightDetectionPoint.position, Vector3.back, 0.05f);
            }
            UnityEditor.Handles.color = Color.red;
            foreach (Vector3 item in intersectPointsGizmo)
            {
                UnityEditor.Handles.DrawWireDisc(item, Vector3.back, 0.05f);
            }
        }

        private void Awake()
        {
            walkDirection = Vector3.up.RotateZWithDegrees(UnityEngine.Random.Range(0f, 360f));
            antName = Guid.NewGuid().ToString();
        }

        private void Update()
        {
            WorkWithFood();
            ProduceMarker();
            CorrectDirection();
            ApplyRandomRotation();
            Move();
            AvoidWalls();
        }

        private void WorkWithFood()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                transform.position + foodDetectionRadiusDelta,
                foodDetectionRadius
            );
            if (antState == AntState.SearchingHome)
            {
                haltFood.transform.position =
                    transform.position + (walkDirection.normalized * haltDistance);
                if (
                    Vector3.Distance(
                        transform.position,
                        home.HomeZone.ClosestPoint(transform.position)
                    ) <= foodGrabDistance
                )
                {
                    Destroy(haltFood.gameObject);
                    haltFood = null;
                    antState = AntState.SearchingFood;
                }
            }
            if (antState == AntState.SearchingFood)
            {
                IEnumerable<Collider2D> availableSourcesOfFood = colliders.Where(
                    x =>
                        x.TryGetComponent(out FoodSource foodSource)
                        && Vector3.Distance(
                            transform.position,
                            foodSource.FoodSourceZone.ClosestPoint(transform.position)
                        ) <= foodGrabDistance
                );

                if (availableSourcesOfFood.Count() > 0)
                {
                    availableSourcesOfFood
                        .First()
                        .GetComponent<FoodSource>()
                        .GrabFood(out haltFood);
                    antState = AntState.SearchingHome;
                }
            }
        }

        private void ProduceMarker()
        {
            timeToSpawnMarker -= Time.deltaTime;
            if (timeToSpawnMarker <= 0)
            {
                if (antState == AntState.SearchingFood)
                {
                    SpawnMarker(MarkerType.PathToHome, homeMarkerLifeTime);
                }
                if (antState == AntState.SearchingHome)
                {
                    SpawnMarker(MarkerType.PathToFood, foodMarkerLifeTime);
                }
                timeToSpawnMarker = markerSpawnInterval;
            }
        }

        private void SpawnMarker(MarkerType markerType, float lifeTime)
        {
            Marker marker = Instantiate(markerPrototype, transform.position, new Quaternion());
            marker.SetMarkerType(markerType);
            marker.LifeTime = lifeTime;
            marker.AntName = antName;
            if (previousProducedMarker != null && previousProducedMarker.MarkerType == markerType)
            {
                previousProducedMarker.NextMarker = marker;
            }
            previousProducedMarker = marker;
            OnMarkerCreated?.Invoke(marker);
        }

        private void CorrectDirection()
        {
            centerDirection = FindPathDirection(
                home.Markers.Where(
                    x =>
                        x.MarkerType
                            == (
                                antState == AntState.SearchingFood
                                    ? MarkerType.PathToFood
                                    : MarkerType.PathToHome
                            )
                        && Vector3.Distance(x.transform.position, transform.position)
                            <= markersDetectionRadius
                )
            );

            if (centerDirection == null)
            {
                intersectPointsGizmo.Clear();
                return;
            }
            walkDirection = walkDirection.RotateZWithDegrees(
                Vector3.SignedAngle(walkDirection, centerDirection.Value, Vector3.forward)
                    * rotationSpeed
            );
        }

        private List<Vector3> intersectPointsGizmo = new();

        private Vector3? FindPathDirection(IEnumerable<Marker> homeMarkers)
        {
            IEnumerable<Marker> neededMarkers = homeMarkers.Where(x => x.HasNextMarker);

            if (neededMarkers.Count() == 0)
            {
                return null;
            }

            Vector3 vectorResult = Vector3.zero;
            foreach (Marker needMarker in neededMarkers)
            {
                vectorResult += needMarker.VectorToNextMarker;
            }

            Vector3 startPoint = Vector3.zero;
            foreach (Marker needMarker in neededMarkers)
            {
                startPoint += needMarker.transform.position;
            }
            startPoint /= neededMarkers.Count();

            Vector3? intersection = MathTools.IntersectRayCircle(
                startPoint,
                startPoint + vectorResult,
                transform.position,
                markersDetectionRadius
            );

            if (intersection == null)
            {
                return null;
            }

            intersectPointsGizmo = new List<Vector3>() { intersection.Value };

            return intersection.Value - transform.position;
        }

        private void ApplyRandomRotation()
        {
            if (centerDirection != null)
            {
                return;
            }

            float grad = UnityEngine.Random.Range(-randomDirectionDelta, randomDirectionDelta);
            float grad1 = UnityEngine.Random.Range(
                -randomDirectionDelta * 2,
                randomDirectionDelta * 2
            );
            if (grad * grad1 > 0)
            {
                walkDirection = Vector3.LerpUnclamped(
                    walkDirection,
                    walkDirection.RotateZWithDegrees(UnityEngine.Random.Range(grad, grad1)),
                    randomRotationSpeed
                );
            }
        }

        private void AvoidWalls()
        {
            bool leftWall = home.Walls.Any(x => x.IsPointInBounds(leftDetectionPoint.position));
            bool rightWall = home.Walls.Any(x => x.IsPointInBounds(rightDetectionPoint.position));

            if (leftWall && rightWall)
            {
                walkDirection = walkDirection.RotateZWithDegrees(180);
            }
        }

        private void Move()
        {
            transform.position = Vector3.Lerp(
                transform.position,
                transform.position + walkDirection.normalized,
                Time.deltaTime * walkSpeed
            );

            float angle =
                Mathf.Atan2(walkDirection.normalized.y, walkDirection.normalized.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }
    }
}

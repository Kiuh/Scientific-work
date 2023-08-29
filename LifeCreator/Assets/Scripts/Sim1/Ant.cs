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
        SearchingHome
    }

    public enum MarkerType
    {
        PathToFood,
        PathToHome
    }

    [Serializable]
    public struct AntConfig
    {
        [Min(0)]
        public float MarkerSpawnInterval;

        [Min(0)]
        public float WalkSpeed;

        [Range(0, 1)]
        public float RotationSpeed;

        [Range(0, 5)]
        public float RandomRotationDelta;

        [Min(0)]
        public float DetectionRadius;

        [Min(0)]
        public float FoodMarkerLifeTime;

        [Min(0)]
        public float HomeMarkerLifeTime;

        [Min(0)]
        public float FoodGrabDistance;

        [Range(0, 1)]
        public float CenterMassCorrectionSpeed;
    }

    public class Ant : MonoBehaviour
    {
        [SerializeField]
        private Marker markerPrototype;

        [SerializeField]
        [InspectorReadOnly]
        private float markerSpawnInterval;
        public float MarkerSpawnInterval
        {
            get => markerSpawnInterval;
            set => markerSpawnInterval = value;
        }

        [SerializeField]
        [InspectorReadOnly]
        private float timeToSpawnMarker;

        [SerializeField]
        [InspectorReadOnly]
        private Vector3 walkDirection;
        public Vector3 WalkDirection
        {
            get => walkDirection;
            set => walkDirection = value;
        }

        [SerializeField]
        [InspectorReadOnly]
        private float walkSpeed;
        public float WalkSpeed
        {
            get => walkSpeed;
            set => walkSpeed = value;
        }

        [SerializeField]
        [InspectorReadOnly]
        private float rotationSpeed;
        public float RotationSpeed
        {
            get => rotationSpeed;
            set => rotationSpeed = value;
        }

        [SerializeField]
        [InspectorReadOnly]
        private float detectionRadius;
        public float DetectionRadius
        {
            get => detectionRadius;
            set => detectionRadius = value;
        }

        [SerializeField]
        [InspectorReadOnly]
        private AntState antState = AntState.SearchingFood;
        public AntState AntState
        {
            get => antState;
            set => antState = value;
        }

        [SerializeField]
        [InspectorReadOnly]
        private float randomDirectionDelta;
        public float RandomDirectionDelta
        {
            get => randomDirectionDelta;
            set => randomDirectionDelta = value;
        }

        [SerializeField]
        [InspectorReadOnly]
        private float foodGrabDistance;
        private Food haltFood = null;
        private float haltDistance = 0.2f;
        private float foodMarkerLifeTime;
        private float homeMarkerLifeTime;

        public void SetAntConfig(AntConfig config)
        {
            AntState = AntState.SearchingFood;
            DetectionRadius = config.DetectionRadius;
            MarkerSpawnInterval = config.MarkerSpawnInterval;
            RotationSpeed = config.RotationSpeed;
            WalkSpeed = config.WalkSpeed;
            DetectionRadius = config.DetectionRadius;
            foodMarkerLifeTime = config.FoodMarkerLifeTime;
            homeMarkerLifeTime = config.HomeMarkerLifeTime;
            timeToSpawnMarker = MarkerSpawnInterval;
            foodGrabDistance = config.FoodGrabDistance;
            RandomDirectionDelta = config.RandomRotationDelta;
            centerMassCorrectionSpeed = config.CenterMassCorrectionSpeed;
        }

        private Vector3 centerDirection = Vector3.zero;

        private void OnDrawGizmos()
        {
            UnityEditor.Handles.color = Color.yellow;
            UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, detectionRadius);
            UnityEditor.Handles.DrawLine(transform.position, transform.position + walkDirection);
            UnityEditor.Handles.color = Color.blue;
            UnityEditor.Handles.DrawLine(transform.position, transform.position + centerDirection);
        }

        private void Update()
        {
            WorkWithFood();
            ProduceMarker();
            CorrectDirection();
            ApplyRandomRotation();
            Move();
        }

        private void WorkWithFood()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                transform.position,
                detectionRadius
            );
            if (AntState == AntState.SearchingHome)
            {
                haltFood.transform.position =
                    transform.position + (walkDirection.normalized * haltDistance);
                if (
                    colliders.Any(
                        x =>
                            x.TryGetComponent(out Home home)
                            && Vector3.Distance(transform.position, home.transform.position)
                                <= foodGrabDistance
                    )
                )
                {
                    Destroy(haltFood.gameObject);
                    haltFood = null;
                    AntState = AntState.SearchingFood;
                }
            }
            else if (AntState == AntState.SearchingFood)
            {
                IEnumerable<Collider2D> availableFood = colliders.Where(
                    x =>
                        x.TryGetComponent(out Food food)
                        && Vector3.Distance(transform.position, food.transform.position)
                            <= foodGrabDistance
                        && !food.Grabbed
                );

                if (availableFood.Count() > 0)
                {
                    haltFood = availableFood.First().GetComponent<Food>();
                    haltFood.Grabbed = true;
                    AntState = AntState.SearchingHome;
                }
            }
        }

        private void ProduceMarker()
        {
            timeToSpawnMarker -= Time.deltaTime;
            if (timeToSpawnMarker <= 0)
            {
                if (AntState == AntState.SearchingFood)
                {
                    SpawnMarker(MarkerType.PathToHome, homeMarkerLifeTime);
                }
                if (AntState == AntState.SearchingHome)
                {
                    SpawnMarker(MarkerType.PathToFood, foodMarkerLifeTime);
                }
                timeToSpawnMarker = MarkerSpawnInterval;
            }
        }

        private void SpawnMarker(MarkerType markerType, float lifeTime)
        {
            Marker marker = Instantiate(markerPrototype, transform.position, new Quaternion());
            marker.SetMarkerType(markerType);
            marker.LifeTime = lifeTime;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Wall _))
            {
                walkDirection = -walkDirection;
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.TryGetComponent(out Wall _))
            {
                walkDirection = walkDirection.RotateZWithDegrees(4);
            }
        }

        private float centerMassCorrectionSpeed;

        private void CorrectDirection()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(
                transform.position,
                detectionRadius
            );
            List<Marker> markers = new();
            MarkerType searchMarker =
                AntState == AntState.SearchingFood ? MarkerType.PathToFood : MarkerType.PathToHome;
            foreach (Collider2D collider in colliders)
            {
                if (collider.TryGetComponent(out Marker marker))
                {
                    if (marker.MarkerType == searchMarker)
                    {
                        markers.Add(marker);
                    }
                }
            }
            if (markers.Count == 0)
            {
                centerDirection = Vector3.zero;
                return;
            }
            float totalMass = 0;
            float totalX = 0;
            float totalY = 0;
            foreach (Marker marker in markers)
            {
                totalMass += marker.Power;
                totalX += marker.transform.position.x * marker.Power;
                totalY += marker.transform.position.y * marker.Power;
            }
            Vector3 center = new(totalX / totalMass, totalY / totalMass, 0);

            centerDirection = Vector3.Lerp(
                centerDirection,
                (center - transform.position).normalized,
                centerMassCorrectionSpeed * Time.deltaTime
            );

            walkDirection = walkDirection.RotateZWithDegrees(
                Vector3.SignedAngle(
                    walkDirection,
                    (center - transform.position).normalized,
                    Vector3.forward
                ) * rotationSpeed
            );
        }

        private void ApplyRandomRotation()
        {
            float grad = UnityEngine.Random.Range(-randomDirectionDelta, randomDirectionDelta);
            float grad1 = UnityEngine.Random.Range(
                -randomDirectionDelta * 2,
                randomDirectionDelta * 2
            );
            if (grad * grad1 > 0)
            {
                walkDirection = walkDirection.RotateZWithDegrees(
                    UnityEngine.Random.Range(grad, grad1)
                );
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

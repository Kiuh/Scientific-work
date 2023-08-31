using Common;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Sim1
{
    public class Home : MonoBehaviour
    {
        [SerializeField]
        private Transform markersParent;

        [SerializeField]
        private Collider2D homeZone;
        public Collider2D HomeZone => homeZone;

        [SerializeField]
        private Ant antPrototype;

        [SerializeField]
        private float antSpawnInterval;

        [SerializeField]
        private int startCount;

        [SerializeField]
        [InspectorReadOnly]
        private float timeToSpawn = 0;

        [SerializeField]
        private List<Ant> ants = new();
        public IEnumerable<Ant> Ants => ants;

        [SerializeField]
        private List<Marker> markers = new();
        public IEnumerable<Marker> Markers => markers;

        [SerializeField]
        private List<FoodSource> foodSources = new();
        public IEnumerable<FoodSource> FoodSources => foodSources;

        [SerializeField]
        private bool showWallAvoidGizmo;

        [SerializeField]
        private bool showFoodDetectionArea;

        [SerializeField]
        private bool showMarkersDetectionArea;

        [SerializeField]
        private bool showMovementVectors;

        [SerializeField]
        private bool showPathToNextMarker;

        private void Start()
        {
            for (int i = 0; i < startCount; i++)
            {
                SpawnAnt();
            }
        }

        private void Update()
        {
            timeToSpawn -= Time.deltaTime;
            if (timeToSpawn <= 0)
            {
                SpawnAnt();
            }
        }

        private void SpawnAnt()
        {
            Ant ant = Instantiate(antPrototype, transform.position, new Quaternion(), transform);
            ants.Add(ant);
            ant.OnMarkerCreated += (marker) =>
            {
                marker.transform.SetParent(markersParent);
                markers.Add(marker);
                marker.ShowPathToNextMarker = showPathToNextMarker;
                marker.OnDestroyEvent += () => markers.Remove(marker);
            };
            timeToSpawn = antSpawnInterval;
            ant.Home = this;

            ant.ShowWallAvoidGizmo = showWallAvoidGizmo;
            ant.ShowMovementVectors = showMovementVectors;
            ant.ShowFoodDetectionArea = showFoodDetectionArea;
            ant.ShowMarkersDetectionArea = showMarkersDetectionArea;
        }

        private void OnValidate()
        {
            foreach (Ant ant in ants)
            {
                ant.ShowWallAvoidGizmo = showWallAvoidGizmo;
                ant.ShowMovementVectors = showMovementVectors;
                ant.ShowFoodDetectionArea = showFoodDetectionArea;
                ant.ShowMarkersDetectionArea = showMarkersDetectionArea;
            }
            foreach (Marker marker in markers)
            {
                marker.ShowPathToNextMarker = showPathToNextMarker;
            }
        }
    }
}

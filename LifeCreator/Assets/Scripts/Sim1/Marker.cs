using Common;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Scripts.Sim1
{
    [Serializable]
    public struct MarkerTypeColor
    {
        public MarkerType MarkerType;
        public Color Color;
    }

    public class Marker : MonoBehaviour
    {
        [SerializeField]
        [InspectorReadOnly]
        private MarkerType markerType;
        public MarkerType MarkerType => markerType;

        [SerializeField]
        [InspectorReadOnly]
        private string antName;
        public string AntName
        {
            get => antName;
            set => antName = value;
        }

        [SerializeField]
        [InspectorReadOnly]
        private Marker nextMarker = null;
        public Marker NextMarker
        {
            get => nextMarker;
            set
            {
                nextMarker = value;
                vectorToNextMarker = transform.position - nextMarker.transform.position;
                HasNextMarker = true;
            }
        }
        public bool HasNextMarker { get; private set; } = false;

        [SerializeField]
        [InspectorReadOnly]
        private Vector3 vectorToNextMarker = Vector3.zero;
        public Vector3 VectorToNextMarker => vectorToNextMarker;

        [SerializeField]
        [InspectorReadOnly]
        private float lifeTime;
        public float LifeTime
        {
            get => lifeTime;
            set => lifeTime = value;
        }

        [SerializeField]
        [InspectorReadOnly]
        private float timeLived = 0;

        [SerializeField]
        private List<MarkerTypeColor> markerTypeColors;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        private Vector3 startLocalScale;

        public event Action OnDestroyEvent;

        public bool ShowPathToNextMarker = true;

        public float Power => (lifeTime - timeLived) / lifeTime;

        public void OnDrawGizmos()
        {
            if (ShowPathToNextMarker && nextMarker != null)
            {
                UnityEditor.Handles.DrawLine(transform.position, nextMarker.transform.position);
            }
        }

        public void Awake()
        {
            startLocalScale = transform.localScale;
            spriteRenderer.color = markerTypeColors.Find(x => x.MarkerType == markerType).Color;
        }

        public void SetMarkerType(MarkerType markerType)
        {
            this.markerType = markerType;
        }

        private void Update()
        {
            timeLived += Time.deltaTime;
            transform.localScale = startLocalScale * Power;
            if (timeLived > lifeTime)
            {
                Destroy(gameObject);
            }
        }

        private void OnDestroy()
        {
            OnDestroyEvent?.Invoke();
        }
    }
}

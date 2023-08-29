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

        public float Power => (lifeTime - timeLived) / lifeTime;

        public void SetMarkerType(MarkerType markerType)
        {
            this.markerType = markerType;
            spriteRenderer.color = markerTypeColors.Find(x => x.MarkerType == markerType).Color;
        }

        private void Update()
        {
            timeLived += Time.deltaTime;
            Color color = spriteRenderer.color;
            spriteRenderer.color = new Color(color.r, color.g, color.b, Power);
            if (timeLived > lifeTime)
            {
                Destroy(gameObject);
            }
        }
    }
}

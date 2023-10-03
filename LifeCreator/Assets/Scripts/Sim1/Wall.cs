using UnityEngine;

namespace Scripts.Sim1
{
    public class Wall : MonoBehaviour
    {
        [SerializeField]
        private Transform leftDownPoint;

        [SerializeField]
        private Transform rightUpPoint;

        public bool IsPointInBounds(Vector2 point)
        {
            return point.x > leftDownPoint.position.x
                && point.x < rightUpPoint.position.x
                && point.y > leftDownPoint.position.y
                && point.y < rightUpPoint.position.y;
        }
    }
}

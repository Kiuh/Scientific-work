using UnityEngine;

namespace Common
{
    public static class Vector3Tools
    {
        public static Vector3 RotateZWithDegrees(this Vector3 vec, float angle)
        {
            if (angle == 0)
            {
                return vec;
            }

            float newAngle = Mathf.Atan2(vec.y, vec.x) + (angle * Mathf.Deg2Rad);
            return new Vector3(Mathf.Cos(newAngle), Mathf.Sin(newAngle), vec.z);
        }
    }
}

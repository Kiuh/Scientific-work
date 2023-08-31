using System;
using UnityEngine;

namespace Common
{
    public class MathTools
    {

        public static Vector3? IntersectRayCircle(
            Vector3 rayStart,
            Vector3 rayPoint,
            Vector3 circlePosition,
            float circleRadiusSquared
        )
        {
            if (rayStart == rayPoint || circleRadiusSquared <= 0)
            {
                return null;
            }

            Vector3 nearest = GetNearestPoint(circlePosition, rayStart, rayPoint, false, false);
            float distanceSquared = (nearest - circlePosition).sqrMagnitude;

            if (distanceSquared > circleRadiusSquared)
            {
                return null;
            }

            Vector3 offset =
                Vector3.Normalize(rayPoint - rayStart)
                * (float)Math.Sqrt(circleRadiusSquared - distanceSquared);

            return (circlePosition - rayStart).sqrMagnitude < circleRadiusSquared
                ? nearest + offset
                : nearest - offset;
        }

        public static Vector3 GetNearestPoint(
            Vector3 location,
            Vector3 segmentStart,
            Vector3 segmentEnd,
            bool trimStart,
            bool trimEnd
        )
        {
            if (segmentStart == segmentEnd)
            {
                throw new ArgumentException("segmentStart cannot be equal to segmentEnd.");
            }

            Vector3 AP = location - segmentStart;
            Vector3 AB = segmentEnd - segmentStart;

            float magnitudeAB = AB.sqrMagnitude;
            float distance = Vector3.Dot(AP, AB) / magnitudeAB;

            return (distance < 0 && trimStart)
                ? segmentStart
                : (distance > 1 && trimEnd)
                    ? segmentEnd
                    : segmentStart + (AB * distance);
        }
    }
}

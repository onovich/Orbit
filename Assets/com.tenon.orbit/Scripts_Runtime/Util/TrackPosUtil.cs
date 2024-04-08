using System.Collections.Generic;
using UnityEngine;
using MortiseFrame.Swing;

namespace TenonKit.Orbit {

    public static class TrackPosUtil {

        public static Vector2 CalculateNextPoint(TrackShape trackShape, Vector2 startPos, Vector2 endPos, float t, Vector2 controlPoint1, Vector2 controlPoint2) {
            switch (trackShape) {
                case TrackShape.Line:
                    return LinearInterpolation(startPos, endPos, t);
                case TrackShape.Bezier:
                    return SplineHelper.Easing(startPos, controlPoint1, controlPoint2, endPos, t, 1, SplineType.Bezier);
                default:
                    return Vector2.zero;
            }
        }

        private static Vector2 LinearInterpolation(Vector2 startPos, Vector2 endPos, float t) {
            return Vector2.Lerp(startPos, endPos, t);
        }

    }

}
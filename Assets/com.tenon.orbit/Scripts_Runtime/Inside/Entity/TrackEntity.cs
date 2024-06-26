using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Orbit {

    internal class TrackEntity {

        // ID
        int id;
        internal int ID => id;

        // Node
        TrackNodeComponent pathNodeComponent;

        // Attr
        float speed;
        TrackLoopType loopType;
        TrackShape trackShape;
        Vector2 controlPoint1;
        Vector2 controlPoint2;
        int splineAccuracy; // 曲线精细度, 用于计算曲线长度

        // State
        Vector2 carPos;
        internal Vector2 CarPos => carPos;
        Vector2 carLastFramePos;
        int direction;
        bool isEnd;

        // Timer
        float durationSec;
        float currentSec;

        // Event
        internal event Action<int> OnArriveOnceHandle;
        internal event Action OnArriveEndHandle;

        // Init
        internal void SetID(int id) {
            this.id = id;
        }
        internal void SetNode(Vector2[] nodeArr, int originalIndex) {
            pathNodeComponent = new TrackNodeComponent(nodeArr, originalIndex);
            if (nodeArr.Length <= 1) {
                isEnd = true;
            }
        }

        internal void SetDirection(int direction) {
            this.direction = direction;
        }

        internal void SetLoopType(TrackLoopType loopType) {
            this.loopType = loopType;
        }

        internal void SetTrackShape(TrackShape trackShape) {
            this.trackShape = trackShape;
        }

        internal void SetControlPoint(Vector2 controlPoint1, Vector2 controlPoint2) {
            this.controlPoint1 = controlPoint1;
            this.controlPoint2 = controlPoint2;
        }

        internal void SetSplineAccuracy(int splineAccuracy) {
            this.splineAccuracy = splineAccuracy;
        }

        internal void SetSpeed(float speed) {
            this.speed = speed;
        }

        internal void InitTimer() {
            var currentIndex = pathNodeComponent.CurrentIndex;
            var nextIndex = pathNodeComponent.GetNextIndex(loopType, ref direction);
            ReCalculateTimer(pathNodeComponent.CurrentIndex, nextIndex);
        }

        // Tick
        internal void Tick(float dt) {
            if (isEnd) {
                return;
            }
            if (currentSec >= durationSec) {
                Arrive();
            }
            ApplyCarMove(dt);
        }

        void ApplyCarMove(float dt) {
            if (isEnd) {
                return;
            }
            var startPos = pathNodeComponent.CurrentNode;
            var nextIndex = pathNodeComponent.GetNextIndex(loopType, ref direction);
            var endPos = pathNodeComponent.GetNode(nextIndex);
            currentSec += dt;
            var t = currentSec / durationSec;
            var currentPos = TrackUtil.CalculateNextPoint(trackShape, startPos, endPos, t, controlPoint1, controlPoint2);
            carLastFramePos = carPos;
            carPos = currentPos;
        }

        // Offset
        internal Vector2 GetOffset() {
            return carPos - carLastFramePos;
        }

        // Arrive
        void Arrive() {
            pathNodeComponent.PassCurrent(loopType, ref direction, () => {
                OnArriveEndHandle?.Invoke();
                isEnd = true;
            });
            var nextIndex = pathNodeComponent.GetNextIndex(loopType, ref direction);
            var currentIndex = pathNodeComponent.CurrentIndex;
            ReCalculateTimer(currentIndex, nextIndex);
            OnArriveOnceHandle?.Invoke(currentIndex);
        }

        // Timer
        void ReCalculateTimer(int currentIndex, int nextIndex) {
            currentSec = 0;
            var currentNode = pathNodeComponent.GetNode(currentIndex);
            var nextNode = pathNodeComponent.GetNode(nextIndex);
            var dis = TrackUtil.CalculateDistance(trackShape, currentNode, nextNode, controlPoint1, controlPoint2, splineAccuracy);
            durationSec = dis / speed;
        }

        // Gizmos
        void OnDrawGizmos() {
            if (pathNodeComponent == null) {
                return;
            }
            pathNodeComponent.ForEach((node) => {
                Gizmos.color = Color.green;
                var nextIndex = pathNodeComponent.GetNextIndex(loopType, ref direction);
                var nextNode = pathNodeComponent.GetNode(nextIndex);
                Gizmos.DrawLine(node, nextNode);
            });
        }
    }

}
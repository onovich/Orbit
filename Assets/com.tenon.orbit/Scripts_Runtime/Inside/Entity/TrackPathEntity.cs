using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Orbit {

    public class TrackPath {
        TrackPathNodeComponent pathNodeComponent;
        bool isLoop;
        float speed;

        Vector2 carPos;
        public Vector2 CarPos => carPos;
        int direction;
        float durationSec;
        float currentSec;

        bool isEnd;

        TrackLoopType loopType;

        public void Ctor(int noodCount, int originalIndex, int originalDirection, TrackLoopType loopType, float speed) {
            direction = originalDirection;
            this.loopType = loopType;
            this.speed = speed;
            pathNodeComponent = new TrackPathNodeComponent(noodCount, originalIndex);
            isEnd = false;
            var nextIndex = pathNodeComponent.GetNextIndex(loopType, ref direction);
            if (nextIndex == pathNodeComponent.CurrentIndex) {
                isEnd = true;
            }
            ReCalculateTimer(pathNodeComponent.CurrentIndex, nextIndex);
        }

        public void ReCalculateTimer(int currentIndex, int nextIndex) {
            currentSec = 0;
            var currentPos = pathNodeComponent.GetNode(currentIndex);
            var nextPos = pathNodeComponent.GetNode(nextIndex);
            var dis = Vector2.Distance(currentPos, nextPos);
            durationSec = dis / speed;
        }

        public void TickCarMove(float dt) {
            if (isEnd) {
                return;
            }
            if (currentSec >= durationSec) {
                ArriveTarget();
            }
            var startPos = pathNodeComponent.CurrentNode;
            var nextIndex = pathNodeComponent.GetNextIndex(loopType, ref direction);
            if (nextIndex == pathNodeComponent.CurrentIndex) {
                isEnd = true;
                return;
            }
            var endPos = pathNodeComponent.GetNode(nextIndex);
            currentSec += dt;
            var currentPos = Vector2.Lerp(startPos, endPos, currentSec / durationSec);
            carPos = currentPos;
        }

        void ArriveTarget() {
            pathNodeComponent.PassCurrent(ref direction);
            var nextIndex = pathNodeComponent.GetNextIndex(loopType, ref direction);
            ReCalculateTimer(pathNodeComponent.CurrentIndex, nextIndex);
        }

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
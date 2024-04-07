using System;
using UnityEngine;

namespace TenonKit.Orbit {

    internal class TrackNodeComponent {

        Vector2[] nodeArr;
        int currentIndex;
        internal int CurrentIndex => currentIndex;
        internal Vector2 CurrentNode => nodeArr[currentIndex];

        internal TrackNodeComponent(Vector2[] nodeArr, int originalIndex) {
            this.nodeArr = nodeArr;
            currentIndex = originalIndex;
        }

        internal Vector2 GetNode(int index) {
            return nodeArr[index];
        }

        internal void ForEach(Action<Vector2> action) {
            foreach (var node in nodeArr) {
                action(node);
            }
        }

        internal void PassCurrent(TrackLoopType loopType, ref int direction, Action action) {
            var nextIndex = GetNextIndex(loopType, ref direction);
            currentIndex = nextIndex;
            nextIndex = GetNextIndex(loopType, ref direction);
            if (nextIndex == currentIndex) {
                action?.Invoke();
            }
        }

        internal int GetNextIndex(TrackLoopType loopType, ref int direction) {
            var nextIndex = 0;
            if (currentIndex >= nodeArr.Length - 1 || currentIndex < 0) {
                if (loopType == TrackLoopType.PingPong) {
                    direction *= -1;
                    nextIndex = currentIndex + 1 * direction;
                }
                if (loopType == TrackLoopType.Restart) {
                    nextIndex = 0;
                }
                if (loopType == TrackLoopType.None) {
                    nextIndex = currentIndex;
                }
            } else {
                nextIndex = currentIndex + 1 * direction;
            }
            return nextIndex;
        }

    }

}
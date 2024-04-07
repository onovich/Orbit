using System;
using UnityEngine;

namespace TenonKit.Orbit {

    public class TrackPathNodeComponent {

        Vector2[] nodeArr;
        int currentIndex;
        public int CurrentIndex => currentIndex;
        public Vector2 CurrentNode => nodeArr[currentIndex];

        public TrackPathNodeComponent(int nodeCount, int originalIndex) {
            nodeArr = new Vector2[nodeCount];
            currentIndex = originalIndex;
        }

        public Vector2 GetNode(int index) {
            return nodeArr[index];
        }

        public void ForEach(Action<Vector2> action) {
            foreach (var node in nodeArr) {
                action(node);
            }
        }

        public void PassCurrent(ref int direction) {
            var nextIndex = GetNextIndex(TrackLoopType.Restart, ref direction);
            currentIndex = nextIndex;
        }

        public int GetNextIndex(TrackLoopType loopType, ref int direction) {
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
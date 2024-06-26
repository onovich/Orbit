using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Orbit {

    public class TrackCore {

        TrackCoreContext ctx;

        public TrackCore() {
            ctx = new TrackCoreContext();
        }

        public int CreateTrack(Vector2[] nodeArr, int originalIndex, float speed, int originalDirection, TrackLoopType loopType, TrackShape trackShape, Vector2? controlPoint1 = null, Vector2? controlPoint2 = null) {
            return TrackFactory.SpawnTrack(ctx, nodeArr, speed, originalIndex, originalDirection, loopType, trackShape, controlPoint1, controlPoint2);
        }

        public bool TryGetTrackPos(int id, out Vector2 pos) {
            var succ = ctx.TryGetTrack(id, out var track);
            if (!succ) {
                pos = Vector2.zero;
                return false;
            }
            pos = track.CarPos;
            return true;
        }

        public bool TryGetTrackOffset(int id, out Vector2 offset) {
            var succ = ctx.TryGetTrack(id, out var track);
            if (!succ) {
                offset = Vector2.zero;
                return false;
            }
            offset = track.GetOffset();
            return true;
        }

        public void Tick(float dt) {
            ctx.ForEach(track => {
                track.Tick(dt);
            });
        }

        public void TearDown() {
            ctx.Clear();
        }

    }

}
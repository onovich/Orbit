using UnityEngine;

namespace TenonKit.Orbit {

    internal static class TrackFactory {

        internal static int SpawnTrack(TrackCoreContext ctx, Vector2[] nodeArr, float speed, int originalIndex, int originalDirection, TrackLoopType loopType, TrackShape trackShape, Vector2 controlPoint1, Vector2 controlPoint2) {
            var track = new TrackEntity();
            var id = ctx.IDService.PickTrackID();
            track.SetID(id);
            track.SetNode(nodeArr, originalIndex);
            track.SetDirection(originalDirection);
            track.SetLoopType(loopType);
            track.SetTrackShape(trackShape);
            track.SetControlPoint(controlPoint1, controlPoint2);
            track.SetSpeed(speed);
            track.InitTimer();
            ctx.AddTrack(track);
            return track.ID;
        }

        internal static void UnspawnTrack(TrackCoreContext ctx, int id) {
            var has = ctx.TryGetTrack(id, out var track);
            if (!has) {
                OLog.Error($"TrackFactory.UnspawnTrack: Track not found, id={id}");
                return;
            }
            ctx.RemoveTrack(id);
        }

    }

}
using UnityEngine;

namespace TenonKit.Orbit {

    internal static class TrackFactory {

        internal static int SpawnTrack(TrackCoreContext ctx, Vector2[] nodeArr, int originalIndex, int originalDirection, TrackLoopType loopType, float speed) {
            var track = new TrackEntity();
            var id = ctx.IDService.PickTrackID();
            track.SetID(id);
            track.SetNode(nodeArr, originalIndex);
            track.SetDirection(originalDirection);
            track.SetLoopType(loopType);
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
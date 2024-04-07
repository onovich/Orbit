using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TenonKit.Orbit {

    internal class TrackCoreContext {

        TrackIDService iDService;
        internal TrackIDService IDService => iDService;

        SortedList<int, TrackEntity> trackList;

        internal TrackCoreContext() {
            iDService = new TrackIDService();
            trackList = new SortedList<int, TrackEntity>();
        }

        internal void AddTrack(TrackEntity track) {
            trackList.Add(track.ID, track);
        }

        internal void RemoveTrack(int id) {
            trackList.Remove(id);
        }

        internal bool TryGetTrack(int id, out TrackEntity track) {
            return trackList.TryGetValue(id, out track);
        }

        internal void ForEach(Action<TrackEntity> action) {
            foreach (var track in trackList.Values) {
                action(track);
            }
        }

        internal void Clear() {
            trackList.Clear();
            iDService.Reset();
        }

    }

}
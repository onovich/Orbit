namespace TenonKit.Orbit {

    internal class TrackIDService {

        int trackIDRecord = 0;

        internal int PickTrackID() {
            return trackIDRecord++;
        }

        internal void Reset() {
            trackIDRecord = 0;
        }

    }

}
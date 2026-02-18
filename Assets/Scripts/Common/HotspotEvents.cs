using System;

namespace dev.xrlab.interactive3dviewer.common
{
    public static class HotspotEvents
    {
        public static Action<HotspotData> OnHotspotActivated;
        public static Action<HotspotData> OnCameraFocusReached;
        public static Action<HotspotData> OnHotspotExited;
    }
}
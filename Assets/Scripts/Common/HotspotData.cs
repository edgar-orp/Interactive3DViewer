using UnityEngine;

namespace dev.xrlab.interactive3dviewer.common
{
    public enum CameraRotationMode
    {
        Full,
        YawOnly,
        LookAt
    }

    [System.Serializable]
    public class HotspotData
    {
        public string id;
        public Transform focusPoint;
        public CameraRotationMode rotationMode = CameraRotationMode.Full;
    }
}
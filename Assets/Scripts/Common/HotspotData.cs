using UnityEngine;
using UnityEngine.Video;

namespace dev.xrlab.interactive3dviewer.common
{
    public enum CameraRotationMode
    {
        Full,
        YawOnly,
        LookAt
    }

    public enum HotspotContentType
    {
        TextOnly,
        Image,
        Video,
        TextAndImage,
        TextAndVideo
    }

    [System.Serializable]
    public class HotspotData
    {
        [Header("Identification")]
        public string id;

        [Header("Camera")]
        public Transform focusPoint;
        public CameraRotationMode rotationMode = CameraRotationMode.Full;

        [Header("Content Type")]
        public HotspotContentType contentType;

        [Header("Text Content")]
        public string title;

        [TextArea(5, 15)]
        public string description;

        [Header("Image Content")]
        public Sprite image;

        [Header("Video Content")]
        public VideoClip videoClip;
    }
}

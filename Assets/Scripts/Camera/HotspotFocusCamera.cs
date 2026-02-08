using UnityEngine;
using dev.xrlab.interactive3dviewer.common;

namespace dev.xrlab.interactive3dviewer.camera
{
    public class HotspotFocusCamera : MonoBehaviour
    {
        [SerializeField] private Camera targetCamera;
        [SerializeField] private float moveSpeed = 3f;

        private Transform focusPoint;
        private CameraRotationMode rotationMode;

        private void OnEnable()
        {
            HotspotEvents.OnHotspotActivated += OnHotspotActivated;
        }

        private void OnDisable()
        {
            HotspotEvents.OnHotspotActivated -= OnHotspotActivated;
        }

        private void OnHotspotActivated(HotspotData data)
        {
            focusPoint = data.focusPoint;
            rotationMode = data.rotationMode;
        }

        private void Update()
        {
            if (focusPoint == null) return;

            targetCamera.transform.position =
                Vector3.Lerp(
                    targetCamera.transform.position,
                    focusPoint.position,
                    Time.deltaTime * moveSpeed
                );

            Quaternion desiredRotation = GetDesiredRotation();

            targetCamera.transform.rotation =
                Quaternion.Lerp(
                    targetCamera.transform.rotation,
                    desiredRotation,
                    Time.deltaTime * moveSpeed
                );
        }

        private Quaternion GetDesiredRotation()
        {
            switch (rotationMode)
            {
                case CameraRotationMode.YawOnly:
                    Vector3 forward = focusPoint.forward;
                    forward.y = 0f;
                    return Quaternion.LookRotation(forward.normalized, Vector3.up);

                case CameraRotationMode.LookAt:
                    Vector3 dir = focusPoint.position - targetCamera.transform.position;
                    return Quaternion.LookRotation(dir.normalized, Vector3.up);

                default:
                    return focusPoint.rotation;
            }
        }
    }
}

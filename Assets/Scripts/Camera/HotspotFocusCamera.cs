using UnityEngine;
using dev.xrlab.interactive3dviewer.common;

namespace dev.xrlab.interactive3dviewer.camera
{
    public class HotspotFocusCamera : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Camera targetCamera;
        [SerializeField] private Transform anchorPoint;

        [Header("Movement")]
        [SerializeField] private float moveSpeed = 3f;
        [SerializeField] private float positionThreshold = 5f;
        [SerializeField] private float rotationThreshold = 10f;

        private Transform focusPoint;
        private CameraRotationMode rotationMode;
        private HotspotData currentData;

        private bool hasArrived;
        private bool returningToAnchor;

        private void OnEnable()
        {
            HotspotEvents.OnHotspotActivated += OnHotspotActivated;
            HotspotEvents.OnHotspotExited += OnHotspotExited;
        }

        private void OnDisable()
        {
            HotspotEvents.OnHotspotActivated -= OnHotspotActivated;
            HotspotEvents.OnHotspotExited -= OnHotspotExited;
        }

        private void OnHotspotActivated(HotspotData data)
        {
            focusPoint = data.focusPoint;
            rotationMode = data.rotationMode;
            currentData = data;

            hasArrived = false;
            returningToAnchor = false;
        }

        private void OnHotspotExited(HotspotData data)
        {
            if (anchorPoint == null) return;

            focusPoint = anchorPoint;
            rotationMode = CameraRotationMode.Full;

            hasArrived = false;
            returningToAnchor = true;
        }

        private void Update()
        {
            if (focusPoint == null) return;

            Transform cam = targetCamera.transform;

            cam.position = Vector3.Lerp(
                cam.position,
                focusPoint.position,
                Time.deltaTime * moveSpeed
            );

            Quaternion desiredRotation = GetDesiredRotation();

            cam.rotation = Quaternion.Lerp(
                cam.rotation,
                desiredRotation,
                Time.deltaTime * moveSpeed
            );

            if (!hasArrived && HasReachedTarget(desiredRotation))
            {
                hasArrived = true;

                if (!returningToAnchor)
                {
                    HotspotEvents.OnCameraFocusReached?.Invoke(currentData);
                }
                else
                {
                    focusPoint = null;
                    returningToAnchor = false;
                    currentData = null;
                }
            }
        }

        private bool HasReachedTarget(Quaternion desiredRotation)
        {
            float positionDistance =
                Vector3.Distance(targetCamera.transform.position, focusPoint.position);

            float rotationDifference =
                Quaternion.Angle(targetCamera.transform.rotation, desiredRotation);

            return positionDistance <= positionThreshold &&
                   rotationDifference <= rotationThreshold;
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
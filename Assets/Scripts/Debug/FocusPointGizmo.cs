using UnityEngine;

namespace dev.xrlab.interactive3dviewer.debug
{
    public class FocusPointGizmo : MonoBehaviour
    {
        [Header("Gizmo Scale")]
        [SerializeField] private float sphereRadius = 5f;
        [SerializeField] private float forwardLength = 20f;
        [SerializeField] private float arrowHeadSize = 10f;

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;

            Gizmos.DrawSphere(transform.position, sphereRadius);

            Vector3 start = transform.position;
            Vector3 end = start + transform.forward * forwardLength;
            Gizmos.DrawLine(start, end);

            Vector3 right = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, 150, 0) * Vector3.forward;
            Vector3 left = Quaternion.LookRotation(transform.forward) * Quaternion.Euler(0, -150, 0) * Vector3.forward;

            Gizmos.DrawLine(end, end + right * arrowHeadSize);
            Gizmos.DrawLine(end, end + left * arrowHeadSize);
        }
    }
}

using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

[ExecuteAlways]
public class ReticleHandler : MonoBehaviour
{
    [Header("Target")]
    public Transform target;                      // Object to follow

    [Header("Grounding")]
    public LayerMask groundMask = ~0;            // Set to your Terrain/Ground layers
    public float maxRayDistance = 200f;          // How far down we search for ground
    public float rayStartHeight = 5f;            // Start ray a bit above the target
    public float surfaceOffset = 0.02f;          // Prevent z-fighting

    [Header("Smoothing")]
    public float followSmoothing = 20f;          // Higher = snappier

    [Header("Decal")]
    public DecalProjector decalProjector;        // Assign your Decal Projector
    public bool alignToSurfaceNormal = true;     // True: hug slopes; False: straight down

    [Header("Spin")]
    [Tooltip("How fast the reticle rolls around its projection axis (deg/sec).")]
    public float spinDegreesPerSecond = 20f;     // Slow spin
    private float _spinAngle;                    // Accumulated roll angle

    void Update()
    {
        if (target == null || decalProjector == null) return;

        // Advance spin angle (independent of smoothing for steady rotation)
        _spinAngle = (_spinAngle + spinDegreesPerSecond * Time.deltaTime) % 360f;

        // Raycast straight down from above the target
        Vector3 rayOrigin = target.position + Vector3.up * rayStartHeight;
        if (Physics.Raycast(rayOrigin, Vector3.down, out RaycastHit hit, maxRayDistance + rayStartHeight, groundMask))
        {
            // Target pose
            Vector3 desiredPos = hit.point + hit.normal * surfaceOffset;

            // In HDRP, Decal Projector projects along its forward.
            Quaternion baseRot = alignToSurfaceNormal
                ? Quaternion.LookRotation(-hit.normal, Vector3.up)   // forward ~ -normal
                : Quaternion.LookRotation(Vector3.down, Vector3.forward);

            // Roll around the projector's forward axis to get a spinning reticle
            Quaternion spinRot = Quaternion.AngleAxis(_spinAngle, Vector3.forward);
            Quaternion desiredRot = baseRot * spinRot;

            // Smooth move/rotate
            float t = 1f - Mathf.Exp(-followSmoothing * Time.deltaTime);
            transform.position = Vector3.Lerp(transform.position, desiredPos, t);
            transform.rotation = Quaternion.Slerp(transform.rotation, desiredRot, t);

            // Make sure it's enabled when we have ground
            if (!decalProjector.enabled) decalProjector.enabled = true;
        }
        else
        {
            // No ground under target: optionally hide the decal
            if (decalProjector.enabled) decalProjector.enabled = false;
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (target == null) return;
        Gizmos.color = Color.cyan;
        Vector3 start = target.position + Vector3.up * rayStartHeight;
        Gizmos.DrawLine(start, start + Vector3.down * maxRayDistance);
        Gizmos.DrawSphere(start, 0.05f);
    }
#endif
}

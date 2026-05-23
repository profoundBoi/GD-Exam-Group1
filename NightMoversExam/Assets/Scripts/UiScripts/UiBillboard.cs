using UnityEngine;

/// <summary>
/// Keeps a world-space UI above an object without inheriting rotation.
/// The UI always faces the camera like a health bar.
/// </summary>
public class UIBillboard : MonoBehaviour
{
    [Tooltip("Object the UI should follow.")]
    public Transform target;

    [Tooltip("Offset above the target.")]
    public Vector3 offset = new Vector3(0, 2f, 0);

    [Tooltip("Leave empty to use Camera.main automatically.")]
    public Camera targetCamera;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;

        // Auto use parent as target if none assigned
        if (target == null && transform.parent != null)
            target = transform.parent;
    }

    void LateUpdate()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
            return;
        }

        if (target != null)
        {
            // Follow position only
            transform.position = target.position + offset;
        }

        // Face camera without inheriting object rotation
        transform.rotation = targetCamera.transform.rotation;
    }
}
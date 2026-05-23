using UnityEngine;


public class UIBillboard : MonoBehaviour
{
    [Tooltip("Leave empty to use Camera.main automatically.")]
    public Camera targetCamera;

    void Start()
    {
        if (targetCamera == null)
            targetCamera = Camera.main;
    }

    void LateUpdate()
    {
        if (targetCamera == null)
        {
            targetCamera = Camera.main;
            return;
        }

        transform.rotation = targetCamera.transform.rotation;
    }
}
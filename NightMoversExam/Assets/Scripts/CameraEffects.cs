using UnityEngine;
using Unity.Cinemachine;

public class CarCameraOffset : MonoBehaviour
{
    public CinemachineCamera cinemachineCamera;
    public CarController car;

    public float maxOffset = 1.5f;
    public float smoothSpeed = 5f;

    private CinemachineThirdPersonFollow follow;
    private Vector3 originalOffset;

    void Start()
    {
        follow = cinemachineCamera.GetComponent<CinemachineThirdPersonFollow>();

        if (follow != null)
        {
            originalOffset = follow.ShoulderOffset;
        }
    }

    void LateUpdate()
    {
        if (follow == null || car == null) return;

        Vector3 targetOffset = originalOffset;
        targetOffset.x += car.SteeringInput * maxOffset;

        follow.ShoulderOffset = Vector3.Lerp(
            follow.ShoulderOffset,
            targetOffset,
            Time.deltaTime * smoothSpeed
        );
    }
}
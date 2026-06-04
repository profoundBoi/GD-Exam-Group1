using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Engine")]
    public float motorForce = 1500f;
    public float brakeForce = 3000f;
    public float maxSteerAngle = 30f;

    [Header("Balance")]
    public Vector3 centerOfMass = new Vector3(0, -1.5f, 0);

    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Front Wheel Visuals (optional)")]
    public Transform frontLeftTransform;
    public Transform frontRightTransform;

    private float steerInput;
    private float accelerateInput;
    private bool isBraking;

    private Rigidbody rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = centerOfMass;
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        accelerateInput = context.ReadValue<float>();
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        isBraking = context.ReadValue<float>() > 0.1f;
    }

    public void OnSteer(InputAction.CallbackContext context)
    {
        steerInput = context.ReadValue<float>();
    }

    void FixedUpdate()
    {
        float motor = isBraking ? 0f : accelerateInput * motorForce;
        float brake = isBraking ? brakeForce : 0f;

        rearLeftWheel.motorTorque = motor;
        rearRightWheel.motorTorque = motor;

        frontLeftWheel.brakeTorque = brake;
        frontRightWheel.brakeTorque = brake;
        rearLeftWheel.brakeTorque = brake;
        rearRightWheel.brakeTorque = brake;

        float steer = steerInput * maxSteerAngle;
        frontLeftWheel.steerAngle = steer;
        frontRightWheel.steerAngle = steer;

        UpdateWheel(frontLeftWheel, frontLeftTransform);
        UpdateWheel(frontRightWheel, frontRightTransform);
    }

    void UpdateWheel(WheelCollider col, Transform t)
    {
        if (t == null) return;
        col.GetWorldPose(out Vector3 pos, out Quaternion rot);
        t.position = pos;
        t.rotation = rot;
    }
}
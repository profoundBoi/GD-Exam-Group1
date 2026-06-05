using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Engine")]
    public float moveSpeed = 20f;
    public float reverseSpeed = 10f;
    public float turnSpeed = 60f;

    private float steerInput;
    private float accelerateInput;
    private float reverseInput;
    private Rigidbody rb;

    public float SteeringInput => steerInput;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void OnAccelerate(InputAction.CallbackContext context)
    {
        accelerateInput = context.ReadValue<float>();
    }

    public void OnBrake(InputAction.CallbackContext context)
    {
        reverseInput = context.ReadValue<float>();
    }

    public void OnSteer(InputAction.CallbackContext context)
    {
        steerInput = context.ReadValue<float>();
    }

    void FixedUpdate()
    {
        float drive = (accelerateInput * moveSpeed) - (reverseInput * reverseSpeed);
        rb.linearVelocity = transform.forward * drive;

        float speed = rb.linearVelocity.magnitude;
        if (speed > 0.5f)
        {
            float turnDirection = accelerateInput > 0.1f ? 1f : (reverseInput > 0.1f ? -1f : 0f);
            float turn = steerInput * turnSpeed * Time.fixedDeltaTime * turnDirection;
            transform.Rotate(0f, turn, 0f);
        }
    }
}
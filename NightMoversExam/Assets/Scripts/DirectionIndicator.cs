using UnityEngine;

public class DirectionIndicator : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Rotation Axis")]
    public bool rotateOnX = false;
    public bool rotateOnY = true;
    public bool rotateOnZ = false;

    [Header("Smoothing")]
    public bool smoothRotation = true;
    public float rotationSpeed = 5f;

    void Update()
    {
        if (target == null) return;

        Vector3 direction = target.position - transform.position;

        if (direction == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        Vector3 euler = targetRotation.eulerAngles;
        if (!rotateOnX) euler.x = transform.eulerAngles.x;
        if (!rotateOnY) euler.y = transform.eulerAngles.y;
        if (!rotateOnZ) euler.z = transform.eulerAngles.z;
        targetRotation = Quaternion.Euler(euler);

        if (smoothRotation)
        {
            transform.rotation = Quaternion.Lerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
            );
        }
        else
        {
            transform.rotation = targetRotation;
        }
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
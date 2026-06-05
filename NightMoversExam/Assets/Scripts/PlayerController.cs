// Player_Controlls_Script.cs
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController3D : MonoBehaviour
{
    [SerializeField]
    private Vector3 moveInput;
    private Vector2 lookInput;

    private Rigidbody rb;
    private PlayerInput playerInput;
    private Animator animator;

    [Header("Movement")]
    public float speed = 5f;
    public float jumpForce = 5f;

    [Header("Look")]
    public float lookSensitivity = 120f;
    [SerializeField]
    private Transform PlayerCamera;
    public float minLookX = -60f;
    public float maxLookX = 60f;
    private float xRotation;
    [SerializeField]
    private float maxDistance, minDistance;

    private GameObject InteractableObject;
    public LayerMask Interact;
    public bool IsInteracting;

    [SerializeField]
    private int Interactrange;
    private GameObject CurrentInteractableObject;
    public Transform HoldPosition;
    public Transform MidlePoint;

    [SerializeField]
    public GameObject heldObject;
    private ObjectweightManager currentHeavyObject;
    public Transform MedianPoint;

    [Header("Animations")]
    [SerializeField] private Animator playerAnimations;
    private bool isJumping;
    [SerializeField] private List<string> AnimationBools;

    [Header("Carry Object Settings")]
    [SerializeField] private ObjectweightManager ObjectScript;

    [Header("CheckList Settings")]
    [SerializeField]
    private GameObject listDevice;
    public AudioSource audioSource;
    [SerializeField]
    private AudioClip OpenTabletAudio, CloseTabletAudio;
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        rb.freezeRotation = true;
    }

    void Start()
    {
        animator = GetComponent<Animator>();
        listDevice.SetActive(false);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 input = context.ReadValue<Vector2>();
        moveInput = new Vector3(input.x, 0f, input.y);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
    }

    public void OnOpenCheckList(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (!listDevice.activeSelf)
            {
                listDevice.SetActive(true);
                audioSource.clip = null;
                audioSource.clip = OpenTabletAudio;
                audioSource.Stop();
                audioSource.Play();
            }
            else if (listDevice.activeSelf)
            {
                listDevice.SetActive(false);
                audioSource.clip = null;
                audioSource.clip = CloseTabletAudio;
                audioSource.Stop();
                audioSource.Play();
            }
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            IsInteracting = true;
            Ray ray = new Ray(PlayerCamera.position, PlayerCamera.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Interactrange, Interact))
            {
                if (hit.collider != null)
                {
                    GameObject target = hit.collider.gameObject;
                    ObjectweightManager objScript = target.GetComponent<ObjectweightManager>();

                    if (objScript != null && objScript.canBePickedUp)
                    {
                        heldObject = target;
                        Rigidbody objRb = heldObject.GetComponent<Rigidbody>();
                        if (objRb != null) Destroy(objRb);
                    }
                    else if (objScript != null && !objScript.canBePickedUp)
                    {
                        currentHeavyObject = objScript;
                        objScript.AddHoldPosition(HoldPosition);
                    }
                }
            }
        }
        else if (context.canceled)
        {
            IsInteracting =false;
            if (currentHeavyObject != null)
            {
                currentHeavyObject.ClearHoldPositions();
                currentHeavyObject = null;
            }

            if (heldObject != null)
            {
                Rigidbody objRb = heldObject.GetComponent<Rigidbody>();
                if (objRb == null) objRb = heldObject.AddComponent<Rigidbody>();
                objRb.isKinematic = false;
                objRb.useGravity = true;

                ObjectweightManager owm = heldObject.GetComponent<ObjectweightManager>();
                if (owm != null) owm.ClearHoldPositions();

                heldObject = null;
            }
        }
    }

    void FixedUpdate()
    {
        Vector3 moveDirection = transform.TransformDirection(moveInput).normalized;
        Vector3 targetVelocity = moveDirection * speed;
        targetVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = targetVelocity;

        if (heldObject != null)
        {
            ObjectweightManager owm = heldObject.GetComponent<ObjectweightManager>();

            if (owm == null || owm.isNormalObject)
            {
                heldObject.transform.position = HoldPosition.position;
                heldObject.transform.rotation = HoldPosition.rotation;
            }
        }

        checkForInteraction();
    }

    void LateUpdate()
    {
        float mouseX = lookInput.x * lookSensitivity * Time.deltaTime;
        transform.Rotate(Vector3.up * mouseX);

        float mouseY = lookInput.y * lookSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, minLookX, maxLookX);
        PlayerCamera.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        float t = Mathf.InverseLerp(minLookX, maxLookX, xRotation);
        float distance = Mathf.Lerp(minDistance, maxDistance, t);
        PlayerCamera.localPosition = new Vector3(0f, 0f, distance);
    }

    private void checkForInteraction()
    {
        Ray ray = new Ray(PlayerCamera.position, PlayerCamera.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Interactrange, Interact))
        {
            if (hit.collider != null)
            {
                GameObject hitObject = hit.collider.gameObject;

                if (hitObject != CurrentInteractableObject)
                {
                    if (CurrentInteractableObject != null)
                    {
                        Outline old = CurrentInteractableObject.GetComponent<Outline>();
                        if (old != null) Destroy(old);
                    }

                    CurrentInteractableObject = hitObject;

                    if (CurrentInteractableObject.GetComponent<Outline>() == null)
                        CurrentInteractableObject.AddComponent<Outline>();
                }
            }
        }
        else
        {
            if (CurrentInteractableObject != null)
            {
                Outline old = CurrentInteractableObject.GetComponent<Outline>();
                if (old != null) Destroy(old);
                CurrentInteractableObject = null;
            }
        }
    }

    

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 1.1f);
    }
}
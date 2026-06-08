using UnityEngine;
[RequireComponent(typeof(Collider))]
public class TriggerLauncher : MonoBehaviour
{
    [Header("References")]
    [Tooltip("Drag the Teddy Bear GameObject here in the Inspector.")]
    public Rigidbody bearRigidbody;
    [Header("Launch Settings")]
    [Tooltip("How fast the bear is launched forward.")]
    public float launchForce = 15f;
    [Tooltip("Tag used to identify the player.")]
    public string playerTag = "Player";
    [Header("Sound")]
    public AudioClip launchSound;
    [Range(0f, 1f)]
    public float volume = 1f;
    private void Awake()
    {
        Collider col = GetComponent<Collider>();
        if (!col.isTrigger)
        {
            Debug.LogWarning($"[TriggerLauncher] The Collider on '{gameObject.name}' is not set to Is Trigger. Setting it now.");
            col.isTrigger = true;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(playerTag)) return;
        if (bearRigidbody == null)
        {
            Debug.LogWarning("[TriggerLauncher] No Rigidbody assigned for the bear!");
            return;
        }
        Vector3 launchDirection = bearRigidbody.transform.forward;
        bearRigidbody.linearVelocity = Vector3.zero;
        bearRigidbody.AddForce(launchDirection * launchForce, ForceMode.Impulse);
        if (launchSound != null)
            AudioSource.PlayClipAtPoint(launchSound, transform.position, volume);
        Debug.Log("[TriggerLauncher] Bear launched!");
    }
}
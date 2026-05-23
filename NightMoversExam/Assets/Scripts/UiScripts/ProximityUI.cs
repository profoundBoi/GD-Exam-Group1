using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to any interactable object (light or heavy).
/// Shows a world-space UI above the object ONLY when a player is nearby.
/// </summary>
public class ProximityUI : MonoBehaviour
{

    private static readonly List<Transform> _registeredPlayers = new List<Transform>();

    public static List<Transform> RegisteredPlayers => _registeredPlayers;

    public static void RegisterPlayer(Transform player)
    {
        if (player != null && !_registeredPlayers.Contains(player))
            _registeredPlayers.Add(player);
    }

    public static void UnregisterPlayer(Transform player)
    {
        _registeredPlayers.Remove(player);
    }

    [Header("Object Type")]
    [Tooltip("Tick for heavy objects (2 icons).")]
    public bool isHeavy = false;

    [Header("Detection")]
    [Tooltip("How close a player must be to show the UI.")]
    public float detectionRadius = 3f;

    [Tooltip("Player tag used to auto-find players.")]
    public string playerTag = "Player";

    [Header("UI References")]
    [Tooltip("World Space Canvas above the object.")]
    public Canvas uiCanvas;

    [Tooltip("Single icon for light objects.")]
    public GameObject lightIcon;

    [Tooltip("First icon for heavy objects.")]
    public GameObject heavyIcon1;

    [Tooltip("Second icon for heavy objects.")]
    public GameObject heavyIcon2;

    private bool _uiVisible;

    void Start()
    {
        // Find already existing players
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);

        foreach (GameObject player in players)
        {
            RegisterPlayer(player.transform);
        }

        ConfigureIcons();

        // UI starts hidden
        SetUIVisible(false);
    }

    void Update()
    {
        // Clean destroyed players
        _registeredPlayers.RemoveAll(p => p == null);

        bool anyClose = false;

        float sqrRange = detectionRadius * detectionRadius;

        foreach (Transform player in _registeredPlayers)
        {
            if ((transform.position - player.position).sqrMagnitude <= sqrRange)
            {
                anyClose = true;
                break;
            }
        }

        // Only update if state changed
        if (anyClose != _uiVisible)
        {
            SetUIVisible(anyClose);
        }
    }

    void ConfigureIcons()
    {
        if (lightIcon != null)
            lightIcon.SetActive(!isHeavy);

        if (heavyIcon1 != null)
            heavyIcon1.SetActive(isHeavy);

        if (heavyIcon2 != null)
            heavyIcon2.SetActive(isHeavy);
    }

    void SetUIVisible(bool visible)
    {
        _uiVisible = visible;

        if (uiCanvas != null)
        {
            uiCanvas.gameObject.SetActive(visible);
        }

        if (visible)
        {
            ConfigureIcons();
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isHeavy
            ? new Color(1f, 0.4f, 0.1f, 0.3f)
            : new Color(0.2f, 0.8f, 1f, 0.3f);

        Gizmos.DrawSphere(transform.position, detectionRadius);

        Gizmos.color = isHeavy
            ? new Color(1f, 0.4f, 0.1f, 1f)
            : new Color(0.2f, 0.8f, 1f, 1f);

        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
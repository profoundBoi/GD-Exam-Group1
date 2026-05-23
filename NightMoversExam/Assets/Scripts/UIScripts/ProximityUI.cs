using System.Collections.Generic;
using UnityEngine;

public class ProximityUI : MonoBehaviour
{
    private static readonly List<Transform> _registeredPlayers = new List<Transform>();

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
    [Tooltip("Tick for a heavy object (2 icons). Untick for a light object (1 icon).")]
    public bool isHeavy = false;

    [Header("Detection")]
    [Tooltip("Radius in world units that triggers the UI.")]
    public float detectionRadius = 3f;

    [Tooltip("Tag used to find pre-placed players at Start. " +
             "Spawned players should use RegisterPlayer() or PlayerRegistrant.")]
    public string playerTag = "Player";

    [Header("UI References")]
    [Tooltip("World Space Canvas child of this object.")]
    public Canvas uiCanvas;

    [Tooltip("Icon shown for a LIGHT object (single image).")]
    public GameObject lightIcon;

    [Tooltip("First icon for a HEAVY object.")]
    public GameObject heavyIcon1;

    [Tooltip("Second icon for a HEAVY object (shown next to the first).")]
    public GameObject heavyIcon2;

    private bool _uiVisible;

    void Start()
    {
        foreach (GameObject go in GameObject.FindGameObjectsWithTag(playerTag))
            RegisterPlayer(go.transform);

        ConfigureIcons();
        SetUIVisible(false);
    }

    void Update()
    {
        _registeredPlayers.RemoveAll(p => p == null);

        bool anyClose = false;
        foreach (Transform player in _registeredPlayers)
        {
            if (Vector3.Distance(transform.position, player.position) <= detectionRadius)
            {
                anyClose = true;
                break;
            }
        }

        if (anyClose != _uiVisible)
            SetUIVisible(anyClose);
    }

    void ConfigureIcons()
    {
        if (lightIcon != null) lightIcon.SetActive(!isHeavy);
        if (heavyIcon1 != null) heavyIcon1.SetActive(isHeavy);
        if (heavyIcon2 != null) heavyIcon2.SetActive(isHeavy);
    }

    void SetUIVisible(bool visible)
    {
        _uiVisible = visible;
        if (uiCanvas != null)
            uiCanvas.gameObject.SetActive(visible);
        if (visible)
            ConfigureIcons();
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = isHeavy ? new Color(1f, 0.4f, 0.1f, 0.3f)
                               : new Color(0.2f, 0.8f, 1f, 0.3f);
        Gizmos.DrawSphere(transform.position, detectionRadius);

        Gizmos.color = isHeavy ? new Color(1f, 0.4f, 0.1f, 1f)
                               : new Color(0.2f, 0.8f, 1f, 1f);
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
using System.Collections.Generic;
using UnityEngine;

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

    [Header("Detection")]
    [Tooltip("How close a player must be to show the UI.")]
    public float detectionRadius = 3f;

    [Tooltip("Player tag used to auto-find players.")]
    public string playerTag = "Player";

    [Header("UI References")]
    [Tooltip("World Space Canvas above the object.")]
    public Canvas uiCanvas;

    private bool _uiVisible;

    void Start()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag(playerTag);

        foreach (GameObject player in players)
        {
            RegisterPlayer(player.transform);
        }

        SetUIVisible(false);
    }

    void Update()
    {
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

        if (anyClose != _uiVisible)
        {
            SetUIVisible(anyClose);
        }
    }


    void SetUIVisible(bool visible)
    {
        _uiVisible = visible;

        if (uiCanvas != null)
        {
            uiCanvas.gameObject.SetActive(visible);
        }

    }

}
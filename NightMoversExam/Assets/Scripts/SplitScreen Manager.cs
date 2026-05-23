using UnityEngine;
using UnityEngine.InputSystem;

public class SplitScreenManager : MonoBehaviour
{
    private PlayerInputManager playerInputManager;

    void Awake()
    {
        playerInputManager = GetComponent<PlayerInputManager>();
        playerInputManager.onPlayerJoined += OnPlayerJoined;
    }

    void OnDestroy()
    {
        playerInputManager.onPlayerJoined -= OnPlayerJoined;
    }

    void OnPlayerJoined(PlayerInput player)
    {
        Camera cam = player.GetComponentInChildren<Camera>();
        if (cam == null) return;

        int playerIndex = player.playerIndex;

        if (playerIndex == 0)
        {
            // Top half
            cam.rect = new Rect(0f, 0.5f, 1f, 0.5f);
        }
        else if (playerIndex == 1)
        {
            // Bottom half
            cam.rect = new Rect(0f, 0f, 1f, 0.5f);
        }
    }
}
using UnityEngine;

public class PlayerRegistrant : MonoBehaviour
{
    void OnEnable()
    {
        ProximityUI.RegisterPlayer(transform);
    }
    void OnDisable()
    {
        ProximityUI.UnregisterPlayer(transform);
    }
}
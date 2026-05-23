using UnityEngine;

public class SplitScreenManager : MonoBehaviour
{
    [SerializeField] private Camera player1Camera;
    [SerializeField] private Camera player2Camera;

    void Start()
    {
        // Top half
        player1Camera.rect = new Rect(0f, 0.5f, 1f, 0.5f);

        // Bottom half
        player2Camera.rect = new Rect(0f, 0f, 1f, 0.5f);
    }
}
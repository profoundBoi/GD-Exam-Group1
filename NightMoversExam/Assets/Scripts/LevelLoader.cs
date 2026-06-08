using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnArrival : MonoBehaviour
{
    [Header("Settings")]
    public string carTag = "Car";
    public string sceneToLoad = "WinScene";
    public GameFinishManager gameFinisherScript;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(carTag))
        {
            gameFinisherScript.ShowCall();
        }
    }
}
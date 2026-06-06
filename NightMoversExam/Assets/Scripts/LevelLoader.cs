using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneOnArrival : MonoBehaviour
{
    [Header("Settings")]
    public string carTag = "Car";
    public string sceneToLoad = "WinScene";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(carTag))
        {
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
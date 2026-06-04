using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonManager : MonoBehaviour
{
    [SerializeField]
    private string neighbourhoodScene;

    public void StartGame()
    {
        SceneManager.LoadScene(neighbourhoodScene);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void OpenControls()
    {

    }
}

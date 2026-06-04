using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class buttonManager : MonoBehaviour
{
    [SerializeField]
    private string neighbourhoodScene;

    [SerializeField]
    private GameObject PausePanel, ControlsPanel;

    [SerializeField]
    private EventSystem eventSystem;

    [SerializeField]
    private GameObject currentSelectbutton;

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
        ControlsPanel.SetActive(true);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        PausePanel.SetActive(false);

    }

    public void CloseControls()
    {
        ControlsPanel.SetActive(false);
    }

    public void OpenControlsPanel()
    {
        ControlsPanel.SetActive(true);
    }

    public void PauseGame()
    {
        Time.timeScale = 0;
        PausePanel.SetActive(true);
        eventSystem.SetSelectedGameObject(currentSelectbutton);
    }
}

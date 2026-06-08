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

    [SerializeField]
    private GameObject BackButton, ResumeButton;

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
        eventSystem.SetSelectedGameObject(BackButton);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        PausePanel.SetActive(false);

    }

    public void CloseControls()
    {
        ControlsPanel.SetActive(false);
        eventSystem.SetSelectedGameObject(ResumeButton);
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

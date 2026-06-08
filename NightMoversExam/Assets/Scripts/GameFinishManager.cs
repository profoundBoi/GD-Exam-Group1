using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameFinishManager : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> objectsToDeliver;
    [SerializeField]
    private List<bool> objectsDelivered;

    [SerializeField]
    private List<string> missionTexts;
    [SerializeField]
    private TextMeshProUGUI missionText;
    [SerializeField]
    private int textindex, ListNumber;

    public EventSystem eventSystem;
    [SerializeField]
    private GameObject MissionPanel,NextButton;
    private bool canCheck;
    [SerializeField]
    private string SceneToLoad;

    private void Update()
    {
        for (int i = 0; i < objectsToDeliver.Count; i++)
        {
            if (!objectsToDeliver[i].activeSelf)
            {
                objectsDelivered[i] = true;
            }
        }

        if (objectsDelivered.All(b => b))
        {
            Debug.Log("All Objects Done");
            MissionPanel.SetActive(true);
            if (!canCheck)
            {
                canCheck = true;
                Time.timeScale = 0;
                eventSystem.SetSelectedGameObject(NextButton);
            }
        }

        missionText.text = missionTexts[textindex].ToString();
    }

    public void NextText()
    {
        if (textindex < ListNumber)
        {
            textindex++;
        }
        else if (textindex == ListNumber)
        {
            SceneManager.LoadScene(SceneToLoad);
        }
    }
}

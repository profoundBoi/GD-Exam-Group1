using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

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
    private int textindex;

    public EventSystem eventSystem;
    [SerializeField]
    private GameObject MissionPanel,NextButton;
    private bool canCheck;
    

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
                eventSystem.SetSelectedGameObject(NextButton);
            }
        }

        missionText.text = missionTexts[textindex].ToString();
    }

    public void NextText()
    {
        textindex++;
    }
}

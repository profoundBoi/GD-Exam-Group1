using UnityEngine;

public class CarryUIChanger : MonoBehaviour
{
    [Header("Player Tag")]
    public string playerTag = "Player";

    [Header("Object Type")]
    public bool isHeavy;

    [Header("Light Object UI")]
    public GameObject lightIdleImage;
    public GameObject lightCarryImage;

    [Header("Heavy Object UI")]

    public GameObject heavy1IdleImage;
    public GameObject heavy1CarryImage;

    public GameObject heavy2IdleImage;
    public GameObject heavy2CarryImage;

    private PlayerController3D[] players;

    void Start()
    {
        GameObject[] playerObjects = GameObject.FindGameObjectsWithTag(playerTag);

        players = new PlayerController3D[playerObjects.Length];

        for (int i = 0; i < playerObjects.Length; i++)
        {
            players[i] = playerObjects[i].GetComponent<PlayerController3D>();
        }
    }

    void Update()
    {
        if (players == null || players.Length == 0)
            return;

        if (!isHeavy)
        {
            bool anyoneHolding = false;

            foreach (PlayerController3D player in players)
            {
                if (player != null && player.heldObject != null)
                {
                    anyoneHolding = true;
                    break;
                }
            }

            if (lightIdleImage != null)
                lightIdleImage.SetActive(!anyoneHolding);

            if (lightCarryImage != null)
                lightCarryImage.SetActive(anyoneHolding);
        }
  
        else
        {
            if (players.Length > 0)
            {
                bool p1Holding = players[0] != null &&
                                 players[0].heldObject != null;

                if (heavy1IdleImage != null)
                    heavy1IdleImage.SetActive(!p1Holding);

                if (heavy1CarryImage != null)
                    heavy1CarryImage.SetActive(p1Holding);
            }

            if (players.Length > 1)
            {
                bool p2Holding = players[1] != null &&
                                 players[1].heldObject != null;

                if (heavy2IdleImage != null)
                    heavy2IdleImage.SetActive(!p2Holding);

                if (heavy2CarryImage != null)
                    heavy2CarryImage.SetActive(p2Holding);
            }
        }
    }
}
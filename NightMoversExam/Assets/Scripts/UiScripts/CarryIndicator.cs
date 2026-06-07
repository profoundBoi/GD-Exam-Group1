using UnityEngine;
using UnityEngine.UI;

public class CarryIndicator : MonoBehaviour
{
    [Header("References")]
    public PlayerController3D player;

    [Header("UI")]
    public RawImage carryingImage;
    public RawImage notCarryingImage;

    private void Update()
    {
        bool isCarrying = player.heldObject != null;

        if (carryingImage != null)
            carryingImage.gameObject.SetActive(isCarrying);

        if (notCarryingImage != null)
            notCarryingImage.gameObject.SetActive(!isCarrying);
    }
}
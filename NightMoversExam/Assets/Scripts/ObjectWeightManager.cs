// Object_weight_Manager.cs
using System.Collections.Generic;
using UnityEngine;

public class ObjectweightManager : MonoBehaviour
{
    [SerializeField]
    public bool isHeavyObject, isNormalObject;
    public bool canBePickedUp;

    public List<Transform> playerHoldingPosition = new List<Transform>();

    private GameObject medianPointObject;

    private void FixedUpdate()
    {
        performPickUp();

        if (isHeavyObject)
        {
            if (playerHoldingPosition.Count == 2 &&
                playerHoldingPosition[0] != null &&
                playerHoldingPosition[1] != null)
            {
                Vector3 medianPoint = (playerHoldingPosition[0].position + playerHoldingPosition[1].position) / 2f;

                if (medianPointObject == null)
                {
                    medianPointObject = new GameObject("MedianHoldPoint");
                    medianPointObject.transform.position = medianPoint;

                    transform.SetParent(medianPointObject.transform, worldPositionStays: true);
                    SetKinematic(true);
                }
                else
                {
                    medianPointObject.transform.position = medianPoint;
                }
            }
            else if (playerHoldingPosition.Count < 2 && medianPointObject != null)
            {
                transform.SetParent(null, worldPositionStays: true);
                Destroy(medianPointObject);
                medianPointObject = null;
                SetKinematic(false);
            }
        }
    }

    private void SetKinematic(bool isKinematic)
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) return;
        rb.isKinematic = isKinematic;
        rb.useGravity = !isKinematic;
    }

    public void ClearHoldPositions()
    {
        playerHoldingPosition.Clear();
    }

    public void performPickUp()
    {
        if (isNormalObject)
        {
            canBePickedUp = true;
        }
        else if (isHeavyObject)
        {
            canBePickedUp = playerHoldingPosition.Count == 2 &&
                            playerHoldingPosition[0] != null &&
                            playerHoldingPosition[1] != null;
        }
    }

    public void AddHoldPosition(Transform holdPosition)
    {
        if (playerHoldingPosition.Count < 2)
            playerHoldingPosition.Add(holdPosition);
    }

    public void CreateMedianHoldPoint()
    {
        if (playerHoldingPosition.Count < 2) return;
        if (playerHoldingPosition[0] == null || playerHoldingPosition[1] == null) return;

        Vector3 medianPoint = (playerHoldingPosition[0].position + playerHoldingPosition[1].position) / 2f;
        canBePickedUp = true;

        medianPointObject = new GameObject("MedianHoldPoint");
        medianPointObject.transform.position = medianPoint;

        transform.SetParent(medianPointObject.transform, worldPositionStays: true);
        SetKinematic(true);
    }

    public void DestroyMedianHoldPoint()
    {
        transform.SetParent(null, worldPositionStays: true);

        if (medianPointObject != null)
        {
            Destroy(medianPointObject);
            medianPointObject = null;
        }

        SetKinematic(false);
    }
}
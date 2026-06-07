using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryZone : MonoBehaviour
{
    [System.Serializable]
    public class DeliveryItem
    {
        public string objectTag;
        public RawImage uiImageP1;
        public RawImage uiImageP2;
    }

    [Header("Delivery Items")]
    public List<DeliveryItem> deliveryItems = new List<DeliveryItem>();

    [Header("Optional")]
    public ParticleSystem depositEffect;
    public AudioSource depositSound;

    private HashSet<string> depositedTags = new HashSet<string>();

    private void Start()
    {
        foreach (DeliveryItem item in deliveryItems)
        {
            if (item.uiImageP1 != null)
            {
                item.uiImageP1.color = Color.white;
            }

            if (item.uiImageP2 != null)
            {
                item.uiImageP2.color = Color.white;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach (DeliveryItem item in deliveryItems)
        {
            if (depositedTags.Contains(item.objectTag))
                continue;

            if (other.CompareTag(item.objectTag))
            {
                DepositObject(item, other.gameObject);
                break;
            }
        }
    }

    void DepositObject(DeliveryItem item, GameObject obj)
    {
        depositedTags.Add(item.objectTag);

        if (item.uiImageP1 != null)
        {
            item.uiImageP1.color = Color.green;
        }

        if (item.uiImageP2 != null)
        {
            item.uiImageP2.color = Color.green;
        }

        if (depositEffect != null)
        {
            Instantiate(depositEffect, obj.transform.position, Quaternion.identity);
        }

        if (depositSound != null)
        {
            depositSound.Play();
        }

        obj.SetActive(false);

        Debug.Log(obj.name + " deposited!");
    }
}
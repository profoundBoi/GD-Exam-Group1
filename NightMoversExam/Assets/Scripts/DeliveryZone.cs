using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryZone : MonoBehaviour
{
    [System.Serializable]
    public class DeliveryItem
    {
        public string objectTag;
        public Image uiImage;
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
            if (item.uiImage != null)
            {
                item.uiImage.gameObject.SetActive(false);
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

        if (item.uiImage != null)
        {
            item.uiImage.gameObject.SetActive(true);
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
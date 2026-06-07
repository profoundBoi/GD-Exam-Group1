using UnityEngine;

public class Destoyer : MonoBehaviour
{
    [Tooltip("Tag used to identify the player.")]
    public string playerTag = "Player";

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag(playerTag)) return;

        Debug.Log("Destroy");
        Destroy(gameObject);
    }
}
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class TeddyBear : MonoBehaviour
{
    [Header("Movement")]
    public float detectionRange = 20f;
    public float explodeRange = 2.5f;

    [Header("Explosion")]
    public float explosionDelay = 1f;
    public float explosionRadius = 3f;
    public int damage = 50;
    public LayerMask damageLayer;
    public GameObject explosionEffect;

    [Header("Target")]
    [Tooltip("Tag used to locate the player. Must match the Player GameObject's tag.")]
    public string playerTag = "Player";

    private NavMeshAgent agent;
    private bool isExploding = false;
    private Transform player;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = explodeRange * 0.9f;

        GameObject playerObj = GameObject.FindGameObjectWithTag(playerTag);
        if (playerObj != null)
            player = playerObj.transform;
        else
            Debug.LogWarning($"TeddyBear: No GameObject found with tag '{playerTag}'.");
    }

    void Update()
    {
        if (isExploding || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance <= explodeRange)
        {
            StartCoroutine(Explode());
        }
        else if (distance <= detectionRange)
        {
            agent.SetDestination(player.position);
        }
        else
        {
            if (agent.hasPath)
                agent.ResetPath();
        }
    }

    IEnumerator Explode()
    {
        isExploding = true;
        agent.isStopped = true;
        agent.ResetPath();

        Vector3 explosionPos = transform.position;

        foreach (var r in GetComponentsInChildren<Renderer>())
            r.enabled = false;

        agent.enabled = false;

        if (explosionEffect != null)
        {
            GameObject fx = Instantiate(explosionEffect, explosionPos, Quaternion.identity);
            Destroy(fx, 3f);
        }

        DealDamage(explosionPos);

        yield return new WaitForSeconds(explosionDelay);
        Destroy(gameObject);
    }

    void DealDamage(Vector3 center)
    {
        Collider[] hits = Physics.OverlapSphere(center, explosionRadius, damageLayer);

        foreach (Collider hit in hits)
        {
            if (!hit.CompareTag(playerTag) && !hit.transform.root.CompareTag(playerTag))
                continue;

            IDamageable damageable = hit.GetComponentInParent<IDamageable>();
            if (damageable != null)
                damageable.TakeDamage(damage);
        }
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.25f);
        Gizmos.DrawSphere(transform.position, detectionRange);

        Gizmos.color = new Color(1f, 0.5f, 0f, 0.4f);
        Gizmos.DrawSphere(transform.position, explodeRange);

        Gizmos.color = new Color(1f, 0f, 0f, 0.4f);
        Gizmos.DrawSphere(transform.position, explosionRadius);
    }
#endif
}
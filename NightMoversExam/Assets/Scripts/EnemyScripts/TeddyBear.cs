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

    [Header("Animation")]
    public float chaseDelay = 2f; // Should match animationBools[0] clip length

    private NavMeshAgent agent;
    private bool isExploding = false;
    private bool hasActivated = false;
    private bool canChase = false;
    private Transform player;
    private BearAnimationManager animManager;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = explodeRange * 0.9f;

        animManager = GetComponent<BearAnimationManager>();
        if (animManager == null)
            Debug.LogWarning("TeddyBear: No BearAnimationManager found on this GameObject.");

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
            // Trigger activation animations the first time the player is detected
            if (!hasActivated)
            {
                hasActivated = true;
                if (animManager != null)
                    animManager.ActivateBear();
                StartCoroutine(ChaseDelay());
            }

            if (canChase)
                agent.SetDestination(player.position);
        }
        else
        {
            if (agent.hasPath)
                agent.ResetPath();
        }
    }

    IEnumerator ChaseDelay()
    {
        yield return new WaitForSeconds(chaseDelay);
        canChase = true;
    }

    IEnumerator Explode()
    {
        isExploding = true;
        agent.isStopped = true;
        agent.ResetPath();

        // Trigger explosion animation before hiding the bear
        if (animManager != null)
            animManager.TriggerExplosionAnim();

        yield return new WaitForSeconds(explosionDelay);

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
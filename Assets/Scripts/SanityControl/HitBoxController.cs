using UnityEngine;
using UnityEngine.AI;

public class HitBoxController : MonoBehaviour
{
    private SanityDrainer _mainDrainer;
    [SerializeField] private float _damage = 1f;
    [SerializeField] private float _selfStun = 5f;

    public bool isStunned = false;
    private NavMeshAgent _agent;
    private float _stunTimer = 0f;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hitbox triggered by " + other.name);
        if (isStunned) return;

        if (other.CompareTag("Player"))
        {
            PlayerSanity player = other.GetComponentInParent<PlayerSanity>();
            if (player != null)
            {
                player.TakeDamage(_damage);
                StartStun();
            }
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _mainDrainer = GetComponentInParent<SanityDrainer>();
        _agent = GetComponentInParent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isStunned && Time.time >= _stunTimer)
        {
            EndStun();
        }
    }

    private void StartStun()
    {
        isStunned = true;
        _stunTimer = Time.time + _selfStun;

        if (_agent != null)
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
        }

        Debug.Log("Enemy stunned");
    }

    private void EndStun()
    {
        isStunned = false;

        if (_agent != null)
        {
            _agent.isStopped = false;
        }

        Debug.Log("Enemy not stunned");
    }
}

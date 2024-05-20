using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MummyAI : MonoBehaviour
{
    [SerializeField] private Animator mummyAnimator;
    [SerializeField] private NavMeshAgent mummyAgent;
    [SerializeField] private List<Transform> destinations;
    [SerializeField] private Transform player;
    [SerializeField] private Player playerScript;

    // Mummy settings
    [Header("Walk Speeds")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float chaseSpeed;

    [Header("Idle Time")]
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;

    [Header("Field Of View")]
    [SerializeField] private GameObject raycastSource;
    [SerializeField] private float sightDistance;
    [SerializeField] private float fieldOfViewAngle;

    [Header("Chase Time")]
    [SerializeField] private float chaseTimer;
    [SerializeField] private float chaseTime;
    [SerializeField] private float minChaseTime;
    [SerializeField] private float maxChaseTime;

    [Header("Attack Settings")]
    [SerializeField] private float attackDistance;

    public Transform currentDest;
    public int currentDestIndex = 0;
    public bool moveToWaypoint = true;
    public bool isAttacking;

    [SerializeField] private BoxCollider boxCollider;

    public enum EnemyState
    {
        Patrol,
        Chase,
        Attack
    }
    public EnemyState currentState;

    private void Awake()
    {
        mummyAnimator = GetComponent<Animator>();
        mummyAgent = GetComponent<NavMeshAgent>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        boxCollider = GetComponentInChildren<BoxCollider>();
    }

    void Start()
    {
        currentState = EnemyState.Patrol;
        currentDest = destinations[currentDestIndex];
        isAttacking = false;
        chaseTimer = 0;
        boxCollider.enabled = false;
        playerScript = player.GetComponent<Player>();
    }

    void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:
                Patrol();
                break;
            case EnemyState.Chase:
                Chase();
                break;
            case EnemyState.Attack:
                Attack();
                break;
        }
    }

    void Patrol()
    {
        Vector3 direction = (player.position - raycastSource.transform.position).normalized;
        float angle = Vector3.Angle(raycastSource.transform.forward, direction);

        if (angle <= fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(raycastSource.transform.position, direction, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    // Player is within sight distance and angle
                    float distanceToPlayer = Vector3.Distance(raycastSource.transform.position, player.position);
                    if (distanceToPlayer <= sightDistance)
                    {
                        currentState = EnemyState.Chase;
                        chaseTime = Random.Range(minChaseTime, maxChaseTime);
                        chaseTimer = 0;
                        return;
                    }
                }
            }
        }

        if (moveToWaypoint == true)
        {
            mummyAgent.SetDestination(currentDest.position);
            mummyAgent.speed = walkSpeed;
            mummyAnimator.ResetTrigger("Idle");
            mummyAnimator.SetTrigger("Walk");

            if (mummyAgent.remainingDistance <= mummyAgent.stoppingDistance)
            {
                StartCoroutine(StayIdleCoroutine());
                moveToWaypoint = false;
            }
        }
    }

    void Chase()
    {
        mummyAnimator.ResetTrigger("Idle");
        mummyAnimator.ResetTrigger("Walk");
        mummyAnimator.ResetTrigger("Attack");
        mummyAnimator.SetTrigger("Run");
        mummyAgent.speed = chaseSpeed;
        mummyAgent.SetDestination(player.transform.position);

        float distance = Vector3.Distance(player.position, mummyAgent.transform.position);
        chaseTimer += Time.deltaTime;

        if (distance <= attackDistance)
        {
            currentState = EnemyState.Attack;
        }
        else if (chaseTimer >= chaseTime)
        {
            currentState = EnemyState.Patrol;
            chaseTimer = 0;
        }
        else
        {
            Vector3 direction = (player.position - raycastSource.transform.position).normalized;
            float angle = Vector3.Angle(raycastSource.transform.forward, direction);

            if (angle > fieldOfViewAngle * 0.5f || !IsPlayerInSight())
            {
                // Continue chasing for the remaining chaseTime
            }
        }
    }

    void Attack()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            mummyAnimator.ResetTrigger("Idle");
            mummyAnimator.ResetTrigger("Walk");
            mummyAnimator.ResetTrigger("Run");
            mummyAnimator.SetTrigger("Attack");
            mummyAgent.speed = 0;

            Vector3 direction = (player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
        }

        if (mummyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Mummy Attack") &&
            mummyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.95f)
        {
            // Attack animation is almost finished, reset the flag and set cooldown
            isAttacking = false;
            currentState = EnemyState.Chase;
        }
    }

    private IEnumerator StayIdleCoroutine()
    {
        float idleTime = Random.Range(minIdleTime, maxIdleTime);
        if (idleTime < 1f)
        {
            idleTime = 0;
        }
        else
        {
            mummyAnimator.ResetTrigger("Walk");
            mummyAnimator.SetTrigger("Idle");
            mummyAgent.speed = 0;

            yield return new WaitForSeconds(idleTime);

            moveToWaypoint = true;
            // choose next destination
            if (currentDestIndex == destinations.Count - 1)
            {
                currentDestIndex = 0;
            }
            else
            {
                currentDestIndex++;
            }
            currentDest = destinations[currentDestIndex];
            mummyAgent.SetDestination(currentDest.position);
        }
    }

    private bool IsPlayerInSight()
    {
        Vector3 direction = (player.position - raycastSource.transform.position).normalized;
        float angle = Vector3.Angle(raycastSource.transform.forward, direction);

        if (angle <= fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(raycastSource.transform.position, direction, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    return true;
                }
            }
        }
        return false;
    }

    void EnableAttack()
    {
        boxCollider.enabled = true;
    }

    void DisableAttack()
    {
        boxCollider.enabled = false;
    }

    public void OnChildTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerScript.TakeDamage();
        }
    }
}

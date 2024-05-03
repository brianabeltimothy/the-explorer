using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class MummyAI : MonoBehaviour
{
    [SerializeField] private Animator mummyAnimator;
    [SerializeField] private NavMeshAgent mummyAgent;
    [SerializeField] private List<Transform> destinations;

    //mummy settings
    [Header("Mummy Settings")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float chaseSpeed;
    [SerializeField] private float minIdleTime;
    [SerializeField] private float maxIdleTime;
    [SerializeField] private float idleTime;
    [SerializeField] private Vector3 rayCastOffset;
    [SerializeField] private float sightDistance;
    [SerializeField] private float attackDistance;
    [SerializeField] private float chaseTime;
    [SerializeField] private float minChaseTime;
    [SerializeField] private float maxChaseTime; 
    [SerializeField] private float jumpscareTime;

    [SerializeField] private bool floatchasing;
    [SerializeField] private Transform player;
    public float fieldOfViewAngle;
    public GameObject raycastSource; 

    Transform currentDest;
    Vector3 dest;
    int currentDestIndex = 0;

    //FSM
    private FSM<MummyState> fsm;
    public enum MummyState
    {
        Idle,
        Patrol,
        Chase,
        Attack
    }
    private bool isIdleCoroutineRunning = false;
    private bool isAttackCoroutineRunning = false;

    private void Awake()
    {
        mummyAnimator = GetComponent<Animator>();
        mummyAgent = GetComponent<NavMeshAgent>();

        // Initialize the FSM with the initial state
        fsm = new FSM<MummyState>(MummyState.Patrol);
    }

    private void Start()
    {
        currentDest = destinations[currentDestIndex];

        // Add states to the FSM
        fsm.AddState(MummyState.Idle, OnEnterIdle, OnUpdateIdle, OnExitIdle);
        fsm.AddState(MummyState.Patrol, OnEnterPatrol, OnUpdatePatrol, OnExitPatrol);
        fsm.AddState(MummyState.Chase, OnEnterChase, OnUpdateChase, OnExitChase);
        fsm.AddState(MummyState.Attack, OnEnterAttack, OnUpdateAttack, OnExitAttack);
    }
    
    private void Update()
    {
        // Update the FSM
        fsm.Update();

        Vector3 direction = (player.position - raycastSource.transform.position).normalized; // Use the raycastSource's position
        float angle = Vector3.Angle(raycastSource.transform.forward, direction);
        if (angle <= fieldOfViewAngle * 0.5f)
        {
            RaycastHit hit;
            if (Physics.Raycast(raycastSource.transform.position + rayCastOffset, direction, out hit, sightDistance))
            {
                if (hit.collider.gameObject.tag == "Player")
                {
                    float distanceToPlayer = Vector3.Distance(raycastSource.transform.position, player.position);
                    
                    if (distanceToPlayer <= sightDistance)
                    {
                        StopAllCoroutines();
                        StartCoroutine(ChaseCoroutine());
                    }
                }
            }
        }
        Debug.DrawRay(transform.position + rayCastOffset, direction * sightDistance, Color.red);

        float distance = Vector3.Distance(player.position, mummyAgent.transform.position);
        
        if (distance <= attackDistance && !isOnCooldown)
        {
            // Transition to the attack state only if not on cooldown
            fsm.ChangeState(MummyState.Attack);
            // Start the cooldown timer
            attackCooldownTimer = attackCooldownDuration;
            isOnCooldown = true;
        }

        // Update the cooldown timer
        if (isOnCooldown)
        {
            attackCooldownTimer -= Time.deltaTime;
            if (attackCooldownTimer <= 0f)
            {
                isOnCooldown = false; // Reset the cooldown state
            }
        }
    }

    // Define enter, update, and exit callbacks for each state
    #region IdleState
    private void OnEnterIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        if(idleTime < 1f)
        {
            idleTime = 0;
            fsm.ChangeState(MummyState.Patrol);
        }
        else{
            mummyAnimator.ResetTrigger("Walk");
            mummyAnimator.SetTrigger("Idle");
            mummyAgent.speed = 0;
        }
    }
    
    private void OnUpdateIdle()
    {
        // If the coroutine is not already running, start it
        if (!isIdleCoroutineRunning)
        {
            StartCoroutine(StayIdleCoroutine());
        }
    }

    private void OnExitIdle()
    {
        // choose next destination
        if(currentDestIndex == destinations.Count - 1)
        {
            currentDestIndex = 0;
        }
        else
        {
            currentDestIndex++;
        }
        currentDest = destinations[currentDestIndex];
        dest = currentDest.position;
        mummyAgent.destination = dest;
        isIdleCoroutineRunning = false;
    }
    #endregion

    #region PatrolState
    private void OnEnterPatrol()
    {
        
    }
    private void OnUpdatePatrol()
    {
        dest = currentDest.position;
        mummyAgent.destination = dest;
        mummyAgent.speed = walkSpeed;
        mummyAnimator.ResetTrigger("Idle");
        mummyAnimator.SetTrigger("Walk");
        if(mummyAgent.remainingDistance <= mummyAgent.stoppingDistance)
        {
            fsm.ChangeState(MummyState.Idle);
        }
    }   
    private void OnExitPatrol()
    {

    }
    #endregion
    
    #region ChaseState
    private void OnEnterChase()
    {
        Debug.Log("on enter chase");
        mummyAnimator.ResetTrigger("Idle");
        mummyAnimator.ResetTrigger("Walk");
        mummyAnimator.ResetTrigger("Attack");
        mummyAnimator.SetTrigger("Run");
        mummyAgent.speed = chaseSpeed;
    }

    private void OnUpdateChase()
    {
        dest = player.position;
        mummyAgent.destination = dest;
        Vector3 direction = (dest - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);
    }


    private void OnExitChase()
    {
        // Debug.Log("on exit chase");
    }
    #endregion

    #region AttackState
    [SerializeField] private bool isOnCooldown = false;
    [SerializeField] private float attackCooldownTimer = 0f;
    [SerializeField] private float attackCooldownDuration = 1f;

    private void OnEnterAttack()
    {
        mummyAnimator.ResetTrigger("Idle");
        mummyAnimator.ResetTrigger("Walk");
        mummyAnimator.ResetTrigger("Run");
        mummyAnimator.SetTrigger("Attack");
        mummyAgent.speed = 0;
    }

    private void OnUpdateAttack()
    {
        
    }

    private void OnExitAttack()
    {
        // Implement any exit logic here
    }
    #endregion

    private IEnumerator StayIdleCoroutine()
    {
        isIdleCoroutineRunning = true;
        yield return new WaitForSeconds(idleTime);
        fsm.ChangeState(MummyState.Patrol);
    }

    private IEnumerator ChaseCoroutine()
    {
        fsm.ChangeState(MummyState.Chase);
        chaseTime = Random.Range(minChaseTime, maxChaseTime);
        yield return new WaitForSeconds(chaseTime);
        fsm.ChangeState(MummyState.Patrol); 
        currentDest = destinations[currentDestIndex];
    }
}

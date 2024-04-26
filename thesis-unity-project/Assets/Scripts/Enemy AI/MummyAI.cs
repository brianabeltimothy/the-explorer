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
    [SerializeField] private float catchDistance;
    [SerializeField] private float chaseTime;
    [SerializeField] private float minChaseTime;
    [SerializeField] private float maxChaseTime; 
    [SerializeField] private float jumpscareTime;

    // [SerializeField] private bool walking; // will be replaced by state
    // [SerializeField] private bool chasing; // will be replaced by state
    [SerializeField] private bool floatchasing;
    [SerializeField] private Transform player;

    Transform currentDest;
    Vector3 dest;
    int currentDestIndex = 0;

    //FSM
    private FSM<MummyState> fsm;
    public enum MummyState
    {
        Idle,
        Patrol
    }
    private bool isIdleCoroutineRunning = false;

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
    }
    
    private void Update()
    {
        // Update the FSM
        fsm.Update();
    }

    // Define enter, update, and exit callbacks for each state
    private void OnEnterIdle()
    {
        idleTime = Random.Range(minIdleTime, maxIdleTime);
        Debug.Log("idleTime: "+idleTime);
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
            StartCoroutine(stayIdleCoroutine());
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

    private IEnumerator stayIdleCoroutine()
    {
        isIdleCoroutineRunning = true;
        yield return new WaitForSeconds(idleTime);
        fsm.ChangeState(MummyState.Patrol);
    }
}

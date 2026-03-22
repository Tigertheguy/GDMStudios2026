using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class HearOnly : EnemyAI
{
    [Header("Roaming Settings")]
    [SerializeField] private float _roamRadius = 20f;
    [SerializeField] private float _idleListenTime = 3f;
    [SerializeField] private float _startledListenTime = 2f;
    [SerializeField] private float _searchingListenTime = 0.5f;
    [SerializeField] private float _roamingListenTime = 0.5f;
    [SerializeField] private float _idleHearingMultiplier = 3f;

    private float listenTimer;
    private bool isListening;
    private Vector3 roamDestination;
    private float _pauseTimer = 1f;
    private bool isPaused = false;
    private HearingSensor sensor;

    void Start()
    {
        base.Start();
        sensor = GetComponent<HearingSensor>();
        SetNewDestination();
    }


    // Update is called once per frame
    public override void Update()
    {
        StunCheck();
        
        if (currState == AIStates.Chasing)
        {
            Debug.Log("Chasing");
            SeeOnlyChase();
        }

        if (currState == AIStates.Searching)
        {
            Debug.Log("Searching");
            SeeOnlySearch();
        }

        if (currState == AIStates.Startled)
        {
            Debug.Log("Startled");
            SeeOnlyRoam();
        }

        if (currState == AIStates.Idle)
        {
            //Debug.Log("Idle");
            SeeOnlyIdle();
        }

    }

    private void SeeOnlyChase()
    {
        //Gun it straight towards the player
        isListening = false;
        navAgent.speed = chaseSpeed;

        if (currTarget != null)
        {
            lastKnownLocation = currTarget.transform.position;
            navAgent.SetDestination(lastKnownLocation);
            PlayWalkSound();
        }
    }

    private void SeeOnlySearch()
    {

    }

    private void SeeOnlyRoam()
    {
        
    }
    private void SeeOnlyIdle()
    {
        if(isPaused)
        {
            _pauseTimer -= Time.deltaTime;
            if(_pauseTimer <= 0)
            {
                isPaused = false;
                sensor.SetSoundMultiplier(_idleHearingMultiplier);
                SetNewDestination();
            }
            return;
        }

        //If not scanning then has to be moving
        if (!isListening)
        {

            navAgent.speed = idleSpeed;
            //If close enough
            if(!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
            {
                //Start scan
                isListening = true;
                listenTimer = _idleListenTime;
                StopWalkSound();
            }
            else
            {
                PlayWalkSound();
            }
        }
        //Scan
        else
        {
            
            if (sensor != null)
            {
                sensor.SetSoundMultiplier(_idleHearingMultiplier);
            }

            listenTimer -= Time.deltaTime;
            if (listenTimer <= 0)
            {
                isListening = false;
                isPaused = true;
                _pauseTimer = 1f;
                //SetNewDestination();
            }
        }
    }

    private void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _roamRadius;
        randomDirection += initialPosition;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, _roamRadius, 1))
        {
            roamDestination = hit.position;
            navAgent.SetDestination(roamDestination);
        }
    }

}

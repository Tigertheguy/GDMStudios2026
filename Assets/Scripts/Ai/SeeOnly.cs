using NUnit.Framework;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class SeeOnly : EnemyAI
{
    [Header("Roaming Settings")]
    [SerializeField] private float _roamRadius = 20f;
    [SerializeField] private float _idleScanTime = 3f;
    [SerializeField] private float _startledScanTime = 2f;
    [SerializeField] private float _searchingScanTime = 0.5f;
    [SerializeField] private float _roamingScanTime = 0.5f;
    [SerializeField] private float _idleRotateSpeed = 5f;
    [SerializeField] private float _searchRotateSpeed = 80f;
    [SerializeField] private float _roamRotateSpeed = 250f;
    [SerializeField] private float _scanCone = 90f;

    private float scanTimer;
    private bool isScanning;
    private Vector3 roamDestination;
    private float _pauseTimer = 1f;
    private bool isPaused = false;

    void Start()
    {
        base.Start();
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
        isScanning = false;
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
                SetNewDestination();
            }
            return;
        }

        //If not scanning then has to be moving
        if (!isScanning)
        {

            navAgent.speed = idleSpeed;
            //If close enough
            if(!navAgent.pathPending && navAgent.remainingDistance < 0.5f)
            {
                //Start scan
                isScanning = true;
                scanTimer = _idleScanTime;
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
            float rotationAngle = Mathf.Sin(Time.time * _idleRotateSpeed * 0.05f) * _scanCone;
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y + rotationAngle * Time.deltaTime, 0);

            scanTimer -= Time.deltaTime;
            if (scanTimer <= 0)
            {
                isScanning = false;
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

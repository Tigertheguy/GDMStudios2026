using UnityEngine;
using UnityEngine.AI;

public class SeeOnly : EnemyAI
{
    [Header("Roaming Settings")]
    [SerializeField] private float roamRadius = 20f;
    [SerializeField] private float idleScanTime = 3f;
    [SerializeField] private float startledScanTime = 2f;
    [SerializeField] private float searchingScanTime = 0.5f;
    [SerializeField] private float roamingScanTime = 0.5f;
    [SerializeField] private float searchRotateSpeed = 120f;
    [SerializeField] private float roamRotateSpeed = 250f;

    private float scanTimer;
    private bool isScanning;
    private Vector3 roamDestination;


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
    }

    private void SetNewDestination()
    {
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1))
        {
            roamDestination = hit.position;
            navAgent.SetDestination(roamDestination);
        }
    }

}

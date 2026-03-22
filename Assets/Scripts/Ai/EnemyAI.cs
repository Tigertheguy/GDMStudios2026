using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.AI;
using Mono.Cecil.Cil;



#if UNITY_EDITOR
using UnityEditor;
#endif // checks if you are in the editor and runs the code inside the block. If not then wont run

[RequireComponent(typeof(Awareness))]
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]

public class EnemyAI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI FeedbackDisplay;

    [Header("Detection Settings")]
    [SerializeField] public float _VisionConeAngle = 60f;
    [SerializeField] public float _VisionConeRange = 30f;
    [SerializeField] public Color _VisionConeColour = new Color(1f, 0f, 0f, 0.25f);

    [SerializeField] public float _HearingRange = 20f;
    [SerializeField] public Color _HearingRangeColour = new Color(1f, 1f, 0f, 0.25f);

    [SerializeField] public float _ProximityDetectionRange = 3f;
    [SerializeField] public Color _ProximityRangeColour = new Color(1f, 1f, 1f, 0.25f);

    [SerializeField] public AK.Wwise.Event _startEnemyWalkRandomEvent;
    [SerializeField] public AK.Wwise.Event _stopEnemyWalkRandomEvent;

    [SerializeField] public AK.Wwise.Event _startEnemyGrowl1;
    //Getters
    public float VisionConeRange { get { return _VisionConeRange; } }
    public float VisionConeAngle { get { return _VisionConeAngle; } }
    public Color VisionConeColour { get { return _VisionConeColour; } }

    public float HearingRange { get { return _HearingRange; } }
    public Color HearingRangeColour { get { return _HearingRangeColour; } }

    public float ProximityDetectionRange { get { return _ProximityDetectionRange; } }
    public Color ProximityDetectionColour { get { return _ProximityRangeColour; } }

    public float CosVisionConeAngle { get; private set; } = 0f;

    //Set origin point for raycasting to current position
    public Vector3 EyeLocation { get { return transform.position; } }
    //Set direction for raycasting to current forward direction
    public Vector3 EyeDirection { get { return transform.forward; } }

    Awareness awareness;

    public UnityEngine.AI.NavMeshAgent navAgent;

    public enum AIStates { Idle, Roaming, Scanning, Listening, Feeling, Waiting, Ambushing, Charging, Startled, Searching, Chasing }
    public AIStates currState = AIStates.Idle;
    public GameObject currTarget;
    public Vector3 lastKnownLocation;
    public bool isWalking;
    public bool willGrowl;

    private HitBoxController hitbox;

    [Header("Movement Settings")]
    [SerializeField] public float chaseSpeed = 8.0f;
    [SerializeField] public float roamSpeed = 6.5f;
    [SerializeField] public float searchSpeed = 4f;
    [SerializeField] public float startledSpeed = 2f;
    [SerializeField] public float idleSpeed = 1f;
    [SerializeField] public float acceleration = 8.0f;

    void Awake()
    {
        CosVisionConeAngle = Mathf.Cos(VisionConeAngle * Mathf.Deg2Rad);
        awareness = GetComponent<Awareness>();
        navAgent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        navAgent.acceleration = acceleration;
        hitbox = GetComponentInChildren<HitBoxController>();
    }

    void Start()
    {
        isWalking = false;
        willGrowl = true;
    }

    public void StunCheck()
    {
        //Stop if stunned
        if (hitbox.isStunned)
        {
            if (navAgent.isOnNavMesh) navAgent.isStopped = true;
            return;
        }
        //Unstun 
        if (navAgent.isOnNavMesh && navAgent.isStopped)
        {
            navAgent.isStopped = false;
        }
    }
    // Update is called once per frame
    //Virtual is for abstraction
    public virtual void Update()
    {
        StunCheck();

        if (FeedbackDisplay != null)
        {
            FeedbackDisplay.text = currState.ToString();
        }

        if (currState == AIStates.Chasing)
        {
            lastKnownLocation = currTarget.transform.position;
            navAgent.SetDestination(lastKnownLocation);
        }
        else if (currState == AIStates.Searching)
        {
            navAgent.SetDestination(lastKnownLocation);
        }
        else if (currState == AIStates.Startled)
        {
            navAgent.SetDestination(lastKnownLocation);
            // Startled sound in the future
        }
        else if (currState == AIStates.Idle)
        {
            navAgent.ResetPath();
        }
    }

    public void PlayGrowlSound()
    {
        if (!willGrowl)
        {
            return;
        }
        _startEnemyGrowl1.Post(gameObject);
        willGrowl = false;
    }

    public void PlayWalkSound()
    {
        if (isWalking)
        {
            return;
        }
        _startEnemyWalkRandomEvent.Post(gameObject);
        isWalking = true;
    }

    public void StopWalkSound()
    {
        if (!isWalking)
        {
            return;
        }
        _stopEnemyWalkRandomEvent.Post(gameObject);
        isWalking = false;
    }

    public void CanSee(Detectable detectable)
    {
        awareness.CanSee(detectable);
        // Debug.Log("Can see " + detectable.gameObject.name);
    }

    public void CanHear(GameObject source, Vector3 location, HeardSoundType soundType, float loudness)
    {
        awareness.CanHear(source, location, soundType, loudness);
        // Debug.Log("Heard sound of type " + soundType + " at location " + location + " with loudness " + loudness);
    }

    public void CanDetectProximity(Detectable detectable)
    {
        awareness.CanDetectProximity(detectable);
        //Debug.Log("In proximity of " + detectable.gameObject.name);
    }

    public void gainSuspicion()
    {
        currState = AIStates.Startled;
        navAgent.speed = startledSpeed;
        currTarget = null;
        PlayGrowlSound();
        isWalking = false;
    }

    public void gainDetection(GameObject target)
    {
        currState = AIStates.Searching;
        navAgent.speed = searchSpeed;
        currTarget = target;
        lastKnownLocation = target.transform.position;
        isWalking = false;
    }

    public void gainFull(GameObject target)
    {
        currState = AIStates.Chasing;
        PlayWalkSound();
        navAgent.speed = chaseSpeed;
        currTarget = target;
        lastKnownLocation = target.transform.position;
    }

    public void loseDetection(GameObject target)
    {
        currState = AIStates.Searching;
        navAgent.speed = searchSpeed;
        currTarget = target;
    }

    public void loseSuspicion()
    {
        currState = AIStates.Startled;
        navAgent.speed = startledSpeed;
        currTarget = null;
        willGrowl = true;
        StopWalkSound();
    }

    public void loseFull()
    {
        currState = AIStates.Idle;
        navAgent.speed = idleSpeed;
        currTarget = null;
        willGrowl = true;
        StopWalkSound();
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(EnemyAI), true)]
public class EnemyAIEditor : Editor
{
    public void OnSceneGUI()
    {
        var ai = target as EnemyAI;

        // draw the detectopm range
        Handles.color = ai.ProximityDetectionColour;
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.ProximityDetectionRange);

        // draw the hearing range
        Handles.color = ai.HearingRangeColour;
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.HearingRange);

        // work out the start point of the vision cone
        Vector3 startPoint = Mathf.Cos(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.forward +
                             Mathf.Sin(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.right;

        // draw the vision cone
        Handles.color = ai.VisionConeColour;
        Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.VisionConeAngle * 2f, ai.VisionConeRange);
    }
}
#endif 

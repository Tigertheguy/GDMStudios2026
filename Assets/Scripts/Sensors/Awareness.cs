using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;

public class TrackedTarget
{
    public Detectable detectable;
    public Vector3 rawPos;

    public float lastSensedTime = -1;
    public float awareness; //0 - not aware, 0-1 aware, 1-2 highly aware (turn to see), 2 full aggro

    public bool UpdateAwareness(Detectable detectable, Vector3 position, float awareness, float minAwareness)
    {
        var oldAwareness = this.awareness;
        if (detectable != null)
            this.detectable = detectable;
        this.rawPos = position;
        this.lastSensedTime = Time.time;

        this.awareness = Mathf.Clamp(Mathf.Max(this.awareness, minAwareness) + awareness, 0, 2);

        //Recently detected
        if (oldAwareness < 2 && this.awareness >= 2)
        {
            return true;
        }
        if (oldAwareness < 1 && this.awareness >= 1)
        {
            return true;
        }
        if(oldAwareness <= 0 && this.awareness >= 0)
        {
            return true;
        }


        return false;
    }

    public bool DecayAwareness(float delay, float amt)
    {
        //Adds a bit of delay so it doesnt constantly tick when detecting the player
        //Only starts to decay once player is no longer detected
        if((Time.time - lastSensedTime) < delay)
        {
            return false;
        }
        var oldAwareness = this.awareness;

        this.awareness -= amt;

        if (oldAwareness >= 2 && this.awareness < 2)
        {
            return true;
        }
        if (oldAwareness >= 1 && this.awareness < 1)
        {
            return true;
        }

        return this.awareness <= 0;
    }

}
[RequireComponent(typeof(EnemyAI))]

public class Awareness : MonoBehaviour
{
    //This allows the use of an animation curve to dictate the vision values. 
    //Ie perifierals add less while direct center adds a lot
    [SerializeField] AnimationCurve VisionSensitivity;
    [SerializeField] float VisionBaseAwareness = 1; //Min awareness
    [SerializeField] float VisionAwarnessRate = 10; //aggro gained per second

    [SerializeField] float HearingBaseAwareness = 0;
    [SerializeField] float HearingAwarenessRate = 1;

    [SerializeField] float ProximityBaseAwareness = 0;
    [SerializeField] float ProximityAwarenessRate = 2;

    [SerializeField] float DecayRate = 0.1f;
    [SerializeField] float DecayDelay = 0.1f;

    //Basically hashmap
    Dictionary<GameObject, TrackedTarget> targets = new Dictionary<GameObject, TrackedTarget>();
    EnemyAI currAI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currAI = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        List<GameObject> toCleanUp = new List<GameObject>();
        foreach (var targetGameObject in targets.Keys)
        {
            Debug.Log("Threshold change for " + targetGameObject.name + " " + targets[targetGameObject].awareness);
            //If detection = 0 remove
            if (targets[targetGameObject].DecayAwareness(DecayDelay, DecayRate * Time.deltaTime))
            {
                if (targets[targetGameObject].awareness <= 0)
                {
                    currAI.loseFull();
                    toCleanUp.Add(targetGameObject);
                }
                else
                {
                    if(targets[targetGameObject].awareness >= 1)
                    {
                        currAI.loseDetection(targetGameObject);
                    }
                    else
                    {
                        currAI.loseSuspicion();
                    }
                    }
            }
        }

        foreach (var target in toCleanUp)
        {
            targets.Remove(target);
        }
    }

    public void CanSee(Detectable detectable)
    {
        //AI to detectable (player)
        var vectorToTarget = (detectable.transform.position - currAI.EyeLocation).normalized;
        //1 = directly looking, 0 = directly to side of looking, -1 = behind looking direction
        var dotProd = Vector3.Dot(vectorToTarget, currAI.EyeDirection);
        //Send value to the curve to get how much aggro to gain
        //Multiply to get aggro gained this frame
        var awareness = VisionSensitivity.Evaluate(dotProd) * VisionAwarnessRate * Time.deltaTime;

        UpdateAwareness(detectable.gameObject, detectable, detectable.transform.position, awareness, VisionBaseAwareness);
    }

    public void UpdateAwareness(GameObject targetGameObject, Detectable detectable, Vector3 position, float awareness, float minAwareness)
    {
        //Not in list then add to list
        if (!targets.ContainsKey(targetGameObject))
        {
            targets[targetGameObject] = new TrackedTarget();
        }
        //Update awarness in target
        if (targets[targetGameObject].UpdateAwareness(detectable, position, awareness, minAwareness))
        {
            if(targets[targetGameObject].awareness >= 2)
            {
                currAI.gainFull(targetGameObject);
            }else if(targets[targetGameObject].awareness >= 1)
            {
                currAI.gainDetection(targetGameObject);
            }
            else
            {
                currAI.gainSuspicion();
            }
            //Debug.Log("Threshold change for " + targetGameObject.name + " " + targets[targetGameObject].awareness);
        }
    }

    public void CanHear(GameObject source, Vector3 location, HeardSoundType soundType, float loudness)
    {
        //Aggo per frame
        var awareness = loudness * HearingAwarenessRate * Time.deltaTime;

        UpdateAwareness(source, null, location, awareness, HearingBaseAwareness);
    }

    public void CanDetectProximity(Detectable detectable)
    {
        var awareness = ProximityAwarenessRate * Time.deltaTime;

        UpdateAwareness(detectable.gameObject, detectable, detectable.transform.position, awareness, ProximityBaseAwareness);
    }
}

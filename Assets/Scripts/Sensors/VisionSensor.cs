using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class VisionSensor : MonoBehaviour
{   
    
    [SerializeField] LayerMask DetectionMask = ~0; //~0 detects everything

    EnemyAI currAI;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currAI = GetComponent<EnemyAI>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (DetectableManager.Instance == null)
            return;
        */
        if (currAI == null)
            return;

        //Check all detectable entities
        //Debug.Log("VisionSensor: Checking detectables..."+ DetectableManager.Instance.AllDetectables.Count);
        for (int i = 0; i< DetectableManager.Instance.AllDetectables.Count; i++)
        {
            Detectable target = DetectableManager.Instance.AllDetectables[i];

            if(target == null || target.gameObject == this.gameObject)
            {
                continue; //Skip self and null targets
            }

            //Check distance, then check if its in vision cone, then do raycast bc raycast is expensive
            //EyeLocation is just currAI.transform.position
            Vector3 vectorToTarget = target.transform.position - currAI.EyeLocation;

            //If out of range then cant detect
            if(vectorToTarget.sqrMagnitude > (currAI.VisionConeRange * currAI.VisionConeRange))
            {   //Optimization to avoid sqrt calculation
                continue;
            }

            //If not in cone (fov) then cant detect
            //Basically take dot product and compare to cosine and if its less than the angle then its outside fov
            //EyeDirection is just target.transform.forward
            if(Vector3.Dot(vectorToTarget.normalized, currAI.EyeDirection) <= currAI.CosVisionConeAngle)
            {
                continue;
            }
            //If pass all conditions then has line of sight so can raycast
            RaycastHit hitInfo;
            //Send Ray from current Eye location and direction max VisionConeRange
            //Should be fine to direct towards target since it already passed prev conditions
            Debug.DrawRay(currAI.EyeLocation, currAI.EyeDirection, Color.blue, 1.0f);
            
            if(Physics.Raycast(currAI.EyeLocation, vectorToTarget.normalized, out hitInfo, 
                                currAI.VisionConeRange, DetectionMask, QueryTriggerInteraction.Collide))
            {
                if(hitInfo.collider.gameObject == target.gameObject)
                {
                    currAI.CanSee(target);
                }
                
            }

        }
    }
}

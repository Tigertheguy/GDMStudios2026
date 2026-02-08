using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]
public class ProximitySensor : MonoBehaviour
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
      for(int i=0;i < DetectableManager.Instance.AllDetectables.Count; i++)
        {
            var target = DetectableManager.Instance.AllDetectables[i];

            //Skip self and null targets
            if(target == null || target.gameObject == this.gameObject)
            {
                continue; 
            }
            if(Vector3.Distance(transform.position, target.transform.position) 
                <= currAI.ProximityDetectionRange)
            {
                currAI.CanDetectProximity(target);
            }
        }
    }
}

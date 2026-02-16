using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[RequireComponent(typeof(EnemyAI))]


public class HearingSensor : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HearingManager.Instance.RegisterDetectable(this);
    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnDestroy()
    {
        if (HearingManager.Instance != null)
        {
            HearingManager.Instance.UnregisterDetectable(this);
        }
    }

    public void OnHearSound(GameObject source, Vector3 location, HeardSoundType soundType, float loudness)
    {
        //In range
        if (Vector3.Distance(transform.position, location) <= GetComponent<EnemyAI>().HearingRange)
        {
            GetComponent<EnemyAI>().CanHear(source, location, soundType, loudness);
        }//Out of range
        else
        {
            return;
        }
    }
}

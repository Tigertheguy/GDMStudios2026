using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HeardSoundType
{
    FloorRun,
    FloorWalk,
    FloorCrouch,
    BushRun,
    BushWalk,
    BushCrouch,
    Other
}

public class HearingManager : MonoBehaviour
{

    public static HearingManager Instance { get; private set; } = null; //Singleton ie only one can ever exist

    public List<HearingSensor> AllSensors { get; private set; } = new List<HearingSensor>(); //List of all detectable entities

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Debug.LogError("Multiple DetectabkeManager instances detected. Destroying this duplicate instance.", this);
            Destroy(this.gameObject); //You should Destroy(); yourself now!!!
            return;
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    //Add to list
    public void RegisterDetectable(HearingSensor sensor)
    {
        AllSensors.Add(sensor);
    }
    //Remove from list
    public void UnregisterDetectable(HearingSensor sensor)
    {
        AllSensors.Remove(sensor);
    }

    
    //For any sound emitted
    public void OnSoundEmitted(Vector3 location, HeardSoundType soundType, float loudness)
    {
        //Notify all sensors of the sound
        foreach(var sensor in AllSensors)
        {
            sensor.OnHearSound(location, soundType, loudness);
        }
    }
}

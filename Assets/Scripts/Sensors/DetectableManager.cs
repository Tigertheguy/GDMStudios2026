using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectableManager : MonoBehaviour
{

    public static DetectableManager Instance { get; private set; } = null; //Singleton ie only one can ever exist

    public List<Detectable> AllDetectables { get; private set; } = new List<Detectable>(); //List of all detectable entities

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
    public void RegisterDetectable(Detectable detectable)
    {
        AllDetectables.Add(detectable);
    }
    //Remove from list
    public void UnregisterDetectable(Detectable detectable)
    {
        AllDetectables.Remove(detectable);
    }
}

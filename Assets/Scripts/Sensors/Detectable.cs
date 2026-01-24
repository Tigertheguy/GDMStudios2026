using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Detectable : MonoBehaviour
{
    void Start()
    {
        DetectableManager.Instance.RegisterDetectable(this);
    }

    void Update()
    {
        
    }

    void OnDestroy()
    {
        if(DetectableManager.Instance != null)
        {
            DetectableManager.Instance.UnregisterDetectable(this);
        }
    }   
}

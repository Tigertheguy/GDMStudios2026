using System;
using UnityEngine;

public class PlayerSanity : MonoBehaviour
{
    [SerializeField] private float _maxSanity = 100f;
    [SerializeField] private float _currentSanity = 100f;
    [SerializeField] private float _sanityIncreaseRate = 1f; //natural recovery rate
    [SerializeField] private float _sanityRecoverBuffer = 3.0f; //Time after being near enemy to passively get sanity back
    private float _lastNearEnemy = 0f;

    public void DrainSanity(float amount)
    {   
        _lastNearEnemy = Time.time;
        //Need to call it with time.deltaTime scalled call
        _currentSanity -= amount * Time.deltaTime;
        //Keeps number withing range 0-maxSanity
        _currentSanity = Mathf.Clamp(_currentSanity, 0f, _maxSanity);
    }

    public void GainSanity(float amount)
    {
        _currentSanity += amount * Time.deltaTime;
        _currentSanity = Mathf.Clamp(_currentSanity, 0f, _maxSanity);
    }

    //For bush guy maybe when he jumps you you get giga scared
    public void FlatSanityIncrease(float amount)
    {
        _currentSanity += amount;
        _currentSanity = Mathf.Clamp(_currentSanity, 0f, _maxSanity);
    }

    public float GetSanity(){
        return _currentSanity;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("Current Sanity: " + _currentSanity);
        //After a delay and no enemies around gain a little sanity back
        if(Time.time > _lastNearEnemy + _sanityRecoverBuffer)
        {
            GainSanity(_sanityIncreaseRate);
        }
    }

    public float MaxSanity => _maxSanity;

}

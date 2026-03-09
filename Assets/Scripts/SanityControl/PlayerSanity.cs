using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSanity : MonoBehaviour
{
    [SerializeField] private InputActionReference _meditateAction;

    [Header("Sanity Settings")]
    [SerializeField] private float _maxSanity = 100f;
    [SerializeField] private float _currentSanity = 100f;
    [SerializeField] private float _sanityIncreaseRate = 1f; //natural recovery rate
    [SerializeField] private float _sanityRecoverBuffer = 3.0f; //Time after being near enemy to passively get sanity back

    [Header("Meditation Settings")]
    [SerializeField] private float _meditationRegenRate = 5f;
    [SerializeField] private int _meditationCap = 3;
    [SerializeField] private float _meditationDurationMax = 5f; //Max 5s meditation
    [SerializeField] private float _rampThreshold = 1f; //Time to ramp meditation regen
    [SerializeField] private float _rampingStrength = 0.5f; //How strong the ramping is, 0.5 means after 1s you get 50% more regen, after 2s you get 100% more regen etc

    private float _lastNearEnemy = 0f;
    private bool _isMeditating = false;
    private int _currentMeditationCount = 0;
    private float _meditationTimer = 0f;


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

    public float GetSanity()
    {
        return _currentSanity;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void OnEnable()
    {
        if (_meditateAction != null)
            _meditateAction.action.Enable();
    }

    private void OnDisable()
    {
        if (_meditateAction != null)
            _meditateAction.action.Disable();
    }

    // Update is called once per frame
    void Update()
    {
        MeditationDetection();
        //Debug.Log("Current Sanity: " + _currentSanity);
        //After a delay and no enemies around gain a little sanity back
        if (!_isMeditating && Time.time > _lastNearEnemy + _sanityRecoverBuffer)
        {
            GainSanity(_sanityIncreaseRate);
        }
    }

    private void MeditationDetection()
    {
        if (_meditateAction.action.WasPerformedThisFrame() && _currentMeditationCount < _meditationCap)
        {
            _currentMeditationCount++;
            _meditationTimer = 0f; //Reset timer for ramping
            //Debug.Log("Meditating");
        }
        //Q being held
        bool holding = _meditateAction.action.IsPressed() && _meditateAction.action.phase == InputActionPhase.Performed;
        bool underTime = _meditationTimer < _meditationDurationMax;
        bool underCap = _currentMeditationCount <= _meditationCap;

        if(holding && underTime && underCap){
            _isMeditating = true;
            _meditationTimer += Time.deltaTime;

            if(_meditationTimer > _rampThreshold)
            {
                float rampedRegen = _meditationRegenRate + (_meditationRegenRate * _meditationTimer * _rampingStrength);
                GainSanity(rampedRegen);
            }
            else
            {
                GainSanity(_meditationRegenRate);
            }

        }else{
            _isMeditating = false;
        }
        if (_isMeditating == true)
        {
            GainSanity(_meditationRegenRate);
        }


    }

    public void ResetMeditationCount()
    {
        _currentMeditationCount = 0;
    }
    public void SetMeditationCap(int count)
    {
        _meditationCap = count;
    }

    public float MaxSanity => _maxSanity;
    public int CurrentMeditationCount => _currentMeditationCount;
    public int MeditationCap => _meditationCap;
    public bool IsMeditating => _isMeditating;
    public float MeditationTimer => _meditationTimer;

}

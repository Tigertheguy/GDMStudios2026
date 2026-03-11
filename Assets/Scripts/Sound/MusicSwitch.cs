using UnityEngine;

public class MusicSwitch : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Switch SaneSwitch;
    [SerializeField] private AK.Wwise.Switch ShakenSwitch;
    [SerializeField] private AK.Wwise.Switch DisturbedSwitch;
    [SerializeField] private AK.Wwise.Switch InsaneSwitch;

    [SerializeField] private PlayerSanity _playerSanity;

    [SerializeField] private AK.Wwise.Event LowPassOnEvent;
    [SerializeField] private AK.Wwise.Event LowPassOffEvent;

    private bool lowPassStart = false;

    // Update is called once per frame
    void Update()
    {
        var curSanity = _playerSanity.GetState();
        if(curSanity == 0) // Sane
        {
            SaneSwitch.SetValue(gameObject);
        }
        else if(curSanity == 1) // Shaken
        {
            ShakenSwitch.SetValue(gameObject);
        } 
        else if(curSanity == 2) // Disturbed
        {
            DisturbedSwitch.SetValue(gameObject);      
        } 
        else // Insane
        { 
            InsaneSwitch.SetValue(gameObject);
        }
    }

    public void StartLowPass()
    {
        if(lowPassStart)
        {
            return;
        }
        LowPassOnEvent.Post(gameObject);
        lowPassStart = true;
    }

    public void ResetLowPass()
    {
        if(!lowPassStart)
        {
            return;
        }
        
        LowPassOffEvent.Post(gameObject);
        lowPassStart = false;
    }
}

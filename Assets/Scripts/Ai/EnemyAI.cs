using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif // checks if you are in the editor and runs the code inside the block. If not then wont run


public class EnemyAI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI FeedbackDisplay;
    
    [SerializeField] private float _VisionConeAngle = 60f;
    [SerializeField] private float _VisionConeRange = 30f;
    [SerializeField] private Color _VisionConeColour = new Color(1f, 0f, 0f, 0.25f);

    [SerializeField] private float _HearingRange = 20f;
    [SerializeField] private Color _HearingRangeColour = new Color(1f, 1f, 0f, 0.25f);

    [SerializeField] private float _ProximityDetectionRange = 3f;
    [SerializeField] private Color _ProximityRangeColour = new Color(1f, 1f, 1f, 0.25f);

    //Getters
    public float VisionConeRange { get { return _VisionConeRange; } }
    public float VisionConeAngle { get { return _VisionConeAngle; } }
    public Color VisionConeColour { get { return _VisionConeColour; } }

    public float HearingRange { get { return _HearingRange; } }
    public Color HearingRangeColour { get { return _HearingRangeColour; } }

    public float ProximityDetectionRange { get { return _ProximityDetectionRange; } }
    public Color ProximityDetectionColour { get { return _ProximityRangeColour; } }

    public float CosVisionConeAngle { get; private set; } = 0f;

    //Set origin point for raycasting to current position
    public Vector3 EyeLocation { get { return transform.position; } }
    //Set direction for raycasting to current forward direction
    public Vector3 EyeDirection { get {return transform.forward; } }


    void Awake()
    {
        CosVisionConeAngle = Mathf.Cos(VisionConeAngle * Mathf.Deg2Rad );
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CanSee(Detectable detectable) 
    {
        Debug.Log("Can see " + detectable.gameObject.name);
    }

    public void CanHear(Vector3 location, HeardSoundType soundType, float loudness)
    {
        Debug.Log("Heard sound of type " + soundType + " at location " + location + " with loudness " + loudness);
    }

    public void CanDetectProximity(Detectable detectable)
    {
        Debug.Log("In proximity of " + detectable.gameObject.name);
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(EnemyAI))]
public class EnemyAIEditor : Editor
{
    public void OnSceneGUI()
    {
        var ai = target as EnemyAI;

        // draw the detectopm range
        Handles.color = ai.ProximityDetectionColour;
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.ProximityDetectionRange);

        // draw the hearing range
        Handles.color = ai.HearingRangeColour;
        Handles.DrawSolidDisc(ai.transform.position, Vector3.up, ai.HearingRange);

        // work out the start point of the vision cone
        Vector3 startPoint = Mathf.Cos(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.forward +
                             Mathf.Sin(-ai.VisionConeAngle * Mathf.Deg2Rad) * ai.transform.right;

        // draw the vision cone
        Handles.color = ai.VisionConeColour;
        Handles.DrawSolidArc(ai.transform.position, Vector3.up, startPoint, ai.VisionConeAngle * 2f, ai.VisionConeRange);        
    }
}
#endif 

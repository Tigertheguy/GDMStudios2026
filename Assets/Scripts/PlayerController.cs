using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //Can edit in unity but remains private
    [Header("Player Settings")]
    [SerializeField] private float playerSpd = 3f;
    [SerializeField] private float player_Y_Offset = 1f;
    //[SerializeField] private float gravity = -9.8f;

    [Header("Sound Settings")]
    [SerializeField] private float stepInterval = 0.5f;
    [SerializeField] private float walkLoudness = 10f;

    [SerializeField] private AK.Wwise.Event startWalkEvent;
    [SerializeField] private AK.Wwise.Event stopWalkEvent;
    private float stepTimer;

    [SerializeField] private PlayerSanity playerSanity;

    public LayerMask terrainLayer;
    //public Rigidbody rb;
    public SpriteRenderer sr;
    public Animator animator;

    private CharacterController controller;
    private Vector3 moveInput;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //rb = gameObject.GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();


        // start facing the front 
        animator.SetFloat("LastMoveX", 0f);
        animator.SetFloat("LastMoveY", -1f);
    }

    //New unity input system
    public void OnMove(InputAction.CallbackContext context)
    {
        //only reads 2d vector
        moveInput = context.ReadValue<Vector2>();
        if(context.started){
            startWalkEvent.Post(gameObject);
        } else if(context.canceled){
            stopWalkEvent.Post(gameObject);
        }
        //Logs input in console
        //Debug.Log($"Move Input: {moveInput}");
        //prevents player from  moving faster diagonally
        if (moveInput.sqrMagnitude > 1) 
        {
        moveInput.Normalize();
        }
    }

    // Update is called once per frame.
    // There is also fixed update which can run multiple times per frame I think
    // I think update is fine for our purposes
    void Update()
{
    if(playerSanity.IsMeditating)
    {
        // Meditation animations?
        stepTimer = 0;
        return;
    }

    // 1. Calculate movement
    Vector3 move = new Vector3(moveInput.x, 0, moveInput.y);
    controller.Move(move * playerSpd * Time.deltaTime);

    // 2. Update the standard Walking parameters
    animator.SetFloat("MoveX", moveInput.x);
    animator.SetFloat("MoveY", moveInput.y);
    animator.SetFloat("Speed", moveInput.magnitude);

    
    // use a small buffer (0.1f) to ensure we caught the last direction
    if (moveInput.magnitude > 0.1f)
    {
        animator.SetFloat("LastMoveX", moveInput.x);
        animator.SetFloat("LastMoveY", moveInput.y);
    }
    
    HandleSoundEmission();
    oldRaycast();
}

    void oldRaycast()
    {
        //Shoots line from player directly downwards
        RaycastHit hit;
        Vector3 castPos = transform.position;
        //Set to start a bit above player
        castPos.y += 1;

        //If hits terrain then move player set distance above terrain
        if (Physics.Raycast(castPos, -transform.up, out hit, Mathf.Infinity, terrainLayer))
        {
            if (hit.collider != null)
            {
                Vector3 movePos = transform.position;
                movePos.y = hit.point.y + player_Y_Offset;
                transform.position = movePos;
            }
        }

        float x = moveInput.x;
        //float y = moveInput.y;
        //Vector3 playerDir = new Vector3(x, 0, z); //only want x and z axis. Maybe add jump later
        //rb.Velocity = playerDir * playerSpd;


    }
    void HandleSoundEmission()
    {
        //If moving fast
        if (controller.velocity.magnitude > 0.1f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0)
            {
                //Notify HearingManager
                if (HearingManager.Instance != null)
                {
                    //Depends on sound type and intensity which should be based on distance
                    HearingManager.Instance.OnSoundEmitted(gameObject, transform.position, HeardSoundType.FloorWalk, walkLoudness);
                }
                //Reset timer
                stepTimer = stepInterval;
            }
        }
        else
        {
            //Stopped so set to 0 to play sound immediatley on move
            stepTimer = 0;
        }
    }
}

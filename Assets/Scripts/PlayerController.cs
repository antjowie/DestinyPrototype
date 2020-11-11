// Quite a lot is inspired from Dani's video about his movement in Karlson
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 20f;
    public float accel = 1000f;
    public float decel = 1000f;

    public GameObject weapon;
    public GameObject weaponAttach;
    public new GameObject camera;
    public GameObject cameraTarget;
    public GameObject orientation;

    // Components
    Animator animator;
    PlayerInput input;
    Rigidbody rb;

    // Animation hashes
    int horizontalHash;
    int verticalHash;
    int isMovingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        input = GetComponent<PlayerInput>();
        rb = GetComponent<Rigidbody>();

        horizontalHash = Animator.StringToHash("horizontal");
        verticalHash = Animator.StringToHash("vertical");
        isMovingHash = Animator.StringToHash("isMoving");

        // Spawn weapon
        weapon = Instantiate(weapon);
        weapon.transform.parent = weaponAttach.transform;
        weapon.transform.localPosition = Vector3.zero;
        weapon.transform.localRotation = Quaternion.Euler(90,0,0);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        // Rotate player based on camera
        Vector3 forward = cameraTarget.transform.position - camera.transform.position;
        forward = Vector3.ProjectOnPlane(forward,Vector3.up).normalized;
        
        orientation.transform.rotation = Quaternion.LookRotation(forward);
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        // Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        // Get our intended movement 
        Vector3 vel = GetVelRelToLook();
        Vector3 move = GetMoveInput();

        CounterMovement(move, vel);
        
        // vel = rb.velocity;
        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (move.x > 0 && vel.x > maxSpeed) move.x = 0;
        if (move.x < 0 && vel.x < -maxSpeed) move.x = 0;
        if (move.z > 0 && vel.z > maxSpeed) move.z = 0;
        if (move.z < 0 && vel.z < -maxSpeed) move.z = 0;

        // Apply movement
        move = orientation.transform.rotation * move * accel;
        rb.AddForce(move * Time.deltaTime);

        // Update animations from movement
        // vel = rb.velocity / maxSpeed;
        
        // Inverse rotation of velocity
        // vel = Quaternion.Inverse(transform.rotation) * vel;
        animator.SetBool(isMovingHash, vel.sqrMagnitude >= 0.05f || move.sqrMagnitude != 0);
        animator.SetFloat(horizontalHash, vel.x);
        animator.SetFloat(verticalHash, vel.z);
    }
    
    void CounterMovement(Vector3 input, Vector3 relVel)
    {
        // Inverse velocity to simplify calculating intended velocity 
        // (and check things like decel on a per axis basis and prevent overshoot)
        Debug.DrawRay(orientation.transform.position,relVel * 10f);

        // relVel = Quaternion.Inverse(orientation.transform.rotation) * rb.transform.forward;
        // Calculate counter forces
        // NOTE: Does not take weight into consideration atm
        float bias = 0.1f;
        // We check 3 things to apply counter force
        // Does player no longer want to move? 
        // Does player move in opposite direction (first left, then right)
        if ((input.x == 0 && Mathf.Abs(relVel.x) > bias) || (input.x < 0 && relVel.x > bias) || (input.x > 0 && relVel.x < bias))
        {
            Vector3 counter = orientation.transform.right * decel * -relVel.x * Time.deltaTime;
            Debug.DrawRay(orientation.transform.position,counter * 10f, Color.red);
            
            rb.AddForce(counter);
        }
        if ((input.z == 0 && Mathf.Abs(relVel.z) > bias) || (input.z < 0 && relVel.z > bias) || (input.z > 0 && relVel.z < bias))
        {
            Vector3 counter = orientation.transform.forward * decel * -relVel.z * Time.deltaTime;
            Debug.DrawRay(orientation.transform.position,counter * 10f, Color.blue);
            
            rb.AddForce(counter);
        }
        
        //Limit diagonal running. This will also cause a full stop if sliding fast and un-crouching, so not optimal.
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed) {
            float fallspeed = rb.velocity.y;
            Vector3 n = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(n.x, fallspeed, n.z);
        }
    }
    
    Vector3 GetMoveInput()
    {
        Vector3 move = input.GetMoveInput();
        return move;
    }
    
    Vector3 GetVelRelToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float yMag = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);
        float xMag = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);
        
        return new Vector3(xMag, 0f, yMag);
    }
}
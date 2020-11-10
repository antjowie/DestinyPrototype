using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 20f;
    public float accel = 1000f;
    public float decel = 1000f;

    public GameObject weapon;
    public GameObject weaponAttach;

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
    }

    void FixedUpdate()
    {
        Movement();
    }

    void Movement()
    {
        // Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);
        Vector3 vel = rb.velocity;

        // Calculate movement 
        Vector3 move = GetMoveRelToLook();

        // Calculate counter forces
        float bias = 0.5f;
        if (move.x == 0)
        {
            if (Mathf.Clamp(vel.x, -bias, bias) != vel.x)
            {
                rb.AddForce(rb.transform.right * decel * -Mathf.Clamp(vel.x, -1, 1) * Time.deltaTime);
            }
        }
        if (move.z == 0)
        {
            if (Mathf.Clamp(vel.z, -bias, bias) != vel.z)
            {
                rb.AddForce(rb.transform.forward * decel * -Mathf.Clamp(vel.z, -1, 1) * Time.deltaTime);
            }
        }

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (move.x > 0 && vel.x > maxSpeed) move.x = 0;
        if (move.x < 0 && vel.x < -maxSpeed) move.x = 0;
        if (move.z > 0 && vel.z > maxSpeed) move.z = 0;
        if (move.z < 0 && vel.z < -maxSpeed) move.z = 0;

        // Apply movement
        move *= accel;
        rb.AddForce(move * Time.deltaTime);

        // Update animations from movement
        vel = rb.velocity / maxSpeed;
        animator.SetBool(isMovingHash, vel.sqrMagnitude >= 0.05f || move.sqrMagnitude != 0);
        animator.SetFloat(horizontalHash, vel.x);
        animator.SetFloat(verticalHash, vel.z);

        //Some multipliers
        // float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        // if (!grounded) {
        //     multiplier = 0.5f;
        //     multiplierV = 0.5f;
        // }

        // Movement while sliding
        // if (grounded && crouching) multiplierV = 0f;

        //Apply forces to move player
        // rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        // rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }
    
    Vector3 GetMoveRelToLook()
    {
        Vector3 move = input.GetMoveInput();
        // TODO rotate this according to look dir
        return move;
    }
}
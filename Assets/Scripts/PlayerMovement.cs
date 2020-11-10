using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bool up = Input.GetKey("w");
        bool down = Input.GetKey("a");
        bool left = Input.GetKey("s");
        bool right = Input.GetKey("d");

        Vector2 move = new Vector2();
    }
}

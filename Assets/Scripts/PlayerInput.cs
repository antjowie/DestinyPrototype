using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool CanProcessInput()
    {
        return Cursor.lockState == CursorLockMode.Locked;// && !m_GameFlowManager.gameIsEnding;
    }

    // Returns desired move input clamped to 1
    public Vector3 GetMoveInput()
    {
        if(CanProcessInput())
        {
            Vector3 move = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
            move = Vector3.ClampMagnitude(move,1f);
            
            return move;
        }
        
        return Vector3.zero;
    }
    
    public bool GetJump()
    {
        if(CanProcessInput())
        {
            return Input.GetButton("Jump");
        }
        
        return false;
    }
    
    public bool GetCrouch()
    {
        if(CanProcessInput())
        {
            return Input.GetKey(KeyCode.LeftControl);
        }
        
        return false;
    }

    public bool GetCrouchDown()
    {
        if(CanProcessInput())
        {
            return Input.GetKeyDown(KeyCode.LeftControl);
        }
        
        return false;
    }

    public bool GetCrouchUp()
    {
        if(CanProcessInput())
        {
            return Input.GetKeyUp(KeyCode.LeftControl);
        }
        
        return false;
    }
}

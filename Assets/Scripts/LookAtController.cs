using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LookAtController : MonoBehaviour
{
    public Transform origin;
    public Rig rig;
    public float smoothTime = 0.1f;

    public void UpdatePosition(Quaternion viewRotation)
    {
        transform.position = origin.position + viewRotation * Vector3.forward;
    }

    // If the player is moving, we want to stop using the aim rig
    float velocity;
    public void UpdateMoving(bool isMoving)
    {
        rig.weight = Mathf.SmoothDamp(rig.weight, isMoving ? 0f : 1f, ref velocity, smoothTime, 10f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset = Vector3.zero;
    public float smoothTime = 0.2f;
    
    Vector3 velocity = Vector3.zero;

    // void Start()
    // {
    //     // transform.parent = null;
    //     if(transform.parent != null)
    //         Debug.LogWarning($"{transform.gameObject.name} should not be parented");
    // }
    
    void Update()
    {
        // Vector3 delta = target.position - transform.position;
        Vector3 targetPos = target.TransformPoint(offset);
        
        transform.position = Vector3.SmoothDamp(transform.position,targetPos,ref velocity, smoothTime);
    }
}

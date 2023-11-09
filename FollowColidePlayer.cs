using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowColidePlayer : MonoBehaviour
{
    // Start is called before the first frame 
    
    public Vector3 offset;
    public float smoothFactor = 2;
    private Transform Target;
    private Rigidbody2D rb;
    void Start(){
        rb = GetComponent<Rigidbody2D>();
    }
    private void LateUpdate(){
        Follow();
    }
    void Follow(){
        Vector3 targetPosition = Target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(this.transform.position, targetPosition, smoothFactor*Time.fixedDeltaTime);
        rb.position = targetPosition;
    }
    public void SetgameObject(Transform target){
        Target = target;
    }
}

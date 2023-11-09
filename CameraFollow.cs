using UnityEngine;
public class CameraFollow : MonoBehaviour
{
    public Vector3 offset;
    [Range(1,10)]
    public float smoothFactor;
    private Transform Target;
    private void LateUpdate(){
        Follow();
        
    }
    void Follow(){
        Vector3 targetPosition = Target.position + offset;
        Vector3 smoothPosition = Vector3.Lerp(this.transform.position, targetPosition, smoothFactor*Time.fixedDeltaTime);
        this.transform.position = targetPosition;
    }
    public void SetgameObject(Transform target){
        Target = target;
    }
}

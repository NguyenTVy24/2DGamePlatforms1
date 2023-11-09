using UnityEngine;
public class FollowAQ : MonoBehaviour
{
    public Vector3 offset;
    public float smoothFactor = 2;
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

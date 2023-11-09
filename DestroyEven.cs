using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEven : MonoBehaviour
{
    // Destroy particles when animation has finished playing. 
    // destroyEvent() is called as an event in animations.
    private GameObject parSpawen;
    public void SetparSpawen(GameObject parSpawen){
        Debug.Log(parSpawen);
        this.parSpawen = parSpawen;
    }
    public void destroyEvent()
    {
        //DestroyImmediate(gameObject,true);
        if(parSpawen){
            parSpawen.GetComponent<spawenemy>().SetSpawen(true);
        }
        Destroy(gameObject);
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDoor : MonoBehaviour
{
    // Start is called before the first frame update
    Animator anim;
    void Start(){
        anim = transform.GetComponent<Animator>();
        GetComponent<Collider2D>().enabled = false;
    }
    public void CloseTheDoor()
    {
        anim.SetBool("Door",true);
        GetComponent<Collider2D>().enabled = true;
    }

    // Update is called once per frame
    public void OpenTheDoor()
    {
        anim.SetBool("Door",false);
        GetComponent<Collider2D>().enabled = false;
    }
}

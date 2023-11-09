using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDavo : MonoBehaviour
{
    private Animator anim;
    void Start(){
        anim = transform.GetComponent<Animator>();
    }
    public void BlockDertroy(){
        anim.SetBool("hit",true);
        this.transform.GetComponent<Collider2D>().enabled=false;
    }
}

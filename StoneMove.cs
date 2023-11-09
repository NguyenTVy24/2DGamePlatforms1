using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneMove : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private Animator anim;
    public float high;
    public float speeddrop;
    public float dropTime;
    private float nextdropTime = 0f;
    private bool droped;
    private bool delayed;
    private float delaydroptime = 1.8f;
    private float nextdelaydroptime = 0f;
    private float GoOnTime = 5.5f;
    private float nextGoOnTime = 0f;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    public void SetDropStone(){
        if(!droped){
            nextdropTime = dropTime;
            nextdelaydroptime = delaydroptime;
            nextGoOnTime = GoOnTime;
            droped = true;
            delayed = false;
        }
    }
    void Update()
    {
        if(nextdropTime > 0f){
            if(nextdelaydroptime > 0f){
                if(!delayed){
                    anim.SetTrigger("Drop");
                    delayed = true;
                }
                nextdelaydroptime -= Time.deltaTime;
            }else{
                nextdropTime -= Time.deltaTime;
                rb.position = new Vector2(rb.position.x,rb.position.y-speeddrop);
            }   
        }else{
            if(rb.position.y < high){
                if(nextGoOnTime > 0f){
                    nextGoOnTime -= Time.deltaTime;
                }else{
                    rb.position = new Vector2(rb.position.x,rb.position.y+speeddrop);
                    if(droped){
                        droped = false;
                    }
                }
            }
            
        }
    }
}

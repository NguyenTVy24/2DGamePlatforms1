using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class goOnGround : MonoBehaviour
{
    // Start is called before the first frame update
    public float speedOnGround;
    private caidatdiemtua onGround;
    private Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        onGround = transform.Find("diemtua").GetComponent<caidatdiemtua>();
    }
    // Update is called once per frame
    public bool GoOnGround(){
        if(!onGround.State()){
            rb.velocity = new Vector2(0,speedOnGround*(-1));
            return false;
        }
        rb.velocity = new Vector2(0,0);
        return true;
    }
}

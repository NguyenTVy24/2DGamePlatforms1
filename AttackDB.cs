using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDB : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public  int attackDamage;
    private Rigidbody2D rb;
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
    }
    void Start(){
    }
    // Update is called once per frame
    public void SetisfacingRight(float LR){
        Vector3 kich_thuoc = transform.localScale;
        kich_thuoc.x = LR;
        transform.localScale = kich_thuoc;
        rb.AddForce(new Vector2(1,0) * speed * LR, ForceMode2D.Impulse);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player")){
           ControPlayer.Ins.SetTakeDamagePlayer(attackDamage,this.name);
        }
    }
}

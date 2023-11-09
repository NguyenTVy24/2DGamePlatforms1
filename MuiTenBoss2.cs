using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MuiTenBoss2 : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    public  int attackDamage;
    public float delayspaw;
    private float nextdelayspaw = 0f;
    Vector3 positionPlayer;
    Vector2 velocityPlayer;
    float rotationSkill;
    float Pi = 3.1415926535f;
    private Rigidbody2D rb;
    private Animator anim;
    void Awake(){
        rb = GetComponent<Rigidbody2D>();
        anim =GetComponent<Animator>();
    }
    void Start(){
    }
    // Update is called once per frame
    public void SetisfacingRight(){
        positionPlayer = ControPlayer.Ins.GetpositionPlayer();
        float GoAttackX = positionPlayer.x-rb.position.x;
        float GoAttackY = positionPlayer.y-rb.position.y;
        float maxposition = Mathf.Max(Mathf.Abs(GoAttackX),Mathf.Abs(GoAttackY));
        velocityPlayer = new Vector2((GoAttackX/maxposition),(GoAttackY/maxposition));
        //tao luc huong ve nguoi choi
        //xoay mui ten ve nguoi choi (0,-1)
        float SoGoc= Mathf.Acos((velocityPlayer.y*-1)/(Mathf.Sqrt(velocityPlayer.x*velocityPlayer.x+velocityPlayer.y*velocityPlayer.y)*Mathf.Sqrt(1)));
        Debug.Log(SoGoc*180/Pi);
        if(velocityPlayer.x < 0){
            rb.rotation = -1*(SoGoc*180/Pi+135); 
        }else{
            rb.rotation = SoGoc*180/Pi-135; 
        }
        anim.SetTrigger("HinhT");
    }
    void Update(){
        nextdelayspaw += Time.deltaTime;
        if(nextdelayspaw >= delayspaw){
            rb.AddForce(velocityPlayer * speed, ForceMode2D.Impulse);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player")){
           ControPlayer.Ins.SetTakeDamagePlayer(attackDamage,this.name);
        }
    }
}

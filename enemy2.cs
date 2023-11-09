using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy2 : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 100;
    //mau hien tai
    int currentHealth;
    //toc do
    public float speed;
    
    //thong tin tam nhin
    public float visionRange = 0.5f;
    public Transform visionPoint;
    //muc tieu
    public LayerMask playerLayers;
    //dame quai
    public int attackDamage;
    //thiet lap di chuyen
    public float moveTime = 2f;
    public float restTime = 2f;
    private bool ChangeStatus = true;
    private bool ChangeStatusLR = true;
    private float nextChangeStatusTime = 0f ;
    //thiet lap tang cong
    public float speedGoAttack;
    private Vector2 positionPlayer;
    private bool canAttack;
    //thong tin tang cong
    public float attackRange = 0.5f;
    public  float attackRate = 2f;
    private float nextattackTime = 0f;
    public float AttackRangeR = 0.4f;
    public Transform attackPoint;
    public Transform AttackRange;
    //thiet lap bi choang
    public float stunnedTime = 3f;  
    private bool stunned = false;
    private float nextstunedTime = 0;
    private bool attacked = false;
    //thiet lam animation tang cong
    private bool attack1 = true;
    private float DelayAttackTime = .8f;
    private float nextDelayAttack = 0f;
    private bool DelayAttack = true;
    //
    private Animator anim;
    private Rigidbody2D rb;
    private caidatdiemtua groundL;
    private float left_right;
    private bool isfacingRight=false;
    private bool seePlayer;
    private bool life;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundL = transform.Find("groundL").GetComponent<caidatdiemtua>();
        currentHealth=maxHealth;
        life = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(life){
            StunnedDelay();
            if(!stunned){
                aIEnemyMove();
                anim.SetFloat("move",Mathf.Abs(left_right));
            }
        }
    }
    void StunnedDelay(){
        if(Time.time>=nextstunedTime){
            stunned=false;
        }
    }
    void aIEnemyMove(){
        scanPlayer();
        if(!seePlayer||!ControPlayer.Ins.GetlifePlayer()){
            AimoveLeft_right();
        }else{
            if(ControPlayer.Ins.GetlifePlayer()){
                scanAttack();      
                if(canAttack){
                    attack();
                }else{
                    AiGoAttack();
                }
            }
        }
    }
    void AiGoAttack(){
        float GoAttackX = positionPlayer.x-rb.position.x;
        float maxposition = Mathf.Max(Mathf.Abs(GoAttackX),rb.velocity.y);
        rb.velocity = new Vector2((GoAttackX/maxposition)*speedGoAttack,rb.velocity.y);
        left_right=(GoAttackX/maxposition);
        flip();
    }
    void AimoveLeft_right(){
        if(!groundL.State()){
            ChangeStatusLR = !ChangeStatusLR;
            nextChangeStatusTime = Time.time;
        }
        if(Time.time >= nextChangeStatusTime){
            if(ChangeStatus){
                if(ChangeStatusLR){
                    left_right = -1;
                    ChangeStatusLR=false;
                }else{
                    left_right = 1;
                    ChangeStatusLR=true;
                }
                ChangeStatus = false;
                flip();
                nextChangeStatusTime = Time.time + moveTime;
            }else{
                left_right = 0;
                ChangeStatus = true;
                nextChangeStatusTime = Time.time + restTime;
            }
        }
        rb.velocity = new Vector2(left_right*speed,rb.velocity.y);
    }
    void flip(){
        if(isfacingRight && left_right < 0 || !isfacingRight && left_right > 0 ){
            isfacingRight =!isfacingRight;
            Vector3 kich_thuoc =transform.localScale;
            kich_thuoc.x = kich_thuoc.x *-1;
            transform.localScale = kich_thuoc;
        }
    }
    void scanAttack(){
        if(!canAttack){
            Collider2D[] scanAttack = Physics2D.OverlapCircleAll(AttackRange.position,AttackRangeR,playerLayers);
            bool textcanAttack = true;
            foreach(Collider2D player in scanAttack){
                if(Mathf.Abs(player.GetComponent<Rigidbody2D>().velocity.y) <= 0.01f){
                    canAttack=true; 
                    textcanAttack = false;
                }
                attacked = false;
                if(textcanAttack){
                    canAttack=false;
                }
            }
        }  
    }
    void attack(){
        if(attacked){
            if(nextattackTime >= 0){
                nextattackTime -= Time.deltaTime;
            }else{
                attackPlayer();
            }
        }else{
            rb.velocity = new Vector2(0,0);
            nextattackTime = attackRate;
            attacked = true;
        }
    }
    void attackPlayer(){
        if(DelayAttack){
            animationAttack();
            DelayAttack = false;
        }
        if(nextDelayAttack >= DelayAttackTime){
            Collider2D[] scanAttack = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,playerLayers);
            foreach(Collider2D player in scanAttack){
                ControPlayer.Ins.SetTakeDamagePlayer(attackDamage,this.name);
            }
            canAttack = false;
            DelayAttack =true;
            nextDelayAttack = 0f;
        }
        nextDelayAttack += Time.deltaTime;  
    }
    void animationAttack(){
        if(attack1){
            anim.SetTrigger("attack1");
            attack1=false;
        }else{
            anim.SetTrigger("attack2");
            attack1=true;
        }
    }
    void OnDrawGizmosSelected(){
        if(attackPoint != null){
            Gizmos.DrawWireSphere(attackPoint.position,attackRange);
        }
        if(visionPoint != null){
            Gizmos.DrawWireSphere(visionPoint.position,visionRange);
        }
        if(AttackRange != null){
            Gizmos.DrawWireSphere(AttackRange.position,AttackRangeR);
        }
        return;
    }
    void scanPlayer(){
        Collider2D[] scanPlayer = Physics2D.OverlapCircleAll(visionPoint.position,visionRange,playerLayers);
        bool textseePlayer = true;
        foreach(Collider2D player in scanPlayer){
            seePlayer=true;
            textseePlayer=false;
            positionPlayer = ControPlayer.Ins.GetpositionPlayer();
        }
        if((positionPlayer.y >= this.rb.position.y+1)||textseePlayer){
            seePlayer=false;
        }
    }
    public void TakeDamage(int damage){
        //animation nhan dam
        anim.SetTrigger("hit");
        stunned=true;
        rb.velocity = new Vector2(0,0);
        nextstunedTime = Time.time + stunnedTime;
        //tinh dam
        currentHealth-=damage;
        if(currentHealth<=0){
            Die();
        }
    }
    void Die(){
        //animation Die
        anim.SetBool("die",true);
        life = false;
        //disable the enemy
        transform.Find("diemtua").GetComponent<enemylife>().life = life;
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
    }
}

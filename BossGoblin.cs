using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossGoblin : MonoBehaviour
{
    //Player
    public LayerMask playerLayers;
    public GameObject AttackDB;
    //Attack1
    public float attackRange;
    public  float attackRate;
    public int attackDamage;
    public float AttackRangeR = 0.4f;
    public Transform attackPoint;
    public Transform AttackRange;
    private float nextattackTime = 0f;
    private bool canAttack;
    private bool attacked = false;
    private bool TakeDamageed = false;
    //thiet lam animation tang cong1
    private float DelayAttackTime1 = 0.8f;
    private float nextDelayAttack = 0f;
    private bool DelayAttack = true;
    //thiet lam animation tang cong2
    private float DelayAttackTime2 = 0.8f;
    // 
    public GameObject AttackDBPoint;
    private ControllBoss ControllBoss;
    public float speedGoAttack;
    private bool isfacingRight=false;
    private Animator anim;
    private Rigidbody2D rb;
    private float left_right;
    private bool thunLR;
    void Awake(){
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }
    void Start()
    {   
        thunLR = true;
    }
    public void Attack1(Vector2 positionPlayer){  
        scanAttack();
        if(canAttack){
            attack();
        }else{
            AiGoAttack(positionPlayer); 
        }
    }
    public void Attack2(Vector3 left){  
        if(inhere(left)){
            skillattack();
        }else{
            AiGoAttack(left); 
            thunLR = true;
        }
    }
    public void Attack3(Vector3 right){  
        if(inhere(right)){
            skillattack();
        }else{
            AiGoAttack(right);
            thunLR = true;
        }
    }
    public void TakeDamage(int attackDamage){
        //spaw nhan dam
        
        //dieu dinh hp
        ControllBoss.TakeDamageBoss(attackDamage);
    }
    public void SetControlleBoss(ControllBoss ControllBoss){
        this.ControllBoss = ControllBoss;
    }
    public void SetColliderEnabled(bool taget){
        GetComponent<Collider2D>().enabled = taget;
    }
    bool inhere(Vector3 here){
        return Mathf.Abs(here.x - rb.position.x) <= 0.5f;
    }
    void skillattack(){
        //
        if(thunLR){
            thunleft_right();
            thunLR = false;
        }
        left_right = 0;
        anim.SetFloat("move",Mathf.Abs(left_right));
        rb.velocity = new Vector2(0,0);
        if(attacked){
            if(nextattackTime >= 0){
                nextattackTime -= Time.deltaTime;
            }else{
                attackPlayer2();
            }
        }else{
            nextattackTime = attackRate;
            attacked = true;
        }
    }
    void scanAttack(){
        if(!canAttack){
            Collider2D[] scanAttack = Physics2D.OverlapCircleAll(AttackRange.position,AttackRangeR,playerLayers);
            bool textcanAttack = true;
            foreach(Collider2D player in scanAttack){   
                canAttack=true;
                textcanAttack = false;
                attacked = false; 
            }
            if(textcanAttack){
                canAttack=false;
                DelayAttack=true;
                nextDelayAttack = 0f;
            }
        }  
    }
    void AiGoAttack(Vector2 positionPlayer){
        float GoAttackX = positionPlayer.x-rb.position.x;
        float maxposition = Mathf.Max(Mathf.Abs(GoAttackX),rb.velocity.y);
        rb.velocity = new Vector2((GoAttackX/maxposition)*speedGoAttack,rb.velocity.y);
        left_right=(GoAttackX/maxposition);
        anim.SetFloat("move",Mathf.Abs(left_right));
        flip();
    }
    void flip(){
        if(isfacingRight && left_right < 0 || !isfacingRight && left_right > 0 ){
            isfacingRight =!isfacingRight;
            Vector3 kich_thuoc =transform.localScale;
            kich_thuoc.x = kich_thuoc.x *-1;
            transform.localScale = kich_thuoc;
        }
    }
    void thunleft_right(){
        isfacingRight =!isfacingRight;
        Vector3 kich_thuoc =transform.localScale;
        kich_thuoc.x = kich_thuoc.x *-1;
        transform.localScale = kich_thuoc;
    }
    void attack(){
        left_right = 0;
        anim.SetFloat("move",Mathf.Abs(left_right));
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
            anim.SetTrigger("tcong");
            DelayAttack = false;
        }
        if(nextDelayAttack >= DelayAttackTime1 && !TakeDamageed){
            Collider2D[] scanAttack = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,playerLayers);
            foreach(Collider2D player in scanAttack){
                ControPlayer.Ins.SetTakeDamagePlayer(attackDamage,this.name);
                TakeDamageed = true;
            }
        }
        if(nextDelayAttack >= DelayAttackTime1+0.5f){
            canAttack = false;
            DelayAttack = true;
            TakeDamageed = false;
            nextDelayAttack = 0f;
        }
        if(!DelayAttack){
            nextDelayAttack += Time.deltaTime;  
        }
    }
    void attackPlayer2(){
        if(DelayAttack){
            anim.SetTrigger("tcongDB");
            nextDelayAttack = 0f;
            DelayAttack = false;
        }
        if(nextDelayAttack >= DelayAttackTime2){
            SwapAttackDB();
            DelayAttack = true;
        }
        if(!DelayAttack){
            nextDelayAttack += Time.deltaTime;  
        }
    }
    void OnDrawGizmosSelected(){
        if(attackPoint != null){
            Gizmos.DrawWireSphere(attackPoint.position,attackRange);
        }
        if(AttackRange != null){
            Gizmos.DrawWireSphere(AttackRange.position,AttackRangeR);
        }
        return;
    }
    void SwapAttackDB(){
        Vector3 spawnPosition = AttackDBPoint.transform.position;
        if (AttackDB != null){
            // Set correct arrow spawn position
            GameObject Attackdb = Instantiate(AttackDB, spawnPosition, gameObject.transform.localRotation) as GameObject;
            float LR;
            if(isfacingRight){
                LR = 1f;
            }else{
                LR= -1f;
            }
            Attackdb.GetComponent<AttackDB>().SetisfacingRight(LR);
            // Turn arrow in correct direction
        }
    }
    public void BossDie(){
        anim.SetBool("die",true);
        anim.SetTrigger("hit");
    }
    public void SetDelayAttack(bool DelayAttack){
        this.DelayAttack = DelayAttack;
    }
    public void DestroyBoss(){
        transform.GetComponent<DestroyEven>().destroyEvent();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetBoss2 : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 100;
    //mau hien tai
    int currentHealth;
    //toc do
    public float speed;
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
    public float Defere; 
    private Vector2 positionPlayer;
    private Vector2 velocityPlayer;
    private bool canAttack;
    //thong tin tang cong
    public  float attackRate = 0.5f;
    private float nextattackTime = 0f;
    //thiet lap bi choang
    public float stunnedTime = 3f;  
    private bool stunned = false;
    private float nextstunedTime = 0;
    //thiet lam animation tang cong
    private float DelayAttackTime = 0.3f;
    private float nextDelayAttack = 0f;
    private bool DelayAttack = true;
    private float DelayAttackPet = 5f;
    private float nextDelayAttackPet =0f;
    //
    private ControllBoss2 ControllBoss2;
    private Animator anim;
    private Rigidbody2D rb;
    private float left_right;
    private bool isfacingRight=false;
    private bool seePlayer;
    private bool life;
    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth=maxHealth;
        life = true;
        DelayAttack=false;
        canAttack=false;
    }
    // Update is called once per frame
    void Update()
    {
        if(life){
            StunnedDelay();
            if(!stunned){
                aIEnemyMove();
            }
        }else{
            if(GetComponent<goOnGround>().GoOnGround()){
                this.enabled = false;
            }
        }
    }
    void StunnedDelay(){
        if(Time.time>=nextstunedTime){
            stunned=false;
        }
    }
    void aIEnemyMove(){
        if(nextDelayAttackPet < DelayAttackPet){
            AimoveLeft_right();
            nextDelayAttackPet+=Time.deltaTime;
        }else{
            if(ControPlayer.Ins.GetlifePlayer()){
                if(DelayAttack){
                    nextDelayAttack += Time.deltaTime;
                }else{
                    if(canAttack){
                        SetAttack();
                        anim.SetTrigger("attack2");
                        canAttack=false;
                    }else{
                        nextattackTime+=Time.deltaTime;
                        rb.velocity = new Vector2(0,0);
                    }
                    if(nextattackTime>=attackRate){
                        canAttack=true;
                        DelayAttack=true;
                        nextDelayAttack=0f;
                        nextattackTime=0f;
                    }
                }
                if(nextDelayAttack < DelayAttackTime){
                    AiGoAttack();
                }else{
                    DelayAttack=false;
                }
            }
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player") && life){
            ControPlayer.Ins.SetTakeDamagePlayer(attackDamage,this.name);
        }
    }
    void SetAttack(){
        positionPlayer = ControPlayer.Ins.GetpositionPlayer();
        float GoAttackX = positionPlayer.x-rb.position.x;
        float GoAttackY = positionPlayer.y-rb.position.y;
        float maxposition = Mathf.Max(Mathf.Abs(GoAttackX),Mathf.Abs(GoAttackY));
        velocityPlayer = new Vector2((GoAttackX/maxposition)*speedGoAttack-Defere,(GoAttackY/maxposition)*speedGoAttack+Defere);
        left_right=(GoAttackX/maxposition);
        Debug.Log(positionPlayer);
    }
    void AiGoAttack(){
        anim.SetTrigger("attack1");//lao toi
        rb.velocity = velocityPlayer;
        flip();
    }
    void AimoveLeft_right(){
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
                nextChangeStatusTime = Time.time + moveTime;
            }else{
                left_right = 0;
                ChangeStatus = true;
                nextChangeStatusTime = Time.time + restTime;
            }
        }
        rb.velocity = new Vector2(left_right*speed,rb.velocity.y);
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
        ControllBoss2.SetPart2();
        life = false;
        //disable the enemy
        transform.Find("diemtua").GetComponent<enemylife>().life = life;
        GetComponent<Collider2D>().enabled = false;
    }
    public void SetControlleBoss(ControllBoss2 ControllBoss2){
        this.ControllBoss2=ControllBoss2;
    }
    public void Setdefere(float Defere){
        this.Defere=Defere;
    }
}
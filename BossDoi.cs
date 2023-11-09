using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDoi : MonoBehaviour
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
    public Transform[] vitriPet;
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
    private bool CheckppositionLaoToi;
    // 
    public GameObject AttackDBPoint;
    private ControllBoss2 ControllBoss2;
    public float speedGoAttack;
    private bool isfacingRight=false;
    private Animator anim;
    private Rigidbody2D rb;
    private float left_right;
    private bool thunLR;
    //AImoverest
    public float speed = 5f;
    public float moveTime = 4f;
    public float restTime = 4f;
    public GameObject Pet;
    private enemylife enemylifePet1;
    private enemylife enemylifePet2;
    private bool ChangeStatus = true;
    private bool ChangeStatusLR = true;
    private float nextChangeStatusTime = 0f;
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
    public void Attack2(Vector3 left_top , Vector3 right_bot){ 
        if(inhere(left_top)){
            CheckppositionLaoToi=true;
            canAttack=true;
        }else{
            AiGoAttack(left_top);
            thunLR = true;
        }
        if(CheckppositionLaoToi){
            skillattack(right_bot);
            TakeDamageLaoToi();
            if(inhere(right_bot)){
                CheckppositionLaoToi=false;
                anim.SetBool("lao",false);
            }
        }
    }
    public void Attack3(Vector3 right_top , Vector3 left_bot){  
        if(inhere(right_top)){
            CheckppositionLaoToi=true;
            canAttack=true;
        }else{
            AiGoAttack(right_top);
            thunLR = true;
        }
        if(CheckppositionLaoToi){
            skillattack(left_bot);
            TakeDamageLaoToi();
            if(inhere(left_bot)){
                CheckppositionLaoToi=false;
                anim.SetBool("lao",false);
            }
        }
    }
    public void Attack4(Vector3 mid){  
        if(inhere(mid)){
            rb.velocity = new Vector2(0,0);
            attackPlayer3();
        }else{
            AiGoSkill(mid);
        }
    }
    void TakeDamageLaoToi(){
        if(canAttack){
            Collider2D[] scanAttack = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,playerLayers);
            foreach(Collider2D player in scanAttack){
                ControPlayer.Ins.SetTakeDamagePlayer(attackDamage,this.name);
                canAttack=false;
            }
        }
    }
    public void TakeDamage(int attackDamage){
        //spaw nhan dam
        //dieu dinh hp
        ControllBoss2.TakeDamageBoss(attackDamage);
    }
    public void SetControlleBoss(ControllBoss2 ControllBoss2){
        this.ControllBoss2 = ControllBoss2;
    }
    public void SetColliderEnabled(bool taget){
        GetComponent<Collider2D>().enabled = taget;
    }
    bool inhere(Vector3 here){
        return (Mathf.Abs(here.x - rb.position.x) <= 1f) && (Mathf.Abs(here.y - rb.position.y) <= 1f);
    }
    public void AimoveLeft_right(){
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
    public void Spaw2Pet(){
        GameObject Pet1 = Instantiate(Pet,vitriPet[0].position, gameObject.transform.localRotation) as GameObject;
        Pet1.transform.Find("diemtua").GetComponent<enemylife>().SetgameObject(Pet1);
        enemylifePet1 = Pet1.transform.Find("diemtua").GetComponent<enemylife>();
        Pet1.GetComponent<PetBoss2>().SetControlleBoss(ControllBoss2);
        Pet1.GetComponent<PetBoss2>().Setdefere(.02f);
        GameObject Pet2 = Instantiate(Pet,vitriPet[1].position, gameObject.transform.localRotation) as GameObject;
        Pet2.transform.Find("diemtua").GetComponent<enemylife>().SetgameObject(Pet2);
        enemylifePet2 = Pet2.transform.Find("diemtua").GetComponent<enemylife>();
        Pet2.GetComponent<PetBoss2>().SetControlleBoss(ControllBoss2);
        Pet1.GetComponent<PetBoss2>().Setdefere(.01f);
    }
    void skillattack(Vector3 positionAttackDB){
        //
        if(thunLR){
            thunleft_right();
            thunLR = false;
        }
        left_right = 0;
        rb.velocity = new Vector2(0,0);
        if(attacked){
            if(nextattackTime >= 0){
                nextattackTime -= Time.deltaTime;
            }else{
                attackPlayer2(positionAttackDB);
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
        float GoAttackY = positionPlayer.y-rb.position.y;
        float maxposition = Mathf.Max(Mathf.Abs(GoAttackX),Mathf.Abs(GoAttackY));
        rb.velocity = new Vector2((GoAttackX/maxposition)*speedGoAttack,(GoAttackY/maxposition)*speedGoAttack);
        left_right=(GoAttackX/maxposition);
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
    void attackPlayer2(Vector3 positionAttackDB){
        if(DelayAttack){
            anim.SetTrigger("laotoi");
            anim.SetBool("lao",true);
            nextDelayAttack = 0f;
            DelayAttack = false;
        }
        if(nextDelayAttack >= DelayAttackTime2){
           BossLaotoi(positionAttackDB);
        }
        if(!DelayAttack){
            nextDelayAttack += Time.deltaTime;
        }
    }
    void attackPlayer3(){
        if(DelayAttack){
            anim.SetTrigger("tcongDB");
            nextDelayAttack = 0f;
            DelayAttack = false;
        }
        if(nextDelayAttack >= DelayAttackTime2){
            BossAttackDB();
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
    void BossLaotoi(Vector3 positionAttackDB){
        float GoAttackX = positionAttackDB.x-rb.position.x;
        float GoAttackY = positionAttackDB.y-rb.position.y;
        float maxposition = Mathf.Max(Mathf.Abs(GoAttackX),Mathf.Abs(GoAttackY));
        rb.velocity = new Vector2((GoAttackX/maxposition)*(speedGoAttack*4),(GoAttackY/maxposition)*(speedGoAttack*4));
        left_right=(GoAttackX/maxposition);
        flip();
    }
    // tạo hàm check trươc rồi mới gọi hàm này
    public void AiGoSkill(Vector3 positionSkill){
        float GoAttackX = positionSkill.x-rb.position.x;
        float GoAttackY = positionSkill.y-rb.position.y;
        float maxposition = Mathf.Max(Mathf.Abs(GoAttackX),Mathf.Abs(GoAttackY));
        rb.velocity = new Vector2((GoAttackX/maxposition)*(speedGoAttack*2),(GoAttackY/maxposition)*(speedGoAttack*2));
        left_right=(GoAttackX/maxposition);
        flip();
    }
    void BossAttackDB(){
        Vector3 spawnPosition = new Vector3(Random.Range(-22,2),34,-1);
        if (AttackDB != null){
            // Set correct arrow spawn position
            GameObject Attackdb = Instantiate(AttackDB, spawnPosition, gameObject.transform.localRotation) as GameObject;
            Attackdb.GetComponent<MuiTenBoss2>().SetisfacingRight();
            // Turn arrow in correct direction
        }
    }
    public void BossDie(){
        //Viet lai
        anim.SetBool("die",true);
        anim.SetTrigger("hit");
    }
    public void SetDelayAttack(bool DelayAttack){
        this.DelayAttack = DelayAttack;
        CheckppositionLaoToi=false;
    }
    void Destroy2Pet(){
        enemylifePet1.life = false;
        enemylifePet1.Destroyenemy();
        enemylifePet2.life = false;
        enemylifePet2.Destroyenemy();
    }
    public void DestroyBoss(){
        Destroy2Pet();
        transform.GetComponent<DestroyEven>().destroyEvent();
    }
}

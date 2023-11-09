    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{ 
    public int maxHealth = 100;
    public bool life ;
    public float speed;
    public float jumpF;
    public float attackRange = 0.5f;
    public  float attackRate = 0.5f;
    public Transform attackPoint;
    public LayerMask[] LayerOJ;
    public  int attackDamage;
    public float rollF = 1f;
    public GameObject slideDust;
    public GameObject AnimTakeDama;
    private ControPlayer ControPlayer;
    private int currentHealth;
    private float nextattackTime = 0f;
    private bool isfacingRight=true;
    private bool grounded = true;
    private float left_right;
    private float lifeTime = 2f;
    private float nextlifeTime = 2f;
    private bool PlayGame;
    private Rigidbody2D rb;
    private Animator anim;
    private int currentAttack = 0;
    private float timeSinceAttack = 0f;
    private bool WallSliding = false;
    private bool AttackorBlock = true;
    private bool rolling = false;
    private bool delayroll = false;
    public float timeroll = 1f;
    public float delayrolltime = 0.5f;
    private float nextrolltime = 0f;
    private float nextdelayrolltime = 0f;
    private float nextslidetime = 0f;
    private float slidetime = 0.5f;
    private bool slided = false;
    //
    private bool AudioRuned = false;
    // cai dat truoc
    private caidatdiemtua   ongroundSensor;
    private caidatdiemtua   wallRU;
    private caidatdiemtua   wallRD;
    private GameObject TakeDamagePont;
    private DestroyEven DestroySlideDust;

    [SerializeField]
    public ScenesInfo scenesInfo;
    // Audio
    AudioManage audioManage;
    // Start is called before the first frame update
    void Awake(){
        audioManage = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManage>();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = transform.Find("Model").GetComponent<Animator>();
        ongroundSensor = transform.Find("OnGround").GetComponent<caidatdiemtua>();
        wallRU = transform.Find("Model").Find("WallRU").GetComponent<caidatdiemtua>();
        wallRD = transform.Find("Model").Find("WallRD").GetComponent<caidatdiemtua>();
        TakeDamagePont = transform.Find("Model").Find("TakeDamagePont").gameObject;
        DestroySlideDust = slideDust.GetComponent<DestroyEven>();
        currentHealth = maxHealth;
        life = true;
        PlayGame = true;
        anim.SetBool("Grounded", grounded);
        PlayerTeLePoints(scenesInfo.PositionPlayer);
        SetcurrentHealth(scenesInfo.HpPlayer);
        if(ControPlayer){
            ControPlayer.SetProgress(scenesInfo.GetBoss());
            ControPlayer.SetHpplayer(GetHPplayer());
            if(scenesInfo.GetBoss() == 3){
                ControPlayer.SetAces(true);
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!life && PlayGame){
            nextlifeTime += Time.deltaTime;
        }
        if(nextlifeTime >= lifeTime){
            life = true;
        }
        if(life){
            timeSinceAttack += Time.deltaTime;
            if(rolling){
                nextrolltime += Time.deltaTime; 
            }
            if(!rolling && delayroll){
                nextdelayrolltime +=Time.deltaTime;
            }
            if(slided){
                nextslidetime += Time.deltaTime;
            }
            //cai dat diem tua duoi chan
            if(!grounded && ongroundSensor.State()){
                grounded = true;
                anim.SetBool("Grounded", grounded);
            }
            if (grounded && !ongroundSensor.State()){
                grounded = false;
                anim.SetBool("Grounded", grounded);
                
            }
            if(nextrolltime >= timeroll){
                rolling = false;
                nextdelayrolltime = 0f;
                delayroll = true;
                nextrolltime = 0f;
                GetComponent<Collider2D>().enabled = true;
                transform.Find("rollColiide2d").GetComponent<Collider2D>().enabled = false;
            }
            if(nextdelayrolltime >= delayrolltime){
                delayroll = false;
            }
            if(nextslidetime >= slidetime){
                slided = false;
            }
            //cai dat huong nhin
            left_right = Input.GetAxis("Horizontal");
            flip();
            anim.SetFloat("Yvelocity",rb.velocity.y);
            //tangcong/phong thu
            if(!rolling){
                block();
                if(Mathf.Abs(rb.velocity.x) >= 0.1f && Mathf.Abs(rb.velocity.y) <= 0.1f){
                    if(!AudioRuned){
                        AudioRuned = true;
                        audioManage.PlaySFXLongTime(audioManage.audioClips[1],AudioRuned);
                    }
                }else{
                    if(AudioRuned){
                        AudioRuned = false;
                        audioManage.PlaySFXLongTime(audioManage.audioClips[1],AudioRuned);
                        Debug.Log("tatnhac");
                    }
                }
                if(AttackorBlock){
                    attack();
                    //nhay
                    moveLeft_right();
                    anim.SetFloat("move",Mathf.Abs(left_right));
                    
                    playerjump();
                    anim.SetBool("jumd",!grounded);
                }  
                //truoc tuong
                Slide();
                //lon nhao
                if(!delayroll){
                    roll();
                }
            }
        }
    }
    void Slide(){
        WallSliding = (wallRU.State() && wallRD.State());
        if(WallSliding && !slided && rb.velocity.y<-0.1){
            SlideDust();
            audioManage.PlayeSFX(audioManage.audioClips[15]);
            nextslidetime = 0f;
            slided = true;
        }
        anim.SetBool("WallSlide", WallSliding);
        
    }
    void roll(){
        if(Input.GetKeyDown("left shift") && !WallSliding){
            rolling = true;
            anim.SetTrigger("roll");
            audioManage.PlayeSFX(audioManage.audioClips[6]);
            float facingRight;
            if(isfacingRight){
                facingRight=1;
            }else{
                facingRight=-1;
            }
            GetComponent<Collider2D>().enabled = false;
            transform.Find("rollColiide2d").GetComponent<Collider2D>().enabled = true;
            rb.velocity = new Vector2(rollF*facingRight,rb.velocity.y);
        }
    }
    void block(){
        if(Input.GetMouseButtonDown(1)&&grounded){
            anim.SetTrigger("Blockidie");
            anim.SetBool("idieBlock", true);
            AttackorBlock = false;
            anim.SetFloat("move",0);
            left_right = 0;
            rb.velocity =new Vector2(0,0);
        }
        else if(Input.GetMouseButtonUp(1)){
            anim.SetBool("idieBlock", false);
            AttackorBlock = true;
        }
    }
    void playerjump(){
        if((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))&&grounded){
            rb.AddForce(Vector2.up*jumpF, ForceMode2D.Impulse);
            grounded=false;
            anim.SetBool("Grounded",grounded);
            audioManage.PlayeSFX(audioManage.audioClips[5]);
            ongroundSensor.Disable(0.2f);
        }
    }
    void moveLeft_right(){
        rb.velocity = new Vector2(left_right*speed,rb.velocity.y);
    }
    void flip(){
        if(isfacingRight && left_right < 0 || !isfacingRight && left_right > 0 ){
            isfacingRight =!isfacingRight;
            Vector3 kich_thuoc = transform.Find("Model").transform.localScale;
            kich_thuoc.x = kich_thuoc.x *-1;
            transform.Find("Model").transform.localScale = kich_thuoc;
        }
    } 
    void attack(){
        if(Input.GetMouseButtonDown(0)){
            if(Time.time >= nextattackTime){
                AnimationAttack();
                if(currentAttack==3){
                    nextattackTime =Time.time + attackRate*3;
                }else{
                    nextattackTime =Time.time + attackRate;
                }
                TagetAttackLayers();
            }   
        }
    }
    private void TagetAttackLayers(){
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,LayerOJ[0]);
        foreach(Collider2D enemy in hitEnemies){  
            enemy.GetComponent<enemy>().TakeDamage(attackDamage);
        }
        Collider2D[] hitEnemies2 = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,LayerOJ[1]);
        foreach(Collider2D enemy in hitEnemies2){  
            enemy.GetComponent<enemy2>().TakeDamage(attackDamage);   
        }
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,LayerOJ[3]);
        foreach(Collider2D Boss in hitBoss){  
            if(Boss.GetComponent<BossGoblin>()) Boss.GetComponent<BossGoblin>().TakeDamage(attackDamage);
            if(Boss.GetComponent<BossDoi>()) Boss.GetComponent<BossDoi>().TakeDamage(attackDamage);
            if(Boss.GetComponent<BossNam>()) Boss.GetComponent<BossNam>().TakeDamage(attackDamage);
            SwapTakeDamage();  
        }
        Collider2D[] hitSaveRespawnPoints = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,LayerOJ[4]);
        foreach(Collider2D RespawnPoints in hitSaveRespawnPoints){  
            SaveRespawnPoints(RespawnPoints.gameObject);
        }
        Collider2D[] hitBlock = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,LayerOJ[5]);
        foreach(Collider2D Block in hitBlock){  
            Block.GetComponent<AnimationDavo>().BlockDertroy();
        }
        Collider2D[] hitPet = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,LayerOJ[6]);
        foreach(Collider2D Pet in hitPet){  
            Pet.GetComponent<PetBoss2>().TakeDamage(attackDamage);
        }
    }
    void AnimationAttack(){
        currentAttack++;
        // doi duong kiem
        if (currentAttack > 3){
            currentAttack = 1;
        }
        // thiet lap lai duong kiem neu cho qua lau
        if (timeSinceAttack > 1.0f){
            currentAttack = 1;
        }
        //  "attack1", "attack2", "attack3"
        anim.SetTrigger("attack" + currentAttack);
        Collider2D[] watchman = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,LayerOJ[7]);
        foreach(Collider2D man in watchman){
            man.GetComponent<Watchman>().FendOff();
            audioManage.PlayeSFX(audioManage.audioClips[17]);
        }
        audioManage.PlayeSFX(audioManage.audioClips[currentAttack+1]);
        timeSinceAttack = 0f;
    }
    void Die(){
        //animation Die
        life = false;
        PlayGame = false;
        nextlifeTime = 0f;
        anim.SetBool("die",!life);
        audioManage.PlayeSFX(audioManage.audioClips[11]);
        ControPlayer.SetGameOver();
        //disable the enemy
    }
    void OnDrawGizmosSelected(){
        if(attackPoint == null){
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
    public void Setvelocity(float x,float y){
        rb.velocity = new Vector2(x,y);
    }
    public void DieNoBlood(){
        life = false;
        PlayGame = false;
        nextlifeTime = 0f;
        anim.SetBool("die",!life);
        anim.SetTrigger("dienoblood");
        audioManage.PlayeSFX(audioManage.audioClips[11]);
        ControPlayer.SetGameOver();
    }
    public void TakeDamage(int damage,string AttackName){
        bool canBlock = false;
        Collider2D[] Enemieshit = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,LayerOJ[0]);
        foreach(Collider2D enemy in Enemieshit){
            if(enemy.name==AttackName){
                canBlock = true;
            }
        }
        Collider2D[] Enemieshit2 = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,LayerOJ[1]);
        foreach(Collider2D enemy in Enemieshit2){
            if(enemy.name==AttackName){
                canBlock = true;
            }
        }
        Collider2D[] Enemieshit3 = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,LayerOJ[2]);
        foreach(Collider2D enemy in Enemieshit3){
            if(enemy.name==AttackName){
                canBlock = true;
            }
        }
        Collider2D[] Enemieshit4 = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,LayerOJ[6]);
        foreach(Collider2D enemy in Enemieshit3){
            if(enemy.name==AttackName){
                canBlock = true;
            }
        }
        if(AttackorBlock||!canBlock){
            anim.SetTrigger("hit");
            currentHealth-=damage;
            ControPlayer.SetHpplayer(GetHPplayer());
            if(currentHealth<=0){
                Die();
            }
        }else{
            anim.SetTrigger("Block");
            audioManage.PlayeSFX(audioManage.audioClips[14]);
        }
    }
    public Vector2 Getposition(){
        return rb.position;
    }
    public float GetHPplayer(){
        return (float)currentHealth/maxHealth;
    }
    public void SetControPlayer(GameObject controPlayer){
        ControPlayer = controPlayer.GetComponent<ControPlayer>();
    }
    public void SaveRespawnPoints(GameObject RSPointOJ){
        ControPlayer.SetRespawnPoint(RSPointOJ,transform.position);
    }
    public void RespawnPoints(){
        StartOver();
    }
    public void PlayerTeLePoints(Vector2 TeLePoints){
        rb.position = TeLePoints;
    }
    public void SetcurrentHealth(int HpPlayer){
        currentHealth = HpPlayer;
    }
    public int GetcurrentHealth(){
        return currentHealth;
    }
    public void SetscenesInfoBoss(int bossdie){
        scenesInfo.SetBossDie(bossdie);
        ControPlayer.SetProgress(scenesInfo.GetBoss());
        if(scenesInfo.GetBoss() == 3){
            ControPlayer.SetAces(true);
        }
    }
    void StartOver(){
        currentHealth = maxHealth;
        ControPlayer.SetHpplayer(currentHealth/maxHealth);
        anim.SetBool("die",false);
        PlayGame = true;
        anim.SetTrigger("Revival");
    }
    void SlideDust(){
        float slideDustflip;
        if(isfacingRight){
            slideDustflip = 1;
        }else{
            slideDustflip = -1;
        }
        Vector3 spawnPosition = wallRU.transform.position;
        if (slideDust != null){
            // Set correct arrow spawn position
            GameObject dust = Instantiate(slideDust, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            dust.transform.localScale = new Vector3(slideDustflip, 1, 1);
            Destroy(dust,1);
        }
    }
    void SwapTakeDamage(){
        Vector3 spawnPosition = TakeDamagePont.transform.position;
        if (AnimTakeDama != null){
            // Set correct arrow spawn position
            GameObject TakeDame = Instantiate(AnimTakeDama, spawnPosition, gameObject.transform.localRotation) as GameObject;
            // Turn arrow in correct direction
            Destroy(TakeDame,0.5f);
        }
    }
    
}

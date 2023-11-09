using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllBoss3 : MonoBehaviour
{
    // Start is called before the first frame update
    public int maxHealth = 1000;
    public GameObject EvenBoss;
    //mau hien tai
    private int currentHealth;
    //
    private GameObject Boss;
    private float Attack1time;
    private bool LetPlay;
    private float TimePlay;
    private float TimeDie = 6f;
    //trang thai
    public float Staytime;
    public float Atack1time;
    public float Atack21time;
    public GameObject[] taget;
    //
    private bool Bosslife;
    private bool CanTaget;
    private float Model1;
    private float Model2;
    //
    private BossNam BossNam;
    UIManager m_ui;
    void Start()
    {
        m_ui = FindObjectOfType<UIManager>();
        currentHealth = maxHealth;
        Model1 = Staytime + Atack1time;
        Model2 = Staytime + Atack1time + Atack21time;
    }

    // Update is called once per frame
    void Update()
    {
        if(Bosslife){
            Part1Boss();
        }else{
            DelayBossDie();
        }
    }
    private void DelayBossDie(){
        TimeDie -= Time.deltaTime;
        if(TimeDie <= 0f){
            BossDie();
        }
    }
    private void Part1Boss(){
        if(LetPlay){
            TimePlay += Time.deltaTime;
        }
        //3 giay cho
        if(TimePlay <= Staytime){
            
        }
        //10 giay danh thuong
        if(TimePlay > Staytime && TimePlay <= Staytime+0.5f){
            BossNam.SetDelayAttack(true);
        }
        if(TimePlay > Staytime+0.5f && TimePlay <= Model1){
            BossNam.Attack1(ControPlayer.Ins.GetpositionPlayer());
        }
        if(TimePlay > Model1 && TimePlay <= Model1+1f){
            BossNam.SetDelayAttack(true);
        }
        //5 giay danh xa trai
        if(TimePlay > Model1+1f && TimePlay <= Model2){
            BossNam.Attack2(taget);
        }
        if(TimePlay > Model2 && TimePlay <= Model2+1f){
            BossNam.SetDelayAttack(true);
        }
        if(TimePlay > Model2+1f){
            TimePlay = 0f;
        }
    }
    public void SetBoss(GameObject Boss){
        this.Boss = Boss;
        BossNam = this.Boss.transform.GetComponent<BossNam>();
        SetStartBoss();
        currentHealth = maxHealth;
    }
    public void SetStartBoss(){
        LetPlay = true;
        TimePlay = 0f;
        CanTaget = true;
        Bosslife = true;
    }
    public void TakeDamageBoss(int damage){
        //tru mau
        currentHealth-=damage;
        m_ui.SetHpSliderBoss((float)currentHealth/maxHealth);
        //dieu chinh thanh mau boss tren UI
        //neu het mau thi chet
        if(currentHealth<=0){
            BossNam.BossDie();
            EvenBoss.GetComponent<EvenBoss3>().SetDefaulPlayerCam();
            Bosslife = false;
        }
    }
    void BossDie(){
        LetPlay = false;    
        TimePlay = 0f;
        CanTaget = false;
        BossNam.SetColliderEnabled(CanTaget);
        EvenBoss.GetComponent<EvenBoss3>().CloseBoss();
        this.enabled = false;
    }
    public void DestroyBoss(){
       BossNam.DestroyBoss();
    }
    public void SetEndlEven(){
        ControPlayer.Ins.SetscenesInfoBoss(2);
    }
}

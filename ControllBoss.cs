using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllBoss : MonoBehaviour
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
    private float TimeDie = 3f;
    //trang thai
    public float Staytime;
    public float Atack1time;
    public float Atack21time;
    public float Atack22time;
    public Vector3[] taget;
    //
    private bool Bosslife;
    private bool CanTaget;
    private float Model1;
    private float Model2;
    private float Model3;
    //
    private BossGoblin Goblin;
    UIManager m_ui;
    void Start()
    {
        m_ui = FindObjectOfType<UIManager>();
        currentHealth = maxHealth;
        Model1 = Staytime + Atack1time;
        Model2 = Staytime + Atack1time + Atack21time;
        Model3 = Staytime + Atack1time + Atack21time + Atack22time;
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
            if(!CanTaget){
                CanTaget=true;
                Debug.Log("co the tang cong");
                Goblin.SetColliderEnabled(CanTaget);
            }
        }
        //10 giay danh thuong
        if(TimePlay > Staytime && TimePlay <= Staytime+0.5f){
            Goblin.SetDelayAttack(true);
        }
        if(TimePlay > Staytime+0.5f && TimePlay <= Model1){
            Goblin.Attack1(ControPlayer.Ins.GetpositionPlayer());
        }
        if(TimePlay > Model1 && TimePlay <= Model1+1f){
            Goblin.SetDelayAttack(true);
        }
        //5 giay danh xa trai
        if(TimePlay > Model1+1f && TimePlay <= Model2){
            if(CanTaget){
                CanTaget=false;
                Debug.Log("khong the tang cong");
                Goblin.SetColliderEnabled(CanTaget);
            }
            Goblin.Attack2(taget[0]);
        }
        if(TimePlay > Model2 && TimePlay <= Model2+1f){
            Goblin.SetDelayAttack(true);
        }
        //5 giay danh xa phai
        if(TimePlay > Model2+1f && TimePlay <= Model3){
            Goblin.Attack3(taget[1]);
        }
        //lap lai
        if(TimePlay > Model3){
            TimePlay = 0f;
        }
    }
    public void SetBoss(GameObject Boss){
        this.Boss = Boss;
        Goblin = this.Boss.transform.GetComponent<BossGoblin>();
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
            Goblin.BossDie();
            EvenBoss.GetComponent<EvenBoss>().SetDefaulPlayerCam();
            Bosslife = false;
        }
    }
    void BossDie(){
        LetPlay = false;
        TimePlay = 0f;
        CanTaget = false;
        Goblin.SetColliderEnabled(CanTaget);
        EvenBoss.GetComponent<EvenBoss>().CloseBoss();
        ControPlayer.Ins.SetscenesInfoBoss(0);
        this.enabled = false;
    }
    public void DestroyBoss(){
       Goblin.DestroyBoss();
    }
}

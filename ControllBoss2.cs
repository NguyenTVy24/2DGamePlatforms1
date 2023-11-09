using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllBoss2 : MonoBehaviour
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
    private float Pet=2;
    private float Part;
    private bool Bosslife;
    private bool CanTaget;
    private float Model1;
    private float Model2;
    private float Model3;
    //
    private BossDoi Doi;
    UIManager m_ui;
    void Start()
    {
        Part=1;
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
            if(Part==1){
                Part1Boss();
            } 
            if(Part==2){
                Part2Boss();
            }
            if(Part==3){
                Part3Boss();
            }   
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
        //Boss di chuyen qua lai
        Doi.AimoveLeft_right();
    }
    private void Part2Boss(){
        if(LetPlay){
            TimePlay += Time.deltaTime;
        }
        //3 giay cho
        if(TimePlay <= Staytime){
            Doi.AimoveLeft_right();
            if(!CanTaget){
                CanTaget=true;
                Doi.SetColliderEnabled(CanTaget);
            }
        }
        //10 giay danh thuong
        if(TimePlay > Staytime && TimePlay <= Staytime+0.5f){
            Doi.SetDelayAttack(true);
        }
        if(TimePlay > Staytime+0.5f && TimePlay <= Model1){
            Doi.Attack1(ControPlayer.Ins.GetpositionPlayer());
        }
        if(TimePlay > Model1 && TimePlay <= Model1+1f){
            Doi.SetDelayAttack(true);
        }
        //5 giay danh xa trai
        if(TimePlay > Model1+1f && TimePlay <= Model2){
            if(CanTaget){
                CanTaget=false;
                Doi.SetColliderEnabled(CanTaget);
            }
            Doi.Attack2(taget[0],taget[2]);
        }
        if(TimePlay > Model2 && TimePlay <= Model2+1f){
            Doi.SetDelayAttack(true);
        }
        //5 giay danh xa phai
        if(TimePlay > Model2+1f && TimePlay <= Model3){
            Doi.Attack3(taget[1],taget[3]);
        }
        //lap lai
        if(TimePlay > Model3){
            TimePlay = 0f;
        }
    }
    private void Part3Boss(){
        if(LetPlay){
            TimePlay += Time.deltaTime;
        }
        //3 giay cho
        if(TimePlay <= Staytime){
            //Di chuyen den chinh giua map
            if(!CanTaget){
                CanTaget=true;
                Doi.SetColliderEnabled(CanTaget);
            }
        }
        //10 giay danh thuong
        if(TimePlay > Staytime && TimePlay <= Staytime+0.5f){
            Doi.SetDelayAttack(true);
        }
        if(TimePlay > Staytime+0.5f && TimePlay <= Model1){
            // Phong Lao
            Doi.Attack4(taget[4]);
        }
        if(TimePlay > Model1 && TimePlay <= Model1+1f){
            Doi.SetDelayAttack(true);
        }
        //5 giay danh xa trai
        if(TimePlay > Model1+1f && TimePlay <= Model2){
            if(CanTaget){
                CanTaget=false;
                Doi.SetColliderEnabled(CanTaget);
            }
            Doi.Attack2(taget[0],taget[2]);
        }
        if(TimePlay > Model2 && TimePlay <= Model2+1f){
            Doi.SetDelayAttack(true);
        }
        //5 giay danh xa phai
        if(TimePlay > Model2+1f && TimePlay <= Model3){
            Doi.Attack3(taget[1],taget[3]);
        }
        //lap lai
        if(TimePlay > Model3){
            TimePlay = 0f;
        }
    }
    public void SetBoss(GameObject Boss){
        this.Boss = Boss;
        Doi = this.Boss.transform.GetComponent<BossDoi>();
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
        if(currentHealth<=maxHealth/2){
            Part=3;
        }
        //neu het mau thi chet
        if(currentHealth<=0){
            Doi.BossDie();
            EvenBoss.GetComponent<EvenBoss2>().SetDefaulPlayerCam();
            Bosslife = false;
        }
    }
    void BossDie(){
        LetPlay = false;
        TimePlay = 0f;
        CanTaget = false;
        Doi.SetColliderEnabled(CanTaget);
        EvenBoss.GetComponent<EvenBoss2>().CloseBoss();
        ControPlayer.Ins.SetscenesInfoBoss(1);   
        this.enabled = false;
    }
    public void DestroyBoss(){
        Doi.DestroyBoss();
    }
    public void SetPart2(){
        Pet--;
        if(Pet==0){
            Part=2;
        }
    }
    public void SetPart1(){
        Part=1;
        Pet=2;
    }
}

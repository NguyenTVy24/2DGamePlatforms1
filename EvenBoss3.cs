using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenBoss3 : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Door1;
    public GameObject Door2;
    
    public GameObject DeadZon1;
    public GameObject DeadZon2;

    public GameObject StoneON;
    public GameObject Water;
    WaterGoon water;
    public bool Watered;

    private Rigidbody2D RBDeadZon1;
    private Rigidbody2D RBDeadZon2;

    AnimationDoor animDoor1;
    AnimationDoor animDoor2;
    public GameObject CameraPlayer;
    public GameObject Backgroundmap;
    public GameObject Boss;
    public Vector3[] Target;
    public float SizeZoom;
    float Speedzoom = 0.0125f;
    float SpeedDownCam = 0.03f;
    private Camera cam;
    private ControllBoss3 CTBoss;
    private bool ZoomCam;
    private float ZoomSize;
    //
    private bool DownCam;
    private float TimeDownCam = 50f;
    private float NextTimeDownCam = 0f;
    //
    private bool spawedBoss;
    UIManager m_ui;
    AudioManage audioManage;
    void Awake(){
        audioManage = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManage>();
    }
    void Start(){
        m_ui = FindObjectOfType<UIManager>();
        animDoor1 = Door1.transform.GetComponent<AnimationDoor>();
        animDoor2 = Door2.transform.GetComponent<AnimationDoor>();
        RBDeadZon1 = DeadZon1.GetComponent<Rigidbody2D>();
        RBDeadZon2 = DeadZon2.GetComponent<Rigidbody2D>();
        water = Water.GetComponent<WaterGoon>();
        cam = CameraPlayer.transform.GetComponent<Camera>();
        CTBoss = transform.Find("ControBoss").GetComponent<ControllBoss3>();
        ZoomCam = false;
        DownCam = false;
        Watered = false;
        ZoomSize = cam.orthographicSize + SizeZoom;   
        spawedBoss = false;  
        CTBoss.enabled = false;
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Player")){
            //dong cua
            CloseTheDoor();
            //dieu chinh camera
            //ZoomCam = true;sao khi xuong cuoi cung
            DownCam = true;
            Setpositioncam(false);
            OffBackground(false);
            //tha cho
            //hien thanh mau
            //tat colide
            GetComponent<Collider2D>().enabled = false;
            audioManage.SetMusic(audioManage.audioClips[9]);
        }
    }
    void StartEvenBoss3(){
        SpawBoss();
        CTBoss.enabled = true;
        m_ui.ShowHPboss(true);
        m_ui.SetHpSliderBoss(1f);
    }
    public void ResetBoss(){
        //dong cua
        audioManage.SetMusic(audioManage.audioClips[0]);
        NextTimeDownCam = 0f;
        SetDeadZon(true);
        SetDeadZonDefault();
        OpenTheDoor();
        water.DefaulWater();
        GetComponent<Collider2D>().enabled = true;
        ZoomCam = false;
        DownCam = false;
        Watered = false;
        Setpositioncam(true);
        OffBackground(true);
        if(CTBoss.enabled){
            spawedBoss = false;
            m_ui.ShowHPboss(false);
            DestroyBoss();
            //dieu chinh camera
            //huy boss
            CTBoss.enabled = false;
        }
    }
    
    public void CloseBoss(){
        audioManage.SetMusic(audioManage.audioClips[13]);
        Watered = true;
        StoneON.SetActive(true);
        OpenTheDoor();
        Setpositioncam(true);
        OffBackground(true);
    }
    public void SetDefaulPlayerCam(){
        ZoomCam = false;
        m_ui.ShowHPboss(false);
    }
    private void DestroyBoss(){
        CTBoss.DestroyBoss();
    }
    public void CloseTheDoor(){
        animDoor1.CloseTheDoor();
        animDoor2.CloseTheDoor();
    }
    public void OpenTheDoor(){
        animDoor1.OpenTheDoor();
        animDoor2.OpenTheDoor();
    }
    private void SetDeadZon(bool DeadZon){
        DeadZon1.GetComponent<Collider2D>().enabled = DeadZon;
        DeadZon2.GetComponent<Collider2D>().enabled = DeadZon;
    }
    private void SetDeadZonDefault(){
        RBDeadZon1.position = new Vector2(-80.6f,-9.1f);
        RBDeadZon2.position = new Vector2(-80.6f,3f);
    }
    private void DownDeadZon(){
        RBDeadZon1.position = new Vector2(RBDeadZon1.position.x,RBDeadZon1.position.y-SpeedDownCam);
        RBDeadZon2.position = new Vector2(RBDeadZon2.position.x,RBDeadZon2.position.y-SpeedDownCam);
    }
    private void Setpositioncam(bool ENA){
        CameraPlayer.transform.GetComponent<CameraFollow>().enabled = ENA;
    }
    private void OffBackground(bool setBG){
        Backgroundmap.SetActive(setBG);
    }
    private void SpawBoss(){
        if(!spawedBoss){
            GameObject boss = Instantiate(Boss,Target[1], gameObject.transform.localRotation) as GameObject;
            CTBoss.SetBoss(boss);
            boss.transform.GetComponent<BossNam>().SetControlleBoss(CTBoss);
            spawedBoss = true;
        }   
    }
    private void LateUpdate(){
        //zoom camera
        if(DownCam){
            NextTimeDownCam += Time.deltaTime;
            if(NextTimeDownCam >= TimeDownCam){
                DownCam = false;
                ZoomCam = true;
                SetDeadZon(false);
                StartEvenBoss3();
            }else{
                if(cam.transform.position.y >= -93f){
                    cam.transform.position = new Vector3(-81f,cam.transform.position.y-SpeedDownCam,cam.transform.position.z);
                    DownDeadZon();
                }
            }
        }
        if(ZoomCam){
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize,ZoomSize,Speedzoom);
            cam.transform.position = Vector3.Lerp(cam.transform.position,Target[0],Speedzoom);
        }else{
            if(!DownCam){
                cam.orthographicSize = Mathf.Lerp(cam.orthographicSize,ZoomSize-SizeZoom,Speedzoom);
                cam.transform.position = Vector3.Lerp(cam.transform.position,ControPlayer.Ins.GetpositionPlayerCam(),Speedzoom);
            } 
        }
        if(Watered){
            if(water.GethighWater() <= -8f){
                water.WaterGoOn(SpeedDownCam*0.8f);
            }else{
                audioManage.PlayeSFX(audioManage.audioClips[10]);
                audioManage.SetMusic(audioManage.audioClips[0]);
                CTBoss.SetEndlEven();
                this.enabled = false;
            }
        }
    }
}

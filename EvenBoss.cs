using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenBoss : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Door1;
    public GameObject Door2;
    AnimationDoor animDoor1;
    AnimationDoor animDoor2;
    public GameObject CameraPlayer;
    public GameObject Backgroundmap;
    public GameObject Boss;
    public Vector3[] Target;
    public float SizeZoom;
    public float Speedzoom;
    private Camera cam;
    private ControllBoss CTBoss;
    private bool ZoomCam;
    private float ZoomSize;
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
        cam = CameraPlayer.transform.GetComponent<Camera>();
        CTBoss = transform.Find("ControBoss").GetComponent<ControllBoss>();
        ZoomCam = false;
        ZoomSize = cam.orthographicSize + SizeZoom;   
        spawedBoss = false;  
        CTBoss.enabled = false;
    }
    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Player")){
            //dong cua
            CloseTheDoor();
            //dieu chinh camera
            ZoomCam = true;
            Setpositioncam(false);
            OffBackground(false);
            //tha cho
            SpawBoss();
            CTBoss.enabled = true;
            //hien thanh mau
            m_ui.ShowHPboss(true);
            m_ui.SetHpSliderBoss(1f);
            //tat colide
            GetComponent<Collider2D>().enabled = false;
            audioManage.SetMusic(audioManage.audioClips[8]);
        }
    }
    public void ResetBoss(){
        //dong cua
        if(CTBoss.enabled){
            audioManage.SetMusic(audioManage.audioClips[0]);
            spawedBoss = false;
            m_ui.ShowHPboss(false);
            DestroyBoss();
            OpenTheDoor(); 
            GetComponent<Collider2D>().enabled = true;
            //dieu chinh camera
            ZoomCam = false;
            Setpositioncam(true);
            OffBackground(true);
            //huy boss
            CTBoss.enabled = false;
        }
    }
    public void CloseBoss(){
        OpenTheDoor();
        Setpositioncam(true);
        OffBackground(true);
    }
    public void SetDefaulPlayerCam(){ 
        audioManage.SetMusic(audioManage.audioClips[0]);
        audioManage.PlayeSFX(audioManage.audioClips[10]);
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
            boss.transform.GetComponent<BossGoblin>().SetControlleBoss(CTBoss);
            spawedBoss = true;
        }   
    }
    private void LateUpdate(){
        //zoom camera
        if(ZoomCam){
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize,ZoomSize,Speedzoom);
            cam.transform.position = Vector3.Lerp(cam.transform.position,Target[0],Speedzoom);
        }else{
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize,ZoomSize-SizeZoom,Speedzoom);
            cam.transform.position = Vector3.Lerp(cam.transform.position,ControPlayer.Ins.GetpositionPlayerCam(),Speedzoom);
        }
    }
}

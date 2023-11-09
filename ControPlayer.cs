using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControPlayer : MonoBehaviour
{
    // Start is called before the first frame update
    public static ControPlayer Ins;
    public GameObject player;
    public GameObject backgroun;
    public GameObject mainCamera;
    public GameObject ConTrollerenemy;
    private GameObject RSPointOJ;
    private GameObject Player;
    private Vector3 RespawnPoint;
    UIManager m_ui;
    public bool spawnplayed;
    void Start(){
        spawnplayed = false;
        RespawnPoint = transform.position;
        SpawnPlayer();
        m_ui = FindObjectOfType<UIManager>();
    }
    private void Awake(){
        Ins = this;
    }
    public Vector2 GetpositionPlayer(){
        return Player.GetComponent<player>().Getposition();
    }
    public Vector3 GetpositionPlayerCam(){
        Vector3 positionCamPlayer = 
        new Vector3(Player.GetComponent<player>().Getposition().x + 2f
        ,Player.GetComponent<player>().Getposition().y + 2f
        ,mainCamera.transform.position.z);
        return positionCamPlayer;
    }
    public LayerMask GetLayersPlayer(){
        return Player.layer;
    }
    public bool GetlifePlayer(){
        return Player.GetComponent<player>().life;
    }
    public void SpawnPlayer(){
        if(!spawnplayed){
            Player = Instantiate(player,RespawnPoint, gameObject.transform.localRotation) as GameObject;
            backgroun.GetComponent<Backgroundmap>().SetgameObject(Player.transform);
            backgroun.GetComponent<FollowAQ>().SetgameObject(Player.transform);
            mainCamera.GetComponent<CameraFollow>().SetgameObject(Player.transform);
            if(ConTrollerenemy){
                ConTrollerenemy.GetComponent<FollowColidePlayer>().SetgameObject(Player.transform);
            }
            Player.GetComponent<player>().SetControPlayer(gameObject);
            spawnplayed = true;
        }
    }
    public void Replay(){
        m_ui.ShowGameoverPanel(false);
        Player.GetComponent<player>().Setvelocity(0,0);
        Player.GetComponent<player>().RespawnPoints();
        Player.GetComponent<player>().PlayerTeLePoints(RespawnPoint);
        if(RSPointOJ){
            RSPointOJ.GetComponent<RespawnPoint>().RespawnPoints();
        }
    }
    public void SetGameOver(){
        m_ui.ShowGameoverPanel(true);
    }
    public void SetHpplayer(float HP){
        m_ui.SetHpSlider(HP);
    }
    public void SetRespawnPoint(GameObject rSPointOJ,Vector3 respawnPoint){
        RespawnPoint = respawnPoint;
        RSPointOJ = rSPointOJ;
        RSPointOJ.GetComponent<RespawnPoint>().SaveRespawnPoints();
    }
    public void SetTakeDamagePlayer(int damage,string AttackName){
        Player.GetComponent<player>().TakeDamage(damage,AttackName);
    }
    public void SetscenesInfoBoss(int bossdie){
        Player.GetComponent<player>().SetscenesInfoBoss(bossdie);
    }
    public void SetProgress(int progress){
        m_ui.SetProgress(progress);
    }
    public void SetAces(bool aces){
        m_ui.SetActiveAces(aces);
    }
    // Update is called once per frame
}

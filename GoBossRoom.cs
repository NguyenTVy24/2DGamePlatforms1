using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBossRoom : MonoBehaviour
{
    // Start is called before the first frame update
    public float delaytime;
    public string nameScene;
    public Vector2 NextPositionPlayer;
    public bool GateHome;
    private int HpPlayer;
    [SerializeField]
    public ScenesInfo scenesInfo;
    AudioManage audioManage;
    void Awake(){
        audioManage = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManage>();
    }   
    void OnTriggerStay2D(Collider2D col)
    {
        if(col.CompareTag("Player") && Input.GetKeyDown(KeyCode.F)){
            col.gameObject.SetActive(false);
            HpPlayer=col.GetComponent<player>().GetcurrentHealth();
            ModelSelect();
        }
    }
    public void ModelSelect(){
        if(GateHome){
            audioManage.PlayeSFX(audioManage.audioClips[12]);
        }
        StartCoroutine(LoadAfterDelay());
    }
    IEnumerator LoadAfterDelay(){
        yield return new WaitForSeconds(delaytime);
        scenesInfo.PositionPlayer = NextPositionPlayer;
        scenesInfo.HpPlayer = HpPlayer;
        SceneManager.LoadScene(nameScene);
    }
}

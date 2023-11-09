using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    public Slider Hpplayer;
    public Slider Hpboss;
    public GameObject SlideHPboss;
    public GameObject gameoverPanel;
    public Text Progress;
    public GameObject Aces;
    [SerializeField]
    public ScenesInfo scenesInfo;
    public void SetProgress(int progress){
        Debug.Log(progress);
        Progress.text ="Progress: "+(progress*100/3)+ "%";
    }
    public void SetHpSlider(float HP){
        if(Hpplayer){
            Hpplayer.value = HP;
        }
    }
    public void ShowGameoverPanel(bool isShow){
        if(gameoverPanel){
            gameoverPanel.SetActive(isShow);
        }
    }
    public void SetHpSliderBoss(float HP){
        if(Hpboss){
            Hpboss.value = HP;
        }
    }
    public void ShowHPboss(bool isShow){
        if(SlideHPboss){
            SlideHPboss.SetActive(isShow);
        }
    }
    public void SetActiveAces(bool aces){
        Aces.SetActive(aces);
    }
    public void ThoatMenu(){
        scenesInfo.SetDeflau();
        SceneManager.LoadScene(0);
    }
}

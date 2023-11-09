using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watchman : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ChatBoard;
    Animator anim;
    void Start(){
        anim = GetComponent<Animator>();
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player")){
            ChatBoard.SetActive(true);
        }
    }
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("Player")){
            ChatBoard.SetActive(false);
        }
    }
    public void FendOff(){
        thunleft_right(CheckLR());
        anim.SetTrigger("attack");
    }
    private void thunleft_right(bool LR){
        Vector3 kich_thuoc =transform.localScale;
        if(LR){
            kich_thuoc.x = 1;
        }else{
            kich_thuoc.x = -1;
        }
        transform.localScale = kich_thuoc;
        ChatBoard.transform.localScale = kich_thuoc;
    }
    private bool CheckLR(){
        float x = ControPlayer.Ins.GetpositionPlayer().x;
        if(transform.position.x - x > 0){
            return true;
        }
        return false;
    }
}

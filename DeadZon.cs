using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadZon : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter2D(Collision2D col){
        if(col.gameObject.CompareTag("Player")){
            if(ControPlayer.Ins.GetlifePlayer()){
                col.gameObject.GetComponent<player>().DieNoBlood(); 
            }
        }
    }
}

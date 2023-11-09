using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnPoint : MonoBehaviour
{
    // Start is called before the first frame 
    private Animator anim;
    void Start()
    {
        anim = GetComponent<Animator>();
    }
    // Update is called once per frame
    public void SaveRespawnPoints(){
        anim.SetBool("daluu",true);
    }
    public void RespawnPoints(){
        anim.SetTrigger("hoisinh");
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioMenu : MonoBehaviour
{

    AudioManage audioManage;

    void Awake(){
        audioManage = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManage>();
    }
    void Start()
    {
        audioManage.SetMusic(audioManage.audioClips[16]);
    }
}

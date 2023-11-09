using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateGohome : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    public ScenesInfo scenesInfo;
    public GameObject TilemapGateHome;
    void Start()
    {
        if(scenesInfo.GetBoss() == 3){
            TilemapGateHome.SetActive(true);
            GetComponent<Collider2D>().enabled = true;
        }
    }
}

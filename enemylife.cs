using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemylife : MonoBehaviour
{
    public bool life = true;
    public GameObject thisenemy;
    private void OnEnable()
    {
        life = true;
    }
    public void SetgameObject(GameObject enemy){
        thisenemy = enemy;
    }
    public void Destroyenemy(){
        if(life==false){
            thisenemy.GetComponent<DestroyEven>().destroyEvent();
        }
    }
}

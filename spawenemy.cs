using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawenemy : MonoBehaviour
{
    public GameObject enemy;
    public bool spawned = false;
    public float timespawned = 10f;
    private bool enemydie;
    private float nextspawnedtime =0f;
    void Start(){
    }
    void Update(){
        if(spawned && enemydie){
            nextspawnedtime += Time.deltaTime;
        }
        if(nextspawnedtime >= timespawned){
            spawned = false;
        }
    }
    public void Spawn(){
        if(!spawned){
            GameObject Enemy = Instantiate(enemy,transform.position, gameObject.transform.localRotation) as GameObject;
            Enemy.transform.Find("diemtua").GetComponent<enemylife>().SetgameObject(Enemy);
            Enemy.GetComponent<DestroyEven>().SetparSpawen(this.gameObject);
            spawned = true;
            enemydie = false;
            nextspawnedtime = 0f;
        }
    }
    public void SetSpawen(bool enemydie){
        this.enemydie=enemydie;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("GameController")){
            Spawn();
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllerenemy : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D col)
    {
        if(col.CompareTag("Kill")){
            col.GetComponent<enemylife>().Destroyenemy();
        }
    }
}

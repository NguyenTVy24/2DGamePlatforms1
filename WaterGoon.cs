using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterGoon : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] taget;
    private Vector2[] defaul;
    Rigidbody2D Water1;
    Rigidbody2D Water2;
    Rigidbody2D Water3;
    Rigidbody2D Water4;
    void Start()
    {
        Water1 = taget[0].GetComponent<Rigidbody2D>();
        Water2 = taget[1].GetComponent<Rigidbody2D>();
        Water3 = taget[2].GetComponent<Rigidbody2D>();
        Water4 = taget[3].GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    public float GethighWater(){
        return Water1.position.y;
    }
    public void DefaulWater(){
        Water1.position = new Vector2(Water1.position.x,-97.67f);
        Water2.position = new Vector2(Water2.position.x,-97.67f);
        Water3.position = new Vector2(Water3.position.x,-97.67f);
        Water4.position = new Vector2(Water4.position.x,-97.67f);
    }
    public void WaterGoOn(float speed){
        Water1.position = new Vector2(Water1.position.x,Water1.position.y+speed);
        Water2.position = new Vector2(Water2.position.x,Water2.position.y+speed);
        Water3.position = new Vector2(Water3.position.x,Water3.position.y+speed);
        Water4.position = new Vector2(Water4.position.x,Water4.position.y+speed);
    }
}

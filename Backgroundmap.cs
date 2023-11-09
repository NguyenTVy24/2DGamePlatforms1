using UnityEngine;

public class Backgroundmap : MonoBehaviour
{   
    private Transform FollowTF;
    private float offsetX;
    private float offsetY;
    private Material mat;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update(){
        offsetX += (Time.deltaTime * FollowTF.GetComponent<Rigidbody2D>().velocity.x)/110f;
        offsetY += (Time.deltaTime * FollowTF.GetComponent<Rigidbody2D>().velocity.y)/110f;
        mat.SetTextureOffset("_MainTex", new Vector2(offsetX,offsetY));
    }
    public void SetgameObject(Transform followTF){
        FollowTF = followTF;
    }
}

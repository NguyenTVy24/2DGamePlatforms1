using UnityEngine;

public class backgroun : MonoBehaviour
{
    // Start is called before the first frame update
    [Range(-1f,1f)]
    public float scrollSeed =0.5f;
    private float offset;
    private Material mat;
    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }
    // Update is called once per frame
    void Update()
    {
        offset += (Time.deltaTime * scrollSeed)/ 10f;
        mat.SetTextureOffset("_MainTex", new Vector2(offset,0));   
    }
}

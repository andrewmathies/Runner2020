using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideBackground : MonoBehaviour
{
    public float ScrollSpeed;
    Renderer rend;
    
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        //Debug.Log("name of main material: " + rend.material.name);
        string[] names = rend.material.GetTexturePropertyNames();
        //Debug.Log("texture names:\n" + string.Join("\n", names));
    }

    // Update is called once per frame
    void Update()
    {
        float offset = Time.time * ScrollSpeed;
        //Debug.Log(rend.material.mainTextureOffset.x);
        rend.material.SetTextureOffset("_MainTex", new Vector2(offset, 0));
    }
}

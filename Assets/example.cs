using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class example : MonoBehaviour
{

    private float x, y, z;
    public float speed;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("hello");
    }

    // Update is called once per frame
    void Update()
    {
	float move = Input.GetAxis("Horizontal");
	transform.Translate(move * Time.deltaTime * speed, 0, 0);
    }
}

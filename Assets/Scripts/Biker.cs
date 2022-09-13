using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biker : MonoBehaviour
{
    public Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float v = Input.GetAxisRaw("Vertical");
        ani.SetInteger("Vertical", (int)v);
        transform.Translate(Vector3.forward * v * Time.deltaTime);
    }
}

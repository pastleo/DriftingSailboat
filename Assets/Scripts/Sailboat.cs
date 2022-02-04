using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Sailboat : MonoBehaviour
{
    public float force = 10.0f;

    Rigidbody rigidbodyCom;


    // Start is called before the first frame update
    void Start()
    {
        rigidbodyCom = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Jump"))
        {
            rigidbodyCom.AddForce(new Vector3(Input.GetAxis("Horizontal") * 5, 0, force));
        }

        if (transform.position.y <= 0)
        {
            rigidbodyCom.AddForce(-Physics.gravity);
        }
    }
}

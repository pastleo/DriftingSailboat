using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public GameObject sailboat;
    public Vector3 position;
    public Animator animator;

    void Start()
    {
        animator.SetBool("started", true);
    }

    void Update()
    {
        transform.position = sailboat.transform.position + position;
    }
}

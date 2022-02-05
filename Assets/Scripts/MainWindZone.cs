using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWindZone : MonoBehaviour
{
    public Vector3 baseRotation = new Vector3(0, 0, 0);
    public float directionChanging = 1.0f;
    public float directionMaxBias = 45.0f;
    public float directionToCenterScale = 1.2f;
    public float strengthChanging = 1.0f;
    public float strengthMax = 10.0f;
    public float windPointerStrengthScale = 0.25f;

    public GameObject sailboat;
    public GameObject windPointer;
    public Animator windPointerAnimator;

    float strength = 0.0f;

    public Vector3 GetWindVector()
    {
        return transform.rotation * (new Vector3(0, 0, 1)) * strength;
    }

    void Update()
    {
        float rotationY = (Mathf.PerlinNoise(Time.time * directionChanging, 0) - 0.5f) * directionMaxBias * 2 - sailboat.transform.position.x * directionToCenterScale;
        transform.rotation = Quaternion.Euler(0, rotationY, 0);
        strength = Mathf.PerlinNoise(0, Time.time * strengthChanging) * strengthMax;

        windPointer.transform.rotation = transform.rotation;
        windPointerAnimator.SetFloat("speed", strength * windPointerStrengthScale);
    }
}

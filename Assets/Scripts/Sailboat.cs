using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Sailboat : MonoBehaviour
{
    public float force = 10.0f;
    public float floatingYBase = 0.8f;
    public float floatingYScale = 2.4f;
    public Animator animator;

    public TerrainGenerator terrainGenerator;
    public MainCamera mainCamera;
    public MainWindZone mainWindZone;
    public GameObject mainText;
    public GameObject statusText;

    Rigidbody rigidbodyCom;
    bool started = false;
    bool alive = true;
    int score = 0;

    void Start()
    {
        rigidbodyCom = GetComponent<Rigidbody>();
    }

    void Update()
    {
        rigidbodyCom.AddForce(Physics.gravity * -Mathf.Clamp(floatingYBase - floatingYScale * transform.position.y, 0, 1));

        if (!alive) return;

        if (Input.GetButton("Jump"))
        {
            if (!started && mainCamera != null)
            {
                started = true;
                mainCamera.enabled = true;
                mainText.GetComponent<Text>().text = "";
                animator.SetBool("started", true);
            }

            var windVector = mainWindZone.GetWindVector();
            rigidbodyCom.AddForce(windVector);
        }

        if (started)
        {
            animator.SetBool("sailing", Input.GetButton("Jump"));

            int newScore = (int) (transform.position.z * 10.0f);
            if (newScore > score)
            {
                score = newScore;
                statusText.GetComponent<Text>().text = $"Score: {score}";

                if (transform.position.z > terrainGenerator.latestGeneratedNear)
                {
                    terrainGenerator.Generate();
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        alive = false;
        mainText.GetComponent<Text>().text = $"Game Over\nYour score: {score}";
    }
}

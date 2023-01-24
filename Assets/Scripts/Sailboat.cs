using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Sailboat : MonoBehaviour
{
    public float floatingYBase = 0.8f;
    public float floatingYScale = 2.4f;
    public float sinkAfterHit = 5.0f;
    public float sinkDuration = 55.0f;
    public float sinkExponent = 4.0f;

    public Animator animator;

    public TerrainGenerator terrainGenerator;
    public MainCamera mainCamera;
    public MainWindZone mainWindZone;
    public GameObject mainPanel;
    public GameObject mainText;
    public GameObject statusText;

    Rigidbody rigidbodyCom;
    bool started = false;
    bool alive = true;
    float hitTime;
    int score = 0;

    void Start()
    {
        rigidbodyCom = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        Vector3 floatForce = Physics.gravity * -Mathf.Clamp(floatingYScale * (floatingYBase - transform.position.y), 0, 1);
        if (alive) {
            rigidbodyCom.AddForce(floatForce);
        } else {
            float effective = 1.0f - Mathf.Pow(Mathf.Clamp((Time.time - hitTime - sinkAfterHit) / sinkDuration, 0, 1), sinkExponent);
            rigidbodyCom.AddForce(floatForce * effective);
        }
    }

    void Update()
    {
        if (!alive) return;

        bool inputDown = Input.touchCount > 0 || Input.GetButton("Jump");

        if (inputDown)
        {
            if (!started && mainCamera != null)
            {
                started = true;
                mainCamera.enabled = true;
                mainPanel.SetActive(false);
                animator.SetBool("started", true);
            }

            var windVector = mainWindZone.GetWindVector();
            rigidbodyCom.AddForce(windVector);
        }

        if (started)
        {
            animator.SetBool("sailing", inputDown);

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
        if (alive) {
            alive = false;
            hitTime = Time.time;
            mainText.GetComponent<Text>().text = $"Game Over\nYour score: {score}";
            mainPanel.SetActive(true);
        }
    }
}

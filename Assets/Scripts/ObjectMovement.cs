using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;
    private static float velocity = 10;
    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(0, 0, -velocity);
        if (transform.position.z < -50)
        {
            Score.GameScore += 1;
            Obstacle.obstacles.RemoveAt(0);
            Destroy(gameObject);

        }
    }
}

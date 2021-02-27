using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector3(0, 0, -10);
        if (transform.position.z < -50)
        {
            Score.GameScore += 1;
            Destroy(gameObject);
        }
    }
}

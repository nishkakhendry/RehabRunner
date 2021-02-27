using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private Vector3 end;
    public Rigidbody rb; 
    private bool moving = false;
    private float eps = 0.01f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Right Event
        if (Input.GetKeyDown(KeyCode.RightArrow))
        { 
            if ((transform.position.x + 2.5f < 5.0f)&&(!moving) && (Mathf.Abs(transform.position.y + 1))<eps) {
                moving = true;
                end = transform.position + new Vector3(2.5f,0,0);
            }
            
        }
        // Left Event
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        { 
            if ((transform.position.x - 2.5f > -5.0f)&&(!moving)&& (Mathf.Abs(transform.position.y + 1))<eps) {
                moving = true;
                end = transform.position + new Vector3(-2.5f,0,0);
            }
            
        }

        // Up Event
        if (Input.GetKeyDown(KeyCode.UpArrow)) {
            rb.AddForce(0,500,0);
        }
        if (moving) {
            float timeDelta = 3*Mathf.Abs(end.x - transform.position.x);
            transform.position = Vector3.MoveTowards(transform.position, end, timeDelta*Time.deltaTime);
        }

        if (Mathf.Abs(transform.position.x-end.x)<eps) {
            moving = false;
            
        }
    }
}

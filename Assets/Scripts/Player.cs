using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    private Vector3 end;
    public Rigidbody rb;
    public BoxCollider bc;
    private bool moving = false;
    private bool jumping = false;
    private float timeSinceJump = 0f;
    private float jumpSkip = 0.1f;
    private float eps = 0.01f;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {   
        animator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (jumping && (timeSinceJump<jumpSkip)) {
            timeSinceJump += Time.deltaTime;
        } else {
            timeSinceJump = 0f;
            if ((Mathf.Abs(transform.position.y + 1.5f) < eps)&&jumping){
                animator.runtimeAnimatorController = Resources.Load("BasicMotions@Run") as RuntimeAnimatorController;
                jumping = false;
            }
            
            // Right Event
            if (Input.GetKeyDown(KeyCode.RightArrow))
            { 
                if ((transform.position.x + 2.5f < 5.0f)&& (Mathf.Abs(transform.position.y + 1.5f))<eps) {
                    moving = true;
                    end = transform.position + new Vector3(2.5f,0,0);
                }
                
            }
            // Left Event
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            { 
                if ((transform.position.x - 2.5f > -5.0f)&& (Mathf.Abs(transform.position.y + 1.5f))<eps) {
                    moving = true;
                    end = transform.position + new Vector3(-2.5f,0,0);
                }
                
            }

            // Up Event
            if (Input.GetKeyDown(KeyCode.Space)) {
                
                //set animator to jump
                jumping = true;
                animator.runtimeAnimatorController = Resources.Load("BasicMotions@Jump") as RuntimeAnimatorController;
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

        

        //if position less than -1.5, set jumping to false and change animator back to running
        
    }

    void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.name!="Platform") {
            Debug.Log("Game Over");
            SceneManager.LoadScene("GameOver");
            
        }
    }
}

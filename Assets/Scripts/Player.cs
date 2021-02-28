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
    public enum PossibleMoves {
        Left,
        Right,
        Jump, 
        Idle
    }
    public static List<PossibleMoves> possibleMoves;
    // Start is called before the first frame update
    void Start()
    {   
        animator = gameObject.GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        possibleMoves = generatePossibleMoves();
        
        if (jumping && (timeSinceJump<jumpSkip)) {
            timeSinceJump += Time.deltaTime;
        } else {

            timeSinceJump = 0f;
            if ((Mathf.Abs(transform.position.y + 1.5f) < eps)&&jumping){
                animator.runtimeAnimatorController = Resources.Load("BasicMotions@Run") as RuntimeAnimatorController;
                jumping = false;
            }
            
            // Right Event
            // Debug.Log(gonnaColide());
            if (Input.GetKeyDown(KeyCode.RightArrow))
            { 
                // if (gonnaColide()) {
                    moving = true;
                    end = new Vector3(bestRight(),transform.position.y,transform.position.z);
                // }
                // if ((transform.position.x + 2.5f < 5.0f)&& (Mathf.Abs(transform.position.y + 1.5f))<eps) {
                //     moving = true;
                //     end = transform.position + new Vector3(2.5f,0,0);
                // }
                
            }
            // Left Event
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // if (gonnaColide()) {
                    moving = true;
                    end = new Vector3(bestLeft(),transform.position.y,transform.position.z);
                // }

                // if ((transform.position.x - 2.5f > -5.0f)&& (Mathf.Abs(transform.position.y + 1.5f))<eps) {
                //     moving = true;
                //     end = transform.position + new Vector3(-2.5f,0,0);
                // }
                
            }

            // Up Event
            if (Input.GetKeyDown(KeyCode.Space)) {
                
                //set animator to jump
                jumping = true;
                animator.runtimeAnimatorController = Resources.Load("BasicMotions@Jump") as RuntimeAnimatorController;
                rb.AddForce(0,500,0);

            }
            if (moving) {
                float timeDelta = 5*Mathf.Abs(end.x - transform.position.x);
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
            WebcamScript.webcamTexture.Stop();
            SceneManager.LoadScene("GameOver");
            
        }
    }

    float bestLeft() {
        GameObject nextObstacle = Obstacle.obstacles[0];
        float obstacleCenterX = nextObstacle.transform.position.x;
        float xScale = nextObstacle.transform.localScale.x;
        float leftX = obstacleCenterX - xScale/2;
        float width = bc.size.x;
        float perfectSpot = leftX - 0.2f - width/2;
        if (perfectSpot > transform.position.x){
            return transform.position.x;

        } else if (perfectSpot>(-6+width/2)) {
            return (-6 + leftX)/2;
        } else {
            return -6+width/2;
        }
    }

    float bestRight() {
        GameObject nextObstacle = Obstacle.obstacles[0];
        float obstacleCenterX = nextObstacle.transform.position.x;
        float xScale = nextObstacle.transform.localScale.x;
        float rightX = obstacleCenterX + xScale/2;
        float width = bc.size.x;
        float perfectSpot = rightX + 0.2f + width/2;


        if (perfectSpot < transform.position.x){
            return transform.position.x;
        } else if (perfectSpot<(6-width/2)) {
            return (6+rightX)/2;
        } else {
            return 6-width/2;
        }
    }

    bool gonnaColide() {
        float currXPos = transform.position.x;
        GameObject nextObstacle = Obstacle.obstacles[0];
        float obstacleCenterX = nextObstacle.transform.position.x;
        float xScale = nextObstacle.transform.localScale.x;
        float leftX = obstacleCenterX - xScale/2;
        float rightX = obstacleCenterX + xScale/2;

        if ((leftX - 0.3 <= currXPos)&&(rightX +0.3>=currXPos)) {
            return true;
        } else {
            return false;
        }
    }

    List<PossibleMoves> generatePossibleMoves(){
        List<PossibleMoves> possibleMoves = new List<PossibleMoves>();

        float currXPos = transform.position.x;
        GameObject nextObstacle = Obstacle.obstacles[0];
        float obstacleCenterX = nextObstacle.transform.position.x;
        float xScale = nextObstacle.transform.localScale.x;
        float yScale = nextObstacle.transform.localScale.y;
        float leftX = obstacleCenterX - xScale/2;
        float rightX = obstacleCenterX + xScale/2;
        float width = bc.size.x;
        

        
        if (yScale <= 2){
            possibleMoves.Add(PossibleMoves.Jump);
        }
        float perfectSpot = leftX - 0.2f - width/2;

        if (perfectSpot>(-6+width/2)) {
            possibleMoves.Add(PossibleMoves.Left);
        }

        perfectSpot = rightX + 0.2f + width/2;

        if (perfectSpot<(6-width/2)) {
            possibleMoves.Add(PossibleMoves.Right);
        }

        if (!gonnaColide()) {
            possibleMoves.Add(PossibleMoves.Idle);
        }
       
        string possible = "[";
        foreach( var x in possibleMoves) {
            possible = possible + x.ToString() + ", ";
        }
        
        return possibleMoves;

    }
}

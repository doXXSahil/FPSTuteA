using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 12f;
    public float gravity = -9.8f * 2;
    public float jump = 3f;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 lastPositin = new Vector3(0f, 0f, 0f);

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        // Reset velocity
        if(isGrounded && velocity.y < 0f )
        {
            velocity.y = -2f;
        }

        //Getting inputs 
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        // creating the moving vector
        Vector3 move = transform.right * x + transform.forward * z;//right - red axis , forward - blue axis

        //Actually moving the player 
        controller.Move(move*speed*Time.deltaTime);

        //checking for jump
        if(Input.GetButtonDown("Jump") && isGrounded)
        {
            //going up
            velocity.y = Mathf.Sqrt(jump * -2f * gravity);
        }

        //Falling Down
        velocity.y += gravity*Time.deltaTime;
        
        //ececuting the jump 
        controller.Move(velocity * Time.deltaTime);

        if(lastPositin != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        lastPositin = gameObject.transform.position;
    }
}

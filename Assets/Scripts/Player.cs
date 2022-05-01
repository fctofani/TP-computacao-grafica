using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;

    private float jumpSpeed;

    public float speed;
    public float jumpHeight;
    public float gravity = -9.07f;
    public float horizontalSpeed;

    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        anim.SetBool("Jumping", false);
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.forward * speed;

        if(controller.isGrounded) {
            anim.SetBool("Jumping", false);
            jumpSpeed = 0;    
            if(Input.GetKeyDown(KeyCode.Space)) {   
                jumpSpeed += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
                anim.SetBool("Jumping", true);
            }
            if(Input.GetKeyDown(KeyCode.A) && transform.position.x > -1.4f) {
                StartCoroutine(LeftMove());
            }
            if(Input.GetKeyDown(KeyCode.D) && transform.position.x < 1.4f) {
                StartCoroutine(RightMove());
            }
        } else {   
            jumpSpeed += gravity * Time.deltaTime;
            
        }

        direction.y = jumpSpeed;
        controller.Move(direction * Time.deltaTime);
    }

    IEnumerator LeftMove() 
    {
        for(float i = 0; i < 5; i += 0.1f) {
            controller.Move(Vector3.left * Time.deltaTime * horizontalSpeed);
            yield return null;
        }
    }

    IEnumerator RightMove()
    {
        for(float i = 0; i < 5; i += 0.1f) {
            controller.Move(Vector3.right * Time.deltaTime * horizontalSpeed);
            yield return null;
        }
    }
}

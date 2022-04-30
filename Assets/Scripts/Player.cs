using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private CharacterController controller;

    private float jumpSpeed;

    public float speed;
    public float jumpHeight;
    public float gravity;
    public float horizontalSpeed;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = Vector3.forward * speed;

        if(controller.isGrounded) {
            if(Input.GetKeyDown(KeyCode.Space)) {
                jumpSpeed = jumpHeight;
            }
            if(Input.GetKeyDown(KeyCode.A)) {
                StartCoroutine(LeftMove());
            }
            if(Input.GetKeyDown(KeyCode.D)) {
                StartCoroutine(RightMove());
            }
        } else {
            jumpSpeed -= gravity;
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

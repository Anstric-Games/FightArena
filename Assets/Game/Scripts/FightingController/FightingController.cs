using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingController : MonoBehaviour
{
                            
    public float movementSpeed = 3f;
    public float rotationSpeed = 10f;
    private CharacterController characterController;
    private Animator animator;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        PerformMovement();
    }

    void PerformMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(-verticalInput, 0, horizontalInput);

        if (movement != Vector3.zero)
        {

            Quaternion targetRotation = Quaternion.LookRotation(movement);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            if (horizontalInput > 0)
            {
                animator.SetBool("Walking", true);
            }
            else if (horizontalInput < 0)
            {
                animator.SetBool("Walking", true);
            }
            else
            {
                animator.SetBool("Walking", true);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        characterController.Move(movement * movementSpeed * Time.deltaTime);

    }
}

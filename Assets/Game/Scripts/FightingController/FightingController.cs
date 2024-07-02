using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightingController : MonoBehaviour
{
    [Header("Player Movement")]
    public float movementSpeed = 3f;
    public float rotationSpeed = 10f;
    private CharacterController characterController;
    private Animator animator;

    [Header("Player Fight")]
    public float attackCooldown = 0.5f;
    public int attackDamages = 5;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    public float dodgeDistance = 2f;
    private float lastAttackTIme;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        PerformMovement();
        PerformDodgeFront();

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PerformAttack(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PerformAttack(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PerformAttack(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PerformAttack(3);
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

        void PerformAttack(int attackIndex)
        {
            if (Time.time - lastAttackTIme > attackCooldown)
            {
                animator.Play(attackAnimations[attackIndex]);

                int damage = attackDamages;
                Debug.Log("Performed attack" + (attackIndex + 1) + "dealing" + damage + "damage.");

                lastAttackTIme = Time.time;

                // Loop through each opponent.
            }
            else
            {
                // If the player tries to attack too quickly, inform them:
                Debug.Log("Cannot perform attack yet. Cooldown time left");
            }
        }

        void PerformDodgeFront()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                animator.Play("DodgeFrontAnimation");

                Vector3 dodgeDirection = transform.forward * dodgeDistance;

                characterController.Move(dodgeDirection);

            }
        }

    }
}
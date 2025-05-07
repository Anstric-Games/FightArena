using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;

public class FightingController : MonoBehaviour
{
    [Header("Player Movement")]
    public float movementSpeed = 3f;
    public float rotationSpeed = 10f;
    private CharacterController characterController;
    private Animator animator;
    public Camera mainCamera;


    [Header("Player Fight")]
    public float attackCooldown = 0.5f;
    public int attackDamages = 5;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    public float dodgeDistance = 2f;
    public float attackRadius = 2.2f;
    public float attackAngle = 60f;
    public Transform[] opponents;
    private float lastAttackTIme;

    [Header("Effects and Sound")]
    public ParticleSystem attack1Effect;
    public ParticleSystem attack2Effect;
    public ParticleSystem attack3Effect;
    public ParticleSystem attack4Effect;
    public AudioClip[] hitSounds;

    [Header("Health")]
    public int maxHealth = 100;
    public int currentHealth;
    public HealthBar healthBar;

    void Awake()
    {
        currentHealth = maxHealth;
        healthBar.GiveFullHealth(currentHealth);
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        PerformMovement();
        PerformDodgeFront();

        if (Input.GetKeyDown(KeyCode.Alpha1)) PerformAttack(0);
        else if (Input.GetKeyDown(KeyCode.Alpha2)) PerformAttack(1);
        else if (Input.GetKeyDown(KeyCode.Alpha3)) PerformAttack(2);
        else if (Input.GetKeyDown(KeyCode.Alpha4)) PerformAttack(3);
    }

    void PerformMovement()
    {
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");

        // Calculate movement direction based on Camera Orienc
        Vector3 forward = mainCamera.transform.forward;
        Vector3 right = mainCamera.transform.right;

        // Adjusting vectors onto the XZ plane (ground plane)
        forward.y = 0;
        right.y = 0;
        forward.Normalize();
        right.Normalize();

        // Create Camera-relative movement vector
        Vector3 movement = (forward * verticalInput) + (right * horizontalInput);

        // Adjust Player's rotation (face direction)
        Transform nearestOpponent = GetNearestOpponent();
        if (nearestOpponent != null)
        {
            Vector3 directionToOpponent = nearestOpponent.position - transform.position;
            directionToOpponent.y = 0;

            if (directionToOpponent.sqrMagnitude > 0.01f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToOpponent);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        if (movement != Vector3.zero)
            animator.SetBool("Walking", true);

        characterController.Move(movement * movementSpeed * Time.deltaTime);

    }

    Transform GetNearestOpponent()
    {
        Transform nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform opponent in opponents)
        {
            if (opponent.gameObject.activeSelf)
            {
                float dist = Vector3.Distance(transform.position, opponent.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = opponent;
                }
            }
        }
        return nearest;
    }

    void PerformAttack(int attackIndex)
    {
        if (Time.time - lastAttackTIme > attackCooldown)
        {
            animator.Play(attackAnimations[attackIndex]);

            //int damage = attackDamages;
            //Debug.Log("Performed attack" + (attackIndex + 1) + "dealing" + damage + "damage.");

            lastAttackTIme = Time.time;

            // Loop through each opponent.
            foreach (Transform opponent in opponents)
            {
                if (opponent.gameObject.activeSelf)
                {
                    float distance = Vector3.Distance(transform.position, opponent.position);
                    if (distance <= attackRadius)
                    {
                        if (IsOpponentInAttackAngle(opponent.position))
                        {
                            opponent.GetComponent<OpponentAI>().StartCoroutine(opponent.GetComponent<OpponentAI>().PlayHitDamageAnimation(attackDamages));
                        }
                    }
                }
            }
        }

    }

    bool IsOpponentInAttackAngle(Vector3 opponentPosition)
    {
        // Get direction to opponent (ignoring Y axis)
        Vector3 directionToOpponent = opponentPosition - transform.position;
        directionToOpponent.y = 0; // Ignore vertical difference
        directionToOpponent.Normalize();

        // Get forward direction of player (ignoring Y axis)
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0;
        forwardDirection.Normalize();

        // Calculate angle between forward direction and direction to opponent
        float angle = Vector3.Angle(forwardDirection, directionToOpponent);

        // If angle is less than half of our attack angle, opponent is in front
        return angle <= (attackAngle / 2);
    }


    void PerformDodgeFront()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            animator.Play("DodgeFrontAnimation");

            Vector3 dodgeDirection = transform.forward * dodgeDistance;

            dodgeDirection.y = 0;

            characterController.Move(dodgeDirection);

        }
    }

    public IEnumerator PlayHitDamageAnimation(int takeDamage)
    {
        yield return new WaitForSeconds(0.3f);

        //Play a random hit sound

        if (hitSounds != null && hitSounds.Length > 0)
        {
            int randomIndex = Random.Range(0, hitSounds.Length);
            AudioSource.PlayClipAtPoint(hitSounds[randomIndex], transform.position);
        }

        //Decrease Health
        currentHealth -= takeDamage;
        healthBar.SetHealth(currentHealth);



        animator.Play("HitDamageAnimation");
    }



    public void Attack1Effect() => attack1Effect.Play();
    public void Attack2Effect() => attack2Effect.Play();
    public void Attack3Effect() => attack3Effect.Play();
    public void Attack4Effect() => attack4Effect.Play();
}
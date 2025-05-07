using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpponentAI : MonoBehaviour
{
    [Header("Opponent Movement")]
    public float movementSpeed = 3f;
    public float rotationSpeed = 10f;
    public CharacterController characterController;
    public Animator animator;

    [Header("Opponent Fight")]
    public float attackCooldown = 0.5f;
    public int attackDamages = 5;
    public string[] attackAnimations = { "Attack1Animation", "Attack2Animation", "Attack3Animation", "Attack4Animation" };
    public float dodgeDistance = 2f;
    public int attackCount = 0;
    public int randomNumber;
    public float attackRadius = 2f;
    public float attackAngle = 60f;
    public FightingController[] fightingController;
    public Transform[] players;
    public bool isTakingDamage;
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
        createRandomNumber();
    }

    private void Update()
    {
        /*if (attackCount == randomNumber)
        {
            attackCount = 0;
            createRandomNumber();
        }*/

        for (int i = 0; i < fightingController.Length; i++)
        {

            if (players[i].gameObject.activeSelf)
            {
                float distance = Vector3.Distance(transform.position, players[i].position);
                if (distance <= attackRadius && IsPlayerInAttackAngle(players[i].position))
                {
                    //if (IsPlayerInAttackAngle(players[i].position))
                    //{
                    animator.SetBool("Walking", false);

                    if (Time.time - lastAttackTIme > attackCooldown && !isTakingDamage)
                    {
                        int randomAttackIndex = Random.Range(0, attackAnimations.Length);
                        PerformAttack(randomAttackIndex);

                        // Player Hit/Danage animation on the player.
                        fightingController[i].StartCoroutine(fightingController[i].PlayHitDamageAnimation(attackDamages));
                    }
                    //}
                }
                else
                {
                    Vector3 direction = (players[i].position - transform.position).normalized;
                    characterController.Move(direction * movementSpeed * Time.deltaTime);

                    Quaternion targetRotation = Quaternion.LookRotation(direction);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

                    animator.SetBool("Walking", true);
                }
            }
        }
    }

    void PerformAttack(int attackIndex)
    {
        animator.Play(attackAnimations[attackIndex]);

        Debug.Log("Performed attack" + (attackIndex + 1) + "dealing" + attackDamages + "damage.");

        lastAttackTIme = Time.time;
    }

    bool IsPlayerInAttackAngle(Vector3 playerPosition)
    {
        // Get direction to player (ignoring Y axis)
        Vector3 directionToPlayer = playerPosition - transform.position;
        directionToPlayer.y = 0; // Ignore vertical difference
        directionToPlayer.Normalize();

        // Get forward direction of opponent (ignoring Y axis)
        Vector3 forwardDirection = transform.forward;
        forwardDirection.y = 0;
        forwardDirection.Normalize();

        // Calculate angle between forward direction and direction to player
        float angle = Vector3.Angle(forwardDirection, directionToPlayer);

        // If angle is less than half of our attack angle, player is in front
        return angle <= (attackAngle / 2);
    }


    void createRandomNumber()
    {
        randomNumber = Random.Range(1, 5);
    }

    public IEnumerator PlayHitDamageAnimation(int takeDamage)
    {
        isTakingDamage = true;
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

        if (currentHealth <= 0)
        {
            Die();
        }

        animator.Play("HitDamageAnimation");
        isTakingDamage = false;
    }

    void Die()
    {
        Debug.Log("Opponent Died");
    }

    void PerformDodgeFront()
    {
        animator.Play("DodgeFrontAnimation");

        Vector3 dodgeDirection = -transform.forward * dodgeDistance;

        characterController.SimpleMove(dodgeDirection);
    }

    public void Attack1Effect() => attack1Effect.Play();
    public void Attack2Effect() => attack2Effect.Play();
    public void Attack3Effect() => attack3Effect.Play();
    public void Attack4Effect() => attack4Effect.Play();
}

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbody2D;
    [SerializeField] private TextMeshProUGUI playerNameTextField;
    
    public string PlayerName;
    public string PlayerId;
    [SerializeField] private float speed = 5;
    [SerializeField] private bool isLocalPlayer = false;
    public bool IsLocalPlayer
    {
        get { return isLocalPlayer; }
        set { isLocalPlayer = value; }
    }
    [SerializeField] private float attackDamage = 5;
    private Vector2 lastDirection = Vector2.zero;
    
    [SerializeField] private float damageValue = 20;
    public float DamageValue
    {
        get { return damageValue; }
    }

    [SerializeField] private bool isDead = false;

    private bool isFlying = false;
    private bool waitForFlyEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        playerNameTextField.SetText(PlayerName);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (isFlying)
        {
            if (waitForFlyEnd)
            {
                //Wait for velocity to reach lower magnitude
                if (rigidbody2D.velocity.magnitude < 0.1)
                {
                    waitForFlyEnd = false;
                    isFlying = false;
                }   
            }
        }
        else
        {
            if (isLocalPlayer && PlayerName == "P1")
            {
                float vertical = 0;
                if (Input.GetKey(KeyCode.UpArrow))
                {
                    vertical = 1;
                } else if (Input.GetKey(KeyCode.DownArrow))
                {
                    vertical = -1;
                }
            
                float horizontal = 0;
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    horizontal = -1;
                } else if (Input.GetKey(KeyCode.RightArrow))
                {
                    horizontal = 1;
                }
                UpdateMovement(horizontal, vertical);
        
                // Attack on mouse down
                if (Input.GetKeyDown(KeyCode.M))
                {
                    Attack();
                }
            }

            if (isLocalPlayer && PlayerName == "P2")
            {
                float vertical = 0;
                if (Input.GetKey(KeyCode.W))
                {
                    vertical = 1;
                } else if (Input.GetKey(KeyCode.S))
                {
                    vertical = -1;
                }
            
                float horizontal = 0;
                if (Input.GetKey(KeyCode.A))
                {
                    horizontal = -1;
                } else if (Input.GetKey(KeyCode.D))
                {
                    horizontal = 1;
                }
            
                UpdateMovement(horizontal, vertical);
                // Attack on mouse down
                if (Input.GetKeyDown(KeyCode.X))
                {
                    Attack();
                }
            }
        }


        UpdateAnimatorParameters();
    }

    void UpdateAnimatorParameters()
    {
        if(rigidbody2D.velocity.magnitude > 1)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }
    }
    
    public void UpdateInput(float horizontal, float vertical)
    {
        if (horizontal == 0 && vertical == 0)
        {
            lastDirection = Vector2.zero;
        }
        else
        {
            lastDirection = new Vector2(horizontal, vertical).normalized;
        }

    }
    
    public void UpdateMovement(float horizontal, float vertical)
    {
        
        // If the player is moving, update the last direction
        if (horizontal != 0 || vertical != 0)
        {
            // Set the animator parameters
            animator.SetFloat("X", horizontal);
            animator.SetFloat("Y", vertical);

            lastDirection = new Vector2(horizontal, vertical).normalized;
        }
        
        
        if (isFlying || isDead)
        {
            // Ignore Inputs
            return;
        }
        
        // Set the rigidbody velocity
        rigidbody2D.velocity = (new Vector2(horizontal, vertical)).normalized * speed;
    }

    public void Attack()
    {
        if (isFlying || isDead)
        {
            // Ignore Attack
            return;
        }
        StartCoroutine(ExecuteAttack(direction: lastDirection));
    }

    IEnumerator ExecuteAttack(Vector2 direction)
    {
        animator.SetTrigger("Attack");

        FMODManager.Instance.PlaySwordSound();

        yield return new WaitForSeconds(0.25f); // Wait for the attack animation to play
        
        // Send raycast 1 unit in the attack direction and punch every player that is hit
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 1);
        
        // Only damage players that are in a 30 degree angle to the attack direction
        foreach (Collider2D hit in hits)
        {
            if (hit.gameObject.CompareTag("Player"))
            {
                Vector2 hitDirection = hit.transform.position - transform.position;
                float angle = Vector2.Angle(direction, hitDirection);
                
                if (angle < 45)
                {
                    PlayerController playerController = hit.gameObject.GetComponent<PlayerController>();

                    if (playerController == this)
                    {
                        continue;
                    }
                    
                    playerController.Damage(direction, attackDamage);
                }
            }
        }
    }
    
    void Damage(Vector2 direction, float damage)
    {
        StartCoroutine(ExecuteDamage(direction, damage));
    }

    IEnumerator ExecuteDamage(Vector2 direction, float damage)
    {
        animator.SetTrigger("OnDamage");
        isFlying = true;


        yield return new WaitForSeconds(0.05f);
        // Punch the player in the direction
        
        // Add damage percentage
        damageValue += damage;
        rigidbody2D.AddForce((direction.normalized * (damage * 100) * GetForceBecauseOfDamageMultiplier()));

        yield return new WaitForSeconds(0.05f);
        waitForFlyEnd = true;
    }
    
    float GetForceBecauseOfDamageMultiplier()
    {
        float multiplier = damageValue / 100;
        
        return Mathf.Min(Mathf.Max(1, multiplier), 10);
    }

    public IEnumerator Kill()
    {
        animator.SetBool("IsDead", true);
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        
        yield return new WaitForSeconds(2);
        GameManager.Instance.RemovePlayer(this.PlayerId);
    }
}

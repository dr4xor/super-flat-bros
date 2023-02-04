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

    // Start is called before the first frame update
    void Start()
    {
        playerNameTextField.SetText(PlayerName);
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer && isDead == false)
        {
            // Get the horizontal and vertical input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            
            UpdateMovement(horizontal, vertical);
        
            // Attack on mouse down
            if (Input.GetKeyDown(KeyCode.M))
            {
                Attack();
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
    
    
    public void UpdateMovement(float horizontal, float vertical)
    {
        // If the player is moving, update the last direction
        if (horizontal != 0 || vertical != 0)
        {
            lastDirection = new Vector2(horizontal, vertical).normalized;
                
            // Set the animator parameters
            animator.SetFloat("X", lastDirection.x);
            animator.SetFloat("Y", lastDirection.y);
        }
        
        // Set the rigidbody velocity
        rigidbody2D.velocity = new Vector2(horizontal, vertical) * speed;
    }

    public void Attack()
    {
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
        animator.SetTrigger("OnDamage");
        
        // Punch the player in the direction
        rigidbody2D.AddForce((direction.normalized * (damage * 100) * GetForceBecauseOfDamageMultiplier()));
        
        
        // Add damage percentage
        damageValue += damage;
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

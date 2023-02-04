using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private Animator animator;
    [SerializeField] private Rigidbody2D rigidbody2D;
    
    [SerializeField] private float speed = 5;

    [SerializeField] private bool isLocalPlayer = false;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLocalPlayer)
        {
            // Get the horizontal and vertical input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");


            UpdateMovement(horizontal, vertical);
        
            // Attack on mouse down
            if (Input.GetMouseButtonDown(0))
            {
                ExecuteAttack();
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
    
    
    void UpdateMovement(float horizontal, float vertical)
    {
        // Set the animator parameters
        animator.SetFloat("X", horizontal);
        animator.SetFloat("Y", vertical);
        
        // Set the rigidbody velocity
        rigidbody2D.velocity = new Vector2(horizontal, vertical) * speed;
    }

    void ExecuteAttack()
    {
        animator.SetTrigger("Attack");
    }
}

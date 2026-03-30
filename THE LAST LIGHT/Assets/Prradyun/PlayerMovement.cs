using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public CharacterController controller;
    public float speed = 3f; 
    public float gravity = -9.81f;
    
    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    private Animator anim; 
    private Vector3 velocity;
    private bool isGrounded;
    private bool isDead = false; 

    void Start()
    {
        // Automatically find the Animator on this object (Player_Controller)
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // Stop all logic if Todd is dead
        if (isDead) return;

        // 1. Ground Check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; 
        }

        // 2. Input Handling (Using GetAxisRaw for snappy 1st person feel)
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        // 3. Animation Transitions (Idle <-> Walk)
        if (x != 0 || z != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }

        // 4. Actual Movement
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move.normalized * speed * Time.deltaTime);

        // 5. Physics / Gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // 6. Manual Death Trigger
        if (Input.GetKeyDown(KeyCode.X))
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        
        // Fire the 'die' trigger in the Animator
        if (anim != null)
        {
            anim.SetTrigger("die");
        }

        // Clean up physics so he doesn't float or block the camera
        velocity = Vector3.zero;
        controller.enabled = false;

        Debug.Log("Todd is down. Rest in peace.");
    }
}
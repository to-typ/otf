using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    bool isWalking;
    bool isRunning;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    bool isGrounded;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("jump", false);

        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        isWalking = Input.GetKey(KeyCode.W);
        isRunning = Input.GetKey(KeyCode.LeftShift);

        if (isGrounded)
        {
            animator.SetBool("isJumping", false);
        }
        if (isWalking)
        {
            animator.SetBool("isWalking", true);
        }
        if (!isWalking)
        {
           animator.SetBool("isWalking", false);
        }
        if (isRunning && isWalking)
        {
            animator.SetBool("isRunning", true);
        }
        if (!isRunning || !isWalking)
        {
            animator.SetBool("isRunning", false);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            animator.SetBool("isJumping", true);
        }
        
    }
}

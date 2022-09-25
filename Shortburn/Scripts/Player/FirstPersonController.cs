using UnityEngine;

public class FirstPersonController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterController controller;

    [Header("Settings")]
    [SerializeField] private float speed = 12f;
    [SerializeField] private float jumpHeight = 3f;
    [SerializeField] private float gravity = -9.81f;
    private float jumpTimer = 0.75f;

    [Header("GroundCheck Settings")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundDistance = 0.1f;
    [SerializeField] private LayerMask layer;
    private bool isGrounded;

    private Vector3 velocity;

    void Update()
    {
        //Groundcheck
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, layer);

        //reset velocity
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        //input
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //calculate direction
        Vector3 move = transform.right * x + transform.forward * z;

        //moves player
        controller.Move(move * speed * Time.deltaTime);

        jumpTimer -= Time.deltaTime;
        if (jumpTimer <= 0)
            jumpTimer = 0;

        //jump
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && jumpTimer == 0)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            jumpTimer = 0.75f;
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
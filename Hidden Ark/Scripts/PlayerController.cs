using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("References")]
    public Animator anim;
    public CharacterController controller;
    public Transform cam;
    private Pickup pickup;
    [SerializeField] private PlayerSettings settings;

    [SerializeField] private Transform groundCheck;
    private Vector3 moveDir;
    private Vector3 velocity;
    private bool isGrounded;
    private bool canDoubleJump;
    private float turnSmoothVelocity;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pickup = GetComponent<Pickup>();
        settings.dashCooldown = 0;
    }

    void Update()
    {
        //Groundcheck
        isGrounded = Physics.CheckSphere(groundCheck.position, settings.groundDistance, settings.groundMask);

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        //jump
        if (Input.GetButtonDown("Jump") && isGrounded)
            Jump();
        //double jump
        else if (Input.GetButtonDown("Jump") && !isGrounded && canDoubleJump && pickup.doubleJumpAcquired)
            DoubleJump();

        //land sound effect
        if (!isGrounded)
            AudioManager.instance.Play("Land", false);

        //gravity
        velocity.y += settings.gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        //input and calculating direction
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        //checks if the player is moving
        if (direction.magnitude >= 0.1f)
        {
            //footstep sound effect
            if (isGrounded)
                AudioManager.instance.Play("Footstep", true);

            //sets animation parameter
            anim.SetBool("IsRunning", true);

            //handles moving the player
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, settings.turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * settings.speed * Time.deltaTime);

            //sprint
            if (Input.GetKey(KeyCode.LeftShift))
            {
                anim.SetBool("IsSprinting", true);

                controller.Move(moveDir.normalized * settings.sprintSpeed * Time.deltaTime);
            }
            else anim.SetBool("IsSprinting", false);
        }
        else anim.SetBool("IsRunning", false);

        //standing still
        if (direction.magnitude <= 0)
            anim.SetBool("IsSprinting", false);

        //caps the dash cooldown
        if (settings.dashCooldown <= 0)
            settings.dashCooldown = 0;

        //makes the dash cooldown go down
        if (pickup.dashAcquired)
            settings.dashCooldown -= Time.deltaTime;

        //Dash Ability
        if (Input.GetKeyDown(KeyCode.X) && settings.dashCooldown <= 0 && pickup.dashAcquired && direction.magnitude >= 0.1f)
        {
            StartCoroutine(Dash());
        }
    }

    #region Movement Functions
    private void Jump()
    {
        //sets animation parameter
        anim.SetTrigger("Jump");

        //plays jump sound effect
        AudioManager.instance.Play("Jump", false);

        //moves player upwards
        velocity.y = Mathf.Sqrt(settings.jumpHeight * -2 * settings.gravity);

        canDoubleJump = true;
    }

    private void DoubleJump()
    {
        //sets animation parameter
        anim.SetTrigger("DoubleJump");

        //plays jump sound effect
        AudioManager.instance.Play("Jump", false);

        canDoubleJump = false;

        //moves player upwards
        velocity.y = Mathf.Sqrt(settings.doubleJumpHeight * -2 * settings.gravity);
    }

    
    IEnumerator Dash()
    {
        anim.SetTrigger("Dash");

        AudioManager.instance.Play("Dash", false);

        float startTime = Time.time;

        settings.dashCooldown = 2f;

        while (Time.time < startTime + settings.dashTime)
        {
            controller.Move(moveDir * settings.dashSpeed * Time.deltaTime);

            yield return null;
        }
    }

    #endregion
}
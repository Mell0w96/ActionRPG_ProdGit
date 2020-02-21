using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour, Idamageable
{
    #region Variables
    [Header("Speed Settings")]

    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float deceleration;
    [SerializeField]
    private float timeToMaxSpeed;
    [SerializeField]
    private float timeToZero;

    private bool isSprinting = false;
    private bool ableToSprint;
    private float sprintTimer;
    private float sprintCooldownTime = 3;

    [SerializeField]
    private float MaxSpeed;    
    [SerializeField]
    private float sprintMultiplier;


    [Header("Jump Settings")]
    
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float sprintJumpForce;
    [SerializeField]
    public float distanceToGround = 1f;
    [SerializeField]
    private float fallMulti;
    [SerializeField]
    private float airMovementSpeed;
    public bool playerGrounded = true;
    private float currentJumpForce;

    [Header ("Health Settings")]

    [SerializeField]
    private float totalHealth = 100f;
    [SerializeField]
    private float damagedHealth;
    [SerializeField]
    private float currentHealth;
    [SerializeField]
    private float currentStamina;
    [SerializeField]
    private float StartingStamina = 100f;
    [SerializeField]
    private float staminaDecreaseRate;
    [SerializeField]
    private float staminaIncreaseRate;
    private float expendedStamina;
    
    
    [Header ("Other Settings")]

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rb;    
    public LayerMask walkable;

    public Interactables item;
    public bool canPickUp;

    #endregion

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        walkable = 1 << LayerMask.NameToLayer("Ground");
        anim.SetBool("isGrounded", true);
        currentHealth = totalHealth;        
        currentStamina = StartingStamina;
        

        sprintTimer = 0f;
        currentJumpForce = jumpForce;

        acceleration = MaxSpeed / timeToMaxSpeed;
        deceleration = -MaxSpeed / timeToZero;

        currentSpeed = 0;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    { 

        //regulates sptinting. A timer starts when stamina reaches 0. Player can only sprint again once timer is up.
        if (sprintTimer <= 0)
        {
            ableToSprint = true;

        }
        else
        {
            sprintTimer -= Time.fixedDeltaTime;
            ableToSprint = false;
        
        }


        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");



        Vector3 movementVector = new Vector3(x * currentSpeed * Time.fixedDeltaTime, 0, y * currentSpeed * Time.fixedDeltaTime);

        // If any input is selected, then set current speed to acceleration and apply the movement vector to the rigid body velocity
        if (playerGrounded == true)
        {
            anim.SetFloat("PlayerX", x * 0.5f);
            anim.SetFloat("PlayerY", y * 0.5f);

            
            
            anim.SetBool("isGrounded", true);
            if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
            {

                isSprinting = false;
                currentSpeed += acceleration * Time.fixedDeltaTime;
                currentSpeed = Mathf.Min(currentSpeed, MaxSpeed);

                rb.velocity = movementVector;
                //rb.AddRelativeForce(x * currentSpeed * Time.fixedDeltaTime, 0, y * currentSpeed * Time.fixedDeltaTime);

                // if the sprint button is pressed then change speed and animation as well as currentJumpForce and also decrease stamina
                if (Input.GetButton("Sprint") && Input.GetAxis("Vertical") > 0 && ableToSprint)
                {
                    if (currentStamina > 0)
                    {
                        isSprinting = true;
                        expendedStamina = currentStamina - staminaDecreaseRate * Time.fixedDeltaTime;
                        currentSpeed = MaxSpeed * sprintMultiplier;
                        currentJumpForce = sprintJumpForce;
                        anim.SetFloat("PlayerY", y);
                        if (Input.GetButton("Horizontal"))
                        {
                            anim.SetFloat("PlayerX", x);
                        }

                        currentStamina = expendedStamina;
                    }

                }
                else
                {
                    currentJumpForce = jumpForce;
                }



            }

            //if player lets go of an input but is still moving, decelerate
            else if (Input.GetAxis("Vertical") == 0 && currentSpeed > 0 || Input.GetAxis("Horizontal") == 0 && currentSpeed > 0)
            {
                currentSpeed += deceleration * Time.fixedDeltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0f);
            }


        }

       
       
       

        

        // regerate stamina when no sprinting
        if (isSprinting == false && currentStamina < StartingStamina) 
        {

            currentStamina += staminaIncreaseRate * Time.fixedDeltaTime;           
        
        }

        if (currentStamina <= 0) 
        {
            sprintTimer = sprintCooldownTime;
        
        }




        print("SPRINT TIMER" + sprintTimer);
        print("SPRINTING" + isSprinting);
        print("ABLE TO SPRINT" + ableToSprint);

        // Jump when pressing space
        if (playerGrounded == true) 
        {
            if (Input.GetButton("Jump"))
            {
                playerGrounded = false;
                anim.SetBool("isGrounded", false);
                //rb.velocity = Vector3.up * jumpForce;
                rb.AddForce(Vector3.up * currentJumpForce, ForceMode.Impulse);
                
                //currentSpeed = airMovementSpeed;
                if (Input.GetButton("Vertical") || Input.GetButton("Horizontal")) 
                {
                    rb.velocity = movementVector;
                }
                    
                
                
            }

        }

        // quick fall
        if (rb.velocity.y < 0) 
        {
            rb.velocity += Vector3.up * Physics.gravity.y * (fallMulti - 1) * Time.fixedDeltaTime;
        
        }

        // when health reaches 0 restart scene
        if (currentHealth <= 0) 
        {
            Scene thisScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(thisScene.name);

        }

        print("GROUNDED" + playerGrounded);

    }

   // take damage when enemy hits player
    public void TakeDamage(float damageAmmount) 
    {

        
        damagedHealth = currentHealth - damageAmmount;
        currentHealth = damagedHealth;
        print("CURRENT HEALTH" + currentHealth);

    }

    #region InteractableBehavior



    #endregion



}

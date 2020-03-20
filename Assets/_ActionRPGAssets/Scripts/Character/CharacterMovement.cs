using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour, Idamageable
{
    #region Variables
    [Header("Movement Settings")]

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

    Vector3 forward;

    Transform cameraTransform;


    [Header("Jump Settings")]

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float sprintJumpMulti;
    [SerializeField]
    private float runJumpMulti;
    [SerializeField]
    public float distanceToGround;
    [SerializeField]
    private float fallMulti;
    [SerializeField]
    private float airMovementSpeed;
    public bool playerGrounded = true;
    [SerializeField]
    private float currentJumpForce;

    [Header("Health Settings")]

    [SerializeField]
    private float totalHealth = 100f;
    [SerializeField]
    private float damagedHealth;
   
    public float currentHealth;
    [SerializeField]
    private float currentStamina;
    [SerializeField]
    private float StartingStamina = 100f;
    [SerializeField]
    private float staminaDecreaseRate;
    [SerializeField]
    private float staminaIncreaseRate;
    private float expendedStamina;

    [Header("Attack Settings")]

    public bool hasAttacked;

    [Header("GroundCheck Settings")]

    
    public float maxSlopeAngle = 120f;
    private float currentGroundAngle;
    RaycastHit hit;


    [Header ("Other Settings")]

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rb;    
    public LayerMask walkable;
    
    public bool canPickUp;
    //Camera camera;
    public LayerMask interactions;
    public float SearchRadius;

    public Transform projectilePoint;
    public GameObject windSlashPrefab;


    #endregion


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        walkable = 1 << LayerMask.NameToLayer("Ground");
        interactions = 1 << LayerMask.NameToLayer("Interactions");
        anim.SetBool("isGrounded", true);
        currentHealth = totalHealth;        
        currentStamina = StartingStamina;
        cameraTransform = Camera.main.transform;
        

       // camera = Camera.main;
        

        sprintTimer = 0f;
        currentJumpForce = jumpForce;

        acceleration = MaxSpeed / timeToMaxSpeed;
        deceleration = -MaxSpeed / timeToZero;

        currentSpeed = 0;

       
        
    }

    void Update()
    {

        #region GroundCheck
       
        // draw raycast going down       
        Debug.DrawRay(transform.position, -Vector3.up, Color.red, distanceToGround);

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, distanceToGround, walkable))
        {
            //print("HIT SOMETHING");
            playerGrounded = true;
        }
        else
        {
            playerGrounded = false;
        }
        #endregion       

        #region InteractableBehavior       

        if (Input.GetKey(KeyCode.E))
            {
             Debug.Log("ITEM ADDED");
             CheckForInteractables(SearchRadius, interactions);
            }


        #endregion

        Debug.DrawLine(transform.position, transform.position + forward * 10, Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * 10, Color.green);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalculatePlayerForward();
        CalculateSlopeAngle();
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

       // rb.rotation = Quaternion.Euler(rb.rotation.eulerAngles + camera.transform.up);        

        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");


        Vector3 movementVector = this.transform.right * (x * currentSpeed * Time.fixedDeltaTime) + forward * (y * currentSpeed * Time.fixedDeltaTime);
        
       

        // If any input is selected, then set current speed to acceleration and apply the movement vector to the rigid body velocity
        if (playerGrounded == true && currentGroundAngle < maxSlopeAngle)
        {
            anim.SetFloat("PlayerX", x * 0.5f);
            anim.SetFloat("PlayerY", y * 0.5f);

            currentJumpForce = jumpForce;


            anim.SetBool("isGrounded", true);
            // if movement inputs are pressed
            if (Input.GetButton("Vertical") || Input.GetButton("Horizontal"))
            {

                isSprinting = false;
                currentSpeed += acceleration * Time.fixedDeltaTime;
                currentSpeed = Mathf.Min(currentSpeed, MaxSpeed);

                currentJumpForce = jumpForce * runJumpMulti; // when the player is moving normally add a small multipler to the jump force to make the player jump slightly higher

                // set the player's rotation to the camera rotaion when movement inputs are in effect
                rb.rotation = Quaternion.Euler(rb.velocity.x, cameraTransform.eulerAngles.y, rb.velocity.z);
               

                rb.velocity = new Vector3(movementVector.x, rb.velocity.y, movementVector.z);
                //rb.AddRelativeForce(x * currentSpeed * Time.fixedDeltaTime, 0, y * currentSpeed * Time.fixedDeltaTime);

                // if the sprint button is pressed then change speed and animation as well as currentJumpForce and also decrease stamina
                if (Input.GetButton("Sprint") && Input.GetAxis("Vertical") > 0 && ableToSprint)
                {
                    if (currentStamina > 0)
                    {
                        isSprinting = true;
                        expendedStamina = currentStamina - staminaDecreaseRate * Time.fixedDeltaTime;
                        currentSpeed = MaxSpeed * sprintMultiplier; // add the sprint multiplier to the maxspeed to make the player run faster
                        currentJumpForce = jumpForce * sprintJumpMulti; // add the sprintjump multipplier to the jumpforce to jump higher
                        anim.SetFloat("PlayerY", y);
                        if (Input.GetButton("Horizontal"))
                        {
                            anim.SetFloat("PlayerX", x);
                        }

                        currentStamina = expendedStamina;
                    }

                }
               

            }

            //if player lets go of an input but is still moving, decelerate
            else if (Input.GetAxis("Vertical") == 0 && currentSpeed > 0 || Input.GetAxis("Horizontal") == 0 && currentSpeed > 0)
            {
                currentSpeed += deceleration * Time.fixedDeltaTime;
                currentSpeed = Mathf.Max(currentSpeed, 0f);
            }


            if (Input.GetButton("Fire1"))
            {
                Attacking();
               
            }
            else
            {
                anim.SetInteger("AttackValue", 0);
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

      //  print("SPRINT TIMER" + sprintTimer);
       // print("SPRINTING" + isSprinting);
      //  print("ABLE TO SPRINT" + ableToSprint);

        // Jump when pressing space
        if (playerGrounded == true) 
        {
            if (Input.GetButton("Jump"))
            {
                playerGrounded = false;
                anim.SetBool("isGrounded", false);
                //rb.velocity = Vector3.up * jumpForce;
                rb.AddForce(Vector3.up * currentJumpForce * Time.fixedDeltaTime, ForceMode.Impulse);    
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

    }

    public void CalculatePlayerForward()
    {
        if (!playerGrounded)
        {
            forward = this.transform.forward;
            return;
        }
        forward = Vector3.Cross(transform.right, hit.normal);
    }

    public void CalculateSlopeAngle()
    {
        if (!playerGrounded)
        {
            currentGroundAngle = 90f;
            return;
        }

        currentGroundAngle = Vector3.Angle(hit.normal, transform.forward);
    
    }

   // take damage when enemy hits player
    public void TakeDamage(float damageAmmount) 
    {

        
        damagedHealth = currentHealth - damageAmmount;
        currentHealth = damagedHealth;
       // print("CURRENT HEALTH" + currentHealth);

    }

    void CheckForInteractables(float _range, LayerMask _layermask)
    {
        RaycastHit[] hits;

        hits = Physics.SphereCastAll(this.transform.position, _range, Vector3.forward, 0.01f, _layermask);
        Debug.Log(hits.Length);

        for (int _i = 0; _i < hits.Length; _i++)
        {
            Interactable interactComponent = hits[_i].rigidbody.gameObject.GetComponent<Interactable>();

            if (interactComponent != null)
            {
                interactComponent.Interact();
            }
            else
            {
                Debug.LogWarning("NOPE");
            }
        }
    }

    void Attacking()
    {
       // anim.SetTrigger("attacking");
        anim.SetInteger("AttackValue", Random.Range(1,4));
    }

    public void PerformAbility()
    {
        Instantiate(windSlashPrefab, projectilePoint.transform.position, projectilePoint.transform.rotation);
    }
    

 }

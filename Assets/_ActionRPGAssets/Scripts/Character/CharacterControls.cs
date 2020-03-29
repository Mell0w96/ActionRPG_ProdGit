using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterControls : MonoBehaviour, Idamageable
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
    [SerializeField]
    private float sprintCooldownTime = 3;

    [SerializeField]
    private float MaxSpeed;
    [SerializeField]
    private float sprintMultiplier;

    Vector3 forward;

    Transform cameraTransform;

    Vector3 playerVelocity;

    bool canRun;


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

    [SerializeField]
    public float jumpTimer;
    [SerializeField]
    private float JumpCooldown;

    [SerializeField]
    private bool canJump;

    

    [Header("Health Settings")]

   
    public float totalHealth = 100f;
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
    public WeaponBase Weapon;
    
    // public float isWeapon;

    public GameObject RightFist;
    public GameObject LeftFist;


    public float MaxSpecialPower;
    [SerializeField]
    private float currentSpecialPower;
    [SerializeField]
    private float newCurrentSpecialPower;
    public float specialPowerIncreasePerHit;
    public float specialPowerDecreasePerHit;
    public bool PowerCanIncrease;
    public bool AbilityReady;
    [HideInInspector]
    public bool AttackPointActive;


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
        canJump = true;        
        canRun = true;
        PowerCanIncrease = false;
        currentSpecialPower = 0f;      
        

        


        // camera = Camera.main;


        sprintTimer = 0f;
        
        
        
        currentJumpForce = jumpForce;

        acceleration = MaxSpeed / timeToMaxSpeed;
        deceleration = -MaxSpeed / timeToZero;

        currentSpeed = 0;        



    }

    void Update()
    {

        Weapon = gameObject.GetComponentInChildren<WeaponBase>();
        


        if (currentSpecialPower >= MaxSpecialPower)
        {
            AbilityReady = true;
            currentSpecialPower = MaxSpecialPower;
        }

        if (currentSpecialPower <= 0)
        {
            AbilityReady = false;
            currentSpecialPower = 0;
        }

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

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("ITEM ADDED");
            
            anim.SetBool("PickingUp", true);

        }
        else
        {
            anim.SetBool("PickingUp", false);
        }


        if (Weapon != false)
        {
            if (Input.GetButton("DropItem"))
            {
                Weapon.transform.parent = null;
                Weapon.DropWeapon();
            }
        }




        #endregion

        

        Debug.DrawLine(transform.position, transform.position + forward * 10, Color.blue);
        Debug.DrawLine(transform.position, transform.position - Vector3.up * 10, Color.green);

        if (!Weapon)
        {
            anim.SetInteger("PlayerState", 0);
            print("No Weapon");
            RightFist.SetActive(true);
            LeftFist.SetActive(true);
        }
        else 
        {
            anim.SetInteger("PlayerState", Weapon.setPlayerState);
            anim.SetFloat("AttackSpeed", Weapon.setAttackSpeed);
            print("There is Weapon");
            RightFist.SetActive(false);
            LeftFist.SetActive(false);
        }
        
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

        // set the player's rotation to the camera rotaion when movement inputs are in effect
        rb.rotation = Quaternion.Euler(rb.velocity.x, cameraTransform.eulerAngles.y, rb.velocity.z);


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

          
           Invoke("EnableJumping", JumpCooldown);
          


            anim.SetBool("isGrounded", true);
            // if movement inputs are pressed
            if (Input.GetButton("Vertical") && canRun || Input.GetButton("Horizontal") && canRun)
            {

                isSprinting = false;
                currentSpeed += acceleration * Time.fixedDeltaTime;
                currentSpeed = Mathf.Min(currentSpeed, MaxSpeed);

                currentJumpForce = jumpForce * runJumpMulti; // when the player is moving normally add a small multipler to the jump force to make the player jump slightly higher

                
               
                playerVelocity = new Vector3(movementVector.x, rb.velocity.y, movementVector.z);
                rb.velocity = playerVelocity;
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


            if (Input.GetButtonDown("Fire1"))
            {
                Attacking();
                print("PUNCHING");
                
                
            }
            else 
            {
                anim.SetInteger("AttackValue", 0);
                print("AttackValue is 0");
                
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
        if (playerGrounded == true ) 
        {
            if (canJump)
            {
                if (Input.GetButton("Jump"))
                {                                      
                    anim.SetBool("isGrounded", false);
                    //rb.velocity = Vector3.up * jumpForce;
                    rb.AddForce(Vector3.up * currentJumpForce * Time.fixedDeltaTime, ForceMode.Impulse);
                    //canJump = false;
                    playerGrounded = false;

                }
            }
            

        }

        // if the player is in the air, camera rotation can still rotate the player
        if (playerGrounded == false)
        {            
            rb.rotation = Quaternion.Euler(rb.velocity.x, cameraTransform.eulerAngles.y, rb.velocity.z);
            canJump = false;

        }

        if (Input.GetKey(KeyCode.LeftAlt))
        {
            rb.rotation = Quaternion.Euler(rb.velocity.x, rb.transform.eulerAngles.y, rb.velocity.z);
        }
        else
        {
            rb.rotation = Quaternion.Euler(rb.velocity.x, cameraTransform.eulerAngles.y, rb.velocity.z);
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

    public void CalculatePlayerForward() // calculate player forward. If the player is in the air then its forward is simply its transform.forward
    {
        if (!playerGrounded)
        {
            forward = this.transform.forward;
            return;
        }
        forward = Vector3.Cross(transform.right, hit.normal); // calculate cross product between right vector and ray hit point to get new player forward
    }

    public void CalculateSlopeAngle()
    {
        if (!playerGrounded)
        {
            currentGroundAngle = 90f; // base angle is set at 90
            return;
        }

        currentGroundAngle = Vector3.Angle(hit.normal, transform.forward); // calculate the angle between ray hit point and player transform forward vector
    
    }

   // take damage when enemy hits player
    public void TakeDamage(float damageAmmount) 
    {

        
        damagedHealth = currentHealth - damageAmmount;
        currentHealth = damagedHealth;
       // print("CURRENT HEALTH" + currentHealth);

    }

    void CheckForInteractables(float _range, LayerMask _layermask) // check for interactables according to layer
    {
        RaycastHit[] hits;

        hits = Physics.SphereCastAll(this.transform.position, _range, Vector3.forward, 0.01f, _layermask); // store hits as spherecast collision points
        Debug.Log(hits.Length);

        for (int _i = 0; _i < hits.Length; _i++)
        {
            Interactable interactComponent = hits[_i].transform.gameObject.GetComponent<Interactable>(); // grab hits that contain the interactable component

            if (interactComponent != null && canPickUp) // if the component is present and the item has signaled that it can picked up then call the interact funtion that comes from the Interactable class
            {
                interactComponent.Interact();
                canPickUp = false;
            }
            else 
            {
                Debug.LogWarning("NOPE"); // else don't

            }
        }
    }

    public void PickUpAfterAnimation() // Animation Event played at the end of the Pickup animation
    {
        CheckForInteractables(SearchRadius, interactions);
    }

    void Attacking() // Attack Method - Set Attack Value to a random range to play through random attack animations
    {
       // anim.SetTrigger("attacking");
        anim.SetInteger("AttackValue", Random.Range(1,8));
    }

     public void PerformWeaponAbility() // Event called within the attack animations at a specific frame
     {
          if (AbilityReady)
                 {
                     Weapon.PerfromMainAbility();
                     print("BOOM BOOM ");
                 }
     }

    public void EnableJumping()
    {
        canJump = true;
        //Debug.Log("CAN JUMP NOW");
    }

    #region Ability Power Up & Power Down
    //called by the weaponDamage script whenever enemy collides with the trigger
    public void IncreasePower()
    {
        if (Weapon != null && Weapon.setAsSpecial)
        {
            
          newCurrentSpecialPower = currentSpecialPower + specialPowerIncreasePerHit;
          currentSpecialPower = newCurrentSpecialPower;
          Debug.LogWarning("I CAN FEEL THE POWER");
            
            
        }
    }

    public void DecreasePower()
    {
        if (AbilityReady)
        {
            newCurrentSpecialPower = currentSpecialPower - specialPowerDecreasePerHit;
            currentSpecialPower = newCurrentSpecialPower;
        }
    }
    #endregion 

    #region Attack Point In Animation
    public void EnableAttackPoint()
    {
        AttackPointActive = true;
        Weapon.weaponRB.detectCollisions = true;
        PowerCanIncrease = true;
        print("RB ENABLED");
    }
    public void DisableAttackPoint()
    {
        AttackPointActive = false;
        Weapon.weaponRB.detectCollisions = false;
        PowerCanIncrease = false;
        print("RB Disabled");
    }
    #endregion
}

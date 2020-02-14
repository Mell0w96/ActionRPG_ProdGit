using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour, Idamageable
{
    #region Variables
    [Header ("Speed Settings")]

    [SerializeField]
    private float currentSpeed;
    [SerializeField]
    private float acceleration;
    [SerializeField]
    private float timeToMaxSpeed = 1f;
    [SerializeField]
    private float maxSpeed = 6f;
    [SerializeField]
    private float strafeSpeed = 4f;
    [SerializeField]
    private float additionalSpeed = 2f;


    [Header ("Jump Settings")]

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float distanceToGround = 1f;
    bool playerGrounded = true;

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
    private float expendedStamina;
    [SerializeField]
    private float staminaDecreaseRate = 5f;
    
    [Header ("Other Settings")]

    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Rigidbody rb;    
    public LayerMask walkable;

    #endregion

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = this.gameObject.GetComponent<Animator>();
        walkable = 1 << LayerMask.NameToLayer("Ground");
        anim.SetBool("isGrounded", true);
        currentHealth = totalHealth;        
        currentStamina = StartingStamina;
        

        acceleration = maxSpeed / timeToMaxSpeed;
        currentSpeed = 0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        RaycastHit hit;    
        // draw raycast going down       
        Debug.DrawRay(this.transform.position, -Vector3.up, Color.red, distanceToGround);

        if (Physics.Raycast(this.transform.position, -Vector3.up, out hit, distanceToGround, walkable))
        {
            print("HIT SOMETHING");
            playerGrounded = true;
        }
        else
        {
            playerGrounded = false;
        }


     

        

       /* if (isRunning == false)
        {
            currentSpeed = 0;
            if (Input.GetButton("Horizontal") == true || Input.GetButton("Vertical") == true)
            {
                currentSpeed = maxSpeed; 
                isRunning = true;
                anim.SetBool("isRunning", true);
            }
        }

        if (isRunning == true)
        {
            if (horizontal == 0 && vertical == 0)
            {
                currentSpeed = 0;
                isRunning = false;
                anim.SetBool("isRunning", false);
            }
        }
        */

        if (playerGrounded)
        {
            
            anim.SetBool("isGrounded", true);
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetBool("isGrounded", false);                
                playerGrounded = false;
            }
        }

        

        
        


        /*if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = maxSpeed + additionalSpeed;
            expendedStamina = currentStamina - (staminaDecreaseRate * Time.deltaTime);
            currentStamina = expendedStamina;

        }
        else 
        {
            currentSpeed = maxSpeed;
            if (currentStamina < StartingStamina) 
            {
                currentStamina += staminaDecreaseRate * Time.deltaTime;
            
            }
        
        }*/



        if (currentHealth <= 0) 
        {
            Scene thisScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(thisScene.name);

        }

        print(playerGrounded);

    }

   
    public void TakeDamage(float damageAmmount) 
    {

        
        damagedHealth = currentHealth - damageAmmount;
        currentHealth = damagedHealth;
        print("CURRENT HEALTH" + currentHealth);

    }

    

}

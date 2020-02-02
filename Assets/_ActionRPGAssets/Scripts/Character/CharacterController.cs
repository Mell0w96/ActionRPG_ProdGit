﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterController : MonoBehaviour, Idamageable
{
    private float currentSpeed;
    public float Speed;
    public float additionalSpeed;
    public float jumpForce;
    public float distanceToGround;
    bool isRunning = false;
    public Animator anim;
    LayerMask walkable;
    Rigidbody rb;
    public float totalHealth;
    private float damagedHealth;
    [SerializeField] private float currentHealth;

    [SerializeField] private float currentStamina;
    public float StartingStamina;
    private float expendedStamina;
    public float staminaDecreaseRate;
  

   
   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = this.gameObject.GetComponent<Animator>();
        walkable = 1 << LayerMask.NameToLayer("Ground");
        anim.SetBool("isGrounded", true);
        currentHealth = totalHealth;
        currentSpeed = Speed;
        currentStamina = StartingStamina;
     
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

    

        // draw raycast going down
        Debug.Log(Physics.Raycast(this.transform.position, -Vector3.up, distanceToGround, walkable));
        Debug.DrawRay(this.transform.position, -Vector3.up, Color.red, distanceToGround);


        Vector3 playerMoveVector = new Vector3(horizontal, 0, vertical).normalized * currentSpeed * Time.deltaTime;
        
        transform.Translate(playerMoveVector, Space.Self);

        if (isRunning == false)
        {
            if (Input.GetButton("Horizontal") == true || Input.GetButton("Vertical") == true)
            {             
                isRunning = true;
                anim.SetBool("isRunning", true);
            }
        }

        if (isRunning == true)
        {
            if (horizontal == 0 && vertical == 0)
            {
                isRunning = false;
                anim.SetBool("isRunning", false);
            }
        }


        if (!NotGrounded())
        {
           anim.SetBool("isGrounded", true);
            if (Input.GetButton("Jump"))
            {
                anim.SetBool("isGrounded", false);
                rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
               
            }
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentSpeed = Speed + additionalSpeed;
            expendedStamina = currentStamina - (staminaDecreaseRate * Time.deltaTime);
            currentStamina = expendedStamina;

        }
        else 
        {
            currentSpeed = Speed;
            if (currentStamina < StartingStamina) 
            {
                currentStamina += staminaDecreaseRate * Time.deltaTime;
            
            }
        
        }


        if (currentHealth <= 0) 
        {
            Scene thisScene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(thisScene.name);

        }

    }

    bool NotGrounded()
    {
      return Physics.Raycast(this.transform.position, -Vector3.up, distanceToGround, walkable);
    }

    public void TakeDamage(float damageAmmount) 
    {

        
        damagedHealth = currentHealth - damageAmmount;
        currentHealth = damagedHealth;
        print("CURRENT HEALTH" + currentHealth);

    }

    

}
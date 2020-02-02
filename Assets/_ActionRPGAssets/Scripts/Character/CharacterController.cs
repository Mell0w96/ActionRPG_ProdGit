using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour , Idamageable
{
    public float Speed;
    public float jumpForce;
    public float distanceToGround;
    bool isRunning = false;
    public Animator anim;    
    LayerMask walkable;
    Rigidbody rb;
    public float totalHealth;
    private float damagedHealth;
    [SerializeField] private float currentHealth;
   

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = this.gameObject.GetComponent<Animator>();
        walkable = 1 << LayerMask.NameToLayer("Ground");
        anim.SetBool("isGrounded", true);
        currentHealth = totalHealth;
     
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        // draw raycast going down
        Debug.Log(Physics.Raycast(this.transform.position, -Vector3.up, distanceToGround, walkable));
        Debug.DrawRay(this.transform.position, -Vector3.up, Color.red, distanceToGround);


        Vector3 playerMoveVector = new Vector3(horizontal, 0, vertical).normalized * Speed * Time.deltaTime;
        
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

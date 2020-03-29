using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{


    Rigidbody rigidbodyComponent;

    [HideInInspector]
    //tells the projectile which direction to move
    public Vector3 MoveDirection = Vector3.forward;
    private float TimeActive = 7f;
    private float currentActiveTime;

    [Range(0,50)]
    public float MoveSpeedPerSecond;

    // Start is called before the first frame update
    void Start()
    {
        rigidbodyComponent = this.gameObject.GetComponent<Rigidbody>();
        MoveDirection = this.transform.forward;
        currentActiveTime = 0;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //transform.Translate(Vector3.forward * MoveSpeedPerSecond * Time.deltaTime);

         moveForward();

        // after 7 seconds destroy the game object
        if (currentActiveTime >= TimeActive)
        {
            Destroy(this.gameObject);
        }
        else
        {
            currentActiveTime += Time.deltaTime;
        }
    }


    void moveForward(){
        if(this.transform.forward != MoveDirection){
           // this.transform.Rotate()
           //this is where we will perform a rotation to make sure that the wind is facing the correct direction
        }

        if(MoveSpeedPerSecond >0 && MoveDirection!=Vector3.zero){
            rigidbodyComponent.position += (MoveDirection * MoveSpeedPerSecond) * Time.deltaTime; 
        }
    }

}

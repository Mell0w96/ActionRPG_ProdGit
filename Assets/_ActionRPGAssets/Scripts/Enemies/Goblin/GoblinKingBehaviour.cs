using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinKingBehaviour : EnemyBase
{
    // OnTheLookOut / Scanning Variables    
    Vision vision;
    public GameObject visionCone;    
    public float speedMultiplier;
    private float stopSpeed = 0f;    
    private float currentRateUntilScanning;
    public float startRateUntilScanning;
    public float _totalScanTime;
    private float totalScanTime;    

    // Chasing Variables
    public float meleeRange;
    public float minRangedRange;
    public float maxRangedRange;
    float distanceFromPlayer;    

    // Charging Variables
    public float rageSpeedMultiplier;
    public float _rageTime;
    private float rageTime;

    // Attacking Variables
    private float TimeTillNextAttackRate;
    public float _attackRate;

    // ThrowingSpear Variables
    public GameObject spearPref;
    public Transform ThrowingPosition;
    private float throwingRate;
    public float _throwingRate;

    // Goblin FSM instance
    private GoblinState _currentState;


    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        vision = visionCone.GetComponent<Vision>();
        currentRateUntilScanning = startRateUntilScanning;
        totalScanTime = _totalScanTime;
    }

    // Update is called once per frame
    public override void Update()
    {
        // calculate the distance between player and enemy
        distanceFromPlayer = Vector3.Distance(player.transform.position, transform.position);

        // FSM
        switch (_currentState) {
            // roaming state
            case GoblinState.OntheLookOut:
                {                       
                    currentRateUntilScanning -= Time.deltaTime; // start countdown for the scanning state
                    agent.speed = movementSpeed; // set navmesh agent speed to pubblic variable _movementSpeed
                    //print("ONTHELOOKOUT");
                    //print("currentRateUntilScanning" + currentRateUntilScanning );
                    
                    anim.SetBool("isScanning", false); // make sure scanning animation doesn't play accidentally
                    if (!coroutineIsWorking)
                    {
                        StartCoroutine(ChangeDirection()); // check if coroutine is working 
                    }

                    if (currentRateUntilScanning <= 0)
                    {
                        _currentState = GoblinState.Scanning; // if scanning timer reaches 0, switch to the scanning state
                        totalScanTime = _totalScanTime;       // set scan time to public _totalScanTime

                    }
                    else 
                    {
                        currentRateUntilScanning -= Time.deltaTime; // else keep counting down until 0
                    
                    }

                    if (vision.isInVision == true) // if player comes into vision cone trigger, switch to chasing state
                    {
                       // print("Chasing");
                        _currentState = GoblinState.Chasing;
                    }

                    break;
                }

                // scanning state
            case GoblinState.Scanning: 
                {

                    //print("SCANNING NOW");
                    anim.SetBool("isScanning", true); // set scanning animation bool to true to run animation
                    totalScanTime -= Time.deltaTime; // start  scanning count down at the start of the state                 

                    agent.speed = stopSpeed;          // make sure goblin stops when scanning          

                    //print("totalScanTime" + totalScanTime);

                    if (totalScanTime <= 0) // if timer runs out, return to roaming state
                    {
                        _currentState = GoblinState.OntheLookOut;
                        currentRateUntilScanning = startRateUntilScanning; // reset time until next scan timer

                        anim.SetBool("isScanning", false); // set animation bool to false
                    }

                    if (vision.isInVision == true) // if player comes into vision, switch to chasing state
                    {
                       // print("Chasing");
                        _currentState = GoblinState.Chasing;
                        anim.SetBool("isScanning", false);
                    }

                    break;                
                }

                // chasing state
            case GoblinState.Chasing: 
                {
                    agent.SetDestination(player.transform.position);  // set nav mesh agent's destination to the player's position
                    agent.speed = movementSpeed * speedMultiplier; // make the goblin run faster
                    anim.SetBool("isChasing", true); // set animation bool to run the animation

                    //print("PLAYER FOUND PLAYER FOUND");

                    //if player comes within vision then chase 
                    if (distanceFromPlayer <= meleeRange) // if player comes into melee range, perform melee attack
                    {
                        //attack the player

                        _currentState = GoblinState.Attacking;
                        TimeTillNextAttackRate = 0; // set attack rate timer to 0 so that goblin attacks player right away

                    }
                    else if (distanceFromPlayer > minRangedRange && distanceFromPlayer <= maxRangedRange) // if player comes into spear range, throw spear 
                    {
                      //  print("SPEAR TIME");
                        //throw spear at player
                        _currentState = GoblinState.ThrowingSpear;
                        throwingRate = 0f; // set throwing rate timer to 0 for instant throw

                    }
                   

                    if (vision.isInVision == false) // if player is out of vision, go back to roaming
                    {
                        anim.SetBool("isChasing", false);

                        _currentState = GoblinState.OntheLookOut;
                        currentRateUntilScanning = startRateUntilScanning;
                    } 
                    break;                    
                }
            case GoblinState.Attacking: 
                {
                    agent.speed = 0f;
                    if (TimeTillNextAttackRate > 0) // if attack rate timer is greater than 0, continue counting down
                    {
                        TimeTillNextAttackRate -= Time.deltaTime;                       

                    }
                    else 
                    {
                        TimeTillNextAttackRate = _attackRate; // perform AttackingPlayer function and reset attack rate timeer to public _attackRate
                        AttackingPlayer();                    
                    
                    }

                    if (distanceFromPlayer >= meleeRange) // if player goes out of melee range, continu chasing the player
                    {
                        _currentState = GoblinState.Chasing;
                        anim.SetInteger("AttackValue", 0); // turn off attacking animation 
                    }

                    break;
                
                }
            case GoblinState.ThrowingSpear: 
                {
                    if (throwingRate > 0) // if throwing rate timer is greater than 0, continue decreasing and goblin continues to follow player
                    {
                        throwingRate -= Time.deltaTime;
                        agent.SetDestination(player.transform.position);

                    }
                    else 
                    {
                        throwingRate = _throwingRate; //else throw the spear and reset the throwingrate timer to public _throwingRate
                        ThrowTheSpear();
                        
                    }

                    if (distanceFromPlayer <= meleeRange) // if player comes into melee range while in this state, switch to attack state
                    {
                        _currentState = GoblinState.Attacking;
                        anim.SetInteger("AttackValue", 1);
                    }

                    if (vision.isInVision == false) // if player is out of vision, go back to roaming state
                    {
                        _currentState = GoblinState.OntheLookOut;
                        anim.SetInteger("AttackValue", 0);
                        
                        print("ALRIGHT NO MORE");
                    }

                    break;             
                
                }
            

            

        } 
    
    }

    public void AttackingPlayer()  // play attack animation
    {        
            anim.SetInteger("AttackValue", 1);   
    }

    public void ThrowTheSpear() // play throwing spear animaation 
    {
        anim.SetInteger("AttackValue", 2);
    }

    public void ThrowSpear() // instantiate spear prefab once called by aniamtion event
    {

        Instantiate(spearPref, ThrowingPosition.transform.position, ThrowingPosition.transform.rotation);


    }

    public void WaitingCooldown() // this function is used as an animation event to help transition between throwing/attack animations and chasing animation in colldown periods
    {
        anim.SetInteger("AttackValue", 0);

    }



    public override IEnumerator ChangeDirection()
    {
        anim.SetBool("isChasing", false); // make sure regular walking animation is playing
        return base.ChangeDirection();
        
    } 

 
}

public enum GoblinState
{ 

    // goblin states
    OntheLookOut,
    Scanning,
    Chasing,    
    Attacking,
    ThrowingSpear

}

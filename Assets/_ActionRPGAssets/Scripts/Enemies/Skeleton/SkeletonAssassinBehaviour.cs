using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonAssassinBehaviour : EnemyBase
{
    private SkeletonState _currentState;

    // Scouting and Assist variables
    

    public Material visible;
    public Material invisible;
    public Renderer stateOfVision;
    
    private bool playerInbound;

    private GameObject Ally;

    public float helpAllyRadius;

    
    // Stalking variables
    // public CharacterControls player;
    public float stalkingDistance;
    public float chasingRange;
    public float maximumChasingRange;

    // EngagingPlayer Variables  
    Vector3 teleportDistance;   
    float currentTimer;
    public float dissengageTimer;
    public float attackRate;
    float currentAttackRate;    
    public float appearBehindPlayerDistance;
    public float attackRange;
    public float movementSpeedX;

    //RunningAway variables  
    public float runAwaySpeed;
    float playerTooClose = 2f;
    public float runAwayDistance;




    public override void Start()
    {
        base.Start();
        stateOfVision.material = visible;
        Ally = GameObject.FindGameObjectWithTag("Ally");
        stateOfVision.material = invisible;
    }

    public override void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, player.transform.position);

        if (playerDistance <= stalkingDistance)
        {
            playerInbound = true;
           // Debug.Log("INBOUND PLAYER INBOUND");
        }
        else
        {
            playerInbound = false;
            //Debug.Log("Player has left the chat");
        }
        switch (_currentState) 
        {

            case SkeletonState.Scouting: 
                {
                    agent.speed = movementSpeed;
                    stateOfVision.material = visible;
                    //float DistanceFromAlly = Vector3.Distance(transform.position, Ally.transform.position);

                    /* if (DistanceFromAlly < helpAllyRadius)
                     {
                         { SendEvent(1166984301); }
                     }*/

                    if (!coroutineIsWorking)
                    {
                        StartCoroutine(ChangeDirection());
                    }

                    if (playerInbound == true) 
                    {
                        _currentState = SkeletonState.Stalking;
                         //print("I SEE YOU");                   
                    
                    }


                    break;               
                }
            case SkeletonState.Stalking:
                {
                    stateOfVision.material = invisible;
                    agent.speed = movementSpeed;

                    agent.SetDestination(player.transform.position);

                    // calculate distance from player

                   

                    if (playerDistance < chasingRange)
                    {
                       // print("COME HERE");
                        _currentState = SkeletonState.Engaging;

                        
                    }

                    if (!playerInbound) 
                    {
                        _currentState = SkeletonState.Scouting;
                    
                    }

                    break;
                }
            case SkeletonState.Engaging:
                {
                   
                   // print("ENGAGING");
                    stateOfVision.material = visible;
                    anim.SetInteger("AnimationValue", 1);
                    agent.speed = movementSpeed * movementSpeedX;
                    agent.SetDestination(player.transform.position);
                    
                    
                    if (playerDistance < attackRange)
                    {
                        _currentState = SkeletonState.Attacking;
                        currentAttackRate = 0;
                    }

                    if (playerDistance > maximumChasingRange) 
                    {
                        _currentState = SkeletonState.Scouting;
                    
                    }

                   /* if (dissengageTimer > 0)
                    {
                        dissengageTimer -= Time.deltaTime;
                    }
                    else 
                    {
                        _currentState = SkeletonState.RunningAway;                    
                    }*/


                   
                    break;
                }
            case SkeletonState.Attacking: 
                {
                    agent.speed = 0f;
                    //print("ATTACKING");
                    //print("Attacking" + currentAttackRate);
                    if (currentAttackRate > 0)
                    {

                        currentAttackRate -= Time.deltaTime;
                        //agent.SetDestination(Player.transform.position);

                    }
                    else
                    {
                        currentAttackRate = attackRate;
                        anim.SetInteger("AnimationValue", 2);
                    }

                    if (playerDistance > attackRange) 
                    {
                        _currentState = SkeletonState.Engaging;
                        agent.SetDestination(player.transform.position);

                    }


                        break; 
                }
            case SkeletonState.RunningAway: 
                {
                    print("RUNNING AWAY");
                    agent.speed = runAwaySpeed;
                    stateOfVision.material = invisible;
                    // calculate distance from player
                    float DistanceFromPlayer = Vector3.Distance(transform.position, player.transform.position);
                    // player to enemy vector
                    Vector3 FromToPlayer = transform.position - player.transform.position;
                    // add the previous vector to the position of the thief
                    Vector3 runDirection = transform.position + FromToPlayer;

                    // set destination to the new vector
                    agent.SetDestination(runDirection);


                    if (playerDistance >= runAwayDistance) 
                    {
                        _currentState = SkeletonState.Scouting;
                    
                    }

                    break;
                
                }

             
            
                



        }
        //print("DISSENGAME TIMER" + dissengageTimer);
    }
    public void WaitingSkeletonCooldown() 
    {
        anim.SetInteger("AnimationValue", 1);

    }







    public override IEnumerator ChangeDirection()
    {
        anim.SetInteger("AnimationValue", 0);
        return base.ChangeDirection();
    }
}
public enum SkeletonState
{
    Scouting,
    Stalking,
    Engaging,
    Attacking,
    Assisting,
    RunningAway
}

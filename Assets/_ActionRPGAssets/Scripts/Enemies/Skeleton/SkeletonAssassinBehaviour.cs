using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonAssassinBehaviour : MonoBehaviour
{
    private SkeletonState _currentState;

    // Scouting and Assist variables
    NavMeshAgent agent;
    NavMeshPath path;

    public Material visible;
    public Material invisible;
    public Renderer stateOfVision;
    public float movementSpeed;
    public float FindNewPathTime;
    public CharacterControls Player;
    private Animator anim;
    private bool playerInbound;

    private GameObject Ally;

    public float helpAllyRadius;

    Vector3 Target;

    bool coroutineIsWorking;
    bool pathIsValid;

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








    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        stateOfVision.material = visible;
        agent.speed = movementSpeed;
        anim = GetComponent<Animator>();
        Ally = GameObject.FindGameObjectWithTag("Ally");

        Player = FindObjectOfType<CharacterControls>();
       // player = FindObjectOfType<CharacterControls>();

        
        stateOfVision.material = invisible;
    }

    private void Update()
    {
        float playerDistance = Vector3.Distance(transform.position, Player.transform.position);

        if (playerDistance <= stalkingDistance)
        {
            playerInbound = true;
            Debug.Log("INBOUND PLAYER INBOUND");
        }
        else
        {
            playerInbound = false;
            Debug.Log("Player has left the chat");
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
                         print("I SEE YOU");                   
                    
                    }


                    break;               
                }
            case SkeletonState.Stalking:
                {
                    stateOfVision.material = invisible;
                    agent.speed = movementSpeed;

                    agent.SetDestination(Player.transform.position);

                    // calculate distance from player

                   

                    if (playerDistance < chasingRange)
                    {
                        print("COME HERE");
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
                    print("ENGAGING");
                    stateOfVision.material = visible;
                    anim.SetInteger("AnimationValue", 1);
                    agent.speed = movementSpeed * movementSpeedX;
                    agent.SetDestination(Player.transform.position);
                    
                    
                    if (playerDistance < attackRange)
                    {
                        _currentState = SkeletonState.Attacking;
                        currentAttackRate = 0;
                    }

                    if (playerDistance > maximumChasingRange) 
                    {
                        _currentState = SkeletonState.Scouting;
                    
                    }

                    if (dissengageTimer > 0)
                    {
                        dissengageTimer -= Time.deltaTime;
                    }
                    else 
                    {
                        _currentState = SkeletonState.RunningAway;                    
                    }


                   
                    break;
                }
            case SkeletonState.Attacking: 
                {
                    //print("ATTACKING");
                    //print("Attacking" + currentAttackRate);
                    if (currentAttackRate > 0)
                    {

                        currentAttackRate -= Time.deltaTime;
                        agent.SetDestination(Player.transform.position);

                    }
                    else
                    {
                        currentAttackRate = attackRate;
                        anim.SetInteger("AnimationValue", 2);
                    }

                    if (playerDistance > attackRange) 
                    {
                        _currentState = SkeletonState.Engaging;
                        agent.SetDestination(Player.transform.position);

                    }


                        break; 
                }
            case SkeletonState.RunningAway: 
                {
                    print("RUNNING AWAY");
                    agent.speed = runAwaySpeed;
                    stateOfVision.material = invisible;
                    // calculate distance from player
                    float DistanceFromPlayer = Vector3.Distance(transform.position, Player.transform.position);
                    // player to enemy vector
                    Vector3 FromToPlayer = transform.position - Player.transform.position;
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
    

   
    Vector3 newRandomPosition()
    {
        float x = Random.Range(-20f, 20f);
        float z = Random.Range(-20f, 20f);

        Vector3 position = new Vector3(x, 0, z);
        return position;

    }

    void GetNewPathCoordinates()
    {
        Target = newRandomPosition();

        agent.SetDestination(Target);
    }


    IEnumerator ChangeDirection()
    {
        coroutineIsWorking = true;

        // Get the new path and set it to navmesh agent as destination 
        GetNewPathCoordinates();
        yield return new WaitForSeconds(FindNewPathTime);
        pathIsValid = agent.CalculatePath(Target, path);
        anim.SetInteger("AnimationValue", 0);
        if (!pathIsValid) Debug.Log("Could not Find Path, will recalculate");

        while (!pathIsValid)
        {
            yield return new WaitForSeconds(0.07f);
            GetNewPathCoordinates();
            pathIsValid = agent.CalculatePath(Target, path);
        }


        coroutineIsWorking = false;
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

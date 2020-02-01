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
    public Transform Player;
    private Animator anim;
    private bool playerInbound;

    private GameObject Ally;

    public float helpAllyRadius;

    Vector3 Target;

    bool coroutineIsWorking;
    bool pathIsValid;

    // Stalking variables
    public GameObject player;
    public float minChasingRange;
    public float maxChasingRange;

    // EngagingPlayer Variables  
    Vector3 teleportDistance;
    public float dissengageTimer;
    float currentTimer;
    public float attackRate;
    float currentAttackRate;    
    public float appearBehindPlayerDistance;
    public float attackRange;
    public float movementSpeedX;

    //RunningAway variables  
    public float runAwaySpeed;
    float playerTooClose = 2f;

    
 





    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        stateOfVision.material = visible;
        agent.speed = movementSpeed;
        anim = GetComponent<Animator>();
        Ally = GameObject.FindGameObjectWithTag("Ally");
        
        stateOfVision.material = invisible;
    }

    private void Update()
    {
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

                    agent.SetDestination(player.transform.position);

                    // calculate distance from player

                    float playerDistance = Vector3.Distance(transform.position, player.transform.position);

                    if (playerDistance > minChasingRange && playerDistance < maxChasingRange)
                    {
                        print("COME HERE");
                        _currentState = SkeletonState.Engaging;

                        
                    }

                    break;
                }
            case SkeletonState.Engaging:
                {
                    stateOfVision.material = visible;
                    anim.SetInteger("AnimationValue", 1);
                    agent.speed = movementSpeed * movementSpeedX;
                    agent.SetDestination(player.transform.position);
                    float playerDistance = Vector3.Distance(transform.position, player.transform.position);
                    Vector3 distanceBehindPlayer = new Vector3(1, 0, 1);
                    

                   // if (playerDistance > appearBehindPlayerDistance)
                    //{
                    //    transform.position = player.transform.position - distanceBehindPlayer;
                    //    agent.SetDestination(player.transform.position);
                    //}


                    if (playerDistance < attackRange)
                    {
                        _currentState = SkeletonState.Attacking;
                       

                    }
                   
                    break;
                }
            case SkeletonState.Attacking: 
                {
                    currentAttackRate = attackRate;
                    if (currentAttackRate <= 0)
                    {                        
                        currentAttackRate = attackRate;
                        anim.SetInteger("AnimationValue", 2);
                    }

                    if (currentAttackRate > 0)
                    {
                        anim.SetInteger("AnimationValue", 1);
                        currentAttackRate -= Time.deltaTime;
                        agent.SetDestination(player.transform.position);
                    }
                    break; 
                }
            case SkeletonState.Assisting:
                {
                    break;



                }
            case SkeletonState.RunningAway:
                {
                    break;



                }



        }
    }
    public void WaitingSkeletonCooldown() 
    {
        anim.SetInteger("AnimationValue", 1);

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInbound = true;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerInbound = false;
        }

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

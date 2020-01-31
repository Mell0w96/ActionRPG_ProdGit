using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoblinKingBehaviour : MonoBehaviour
{
    // OnTheLookOut / Scanning Variables
    NavMeshAgent agent;
    NavMeshPath path;
    public float FindNewPathTime;
    Animator anim;
    Vision vision;
    public GameObject visionCone;
    public float _movementSpeed;
    public float speedMultiplier;
    private float stopSpeed = 0f;
    bool coroutineIsWorking;
    bool pathIsValid;
    Vector3 Target;
    public Transform player;
    private float currentRateUntilScanning;
    public float startRateUntilScanning;

    public float _totalScanTime;
    private float totalScanTime;
    

    // Chasing Variables
    public float meleeRange;
    public float minRangedRange;
    public float maxRangedRange;
    float distanceFromPlayer;

    public float _timeUntilRage;
    public float _currentRageTime;

    // Chargin Variables
    public float rageSpeed;

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
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        agent.speed = _movementSpeed;
        anim = GetComponent<Animator>();
        vision = visionCone.GetComponent<Vision>();

        throwingRate = _throwingRate;
        currentRateUntilScanning = startRateUntilScanning;
        totalScanTime = _totalScanTime;
        

    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(player.position, transform.position);

        switch (_currentState) {
            case GoblinState.OntheLookOut:
                {                   
                    currentRateUntilScanning -= Time.deltaTime;
                    agent.speed = _movementSpeed;
                    print("ONTHELOOKOUT");
                    print("currentRateUntilScanning" + currentRateUntilScanning );
                    
                    anim.SetBool("isScanning", false);
                    if (!coroutineIsWorking)
                    {
                        StartCoroutine(ChangeDirection());
                    }

                    if (currentRateUntilScanning <= 0)
                    {
                        _currentState = GoblinState.Scanning;
                        totalScanTime = _totalScanTime;

                    }
                    else 
                    {
                        currentRateUntilScanning -= Time.deltaTime;
                    
                    }



                    if (vision.isInVision == true)
                    {
                        print("Chasing");
                        _currentState = GoblinState.Chasing;
                    }
                    



                    break;
                }
            case GoblinState.Scanning: 
                {

                    print("SCANNING NOW");
                    anim.SetBool("isScanning", true);
                    totalScanTime -= Time.deltaTime;                 

                    agent.speed = stopSpeed;                    

                    print("totalScanTime" + totalScanTime);

                    if (totalScanTime <= 0) 
                    {
                        _currentState = GoblinState.OntheLookOut;
                        currentRateUntilScanning = startRateUntilScanning;

                        anim.SetBool("isScanning", false);
                    }

                    if (vision.isInVision == true)
                    {
                        print("Chasing");
                        _currentState = GoblinState.Chasing;
                        anim.SetBool("isScanning", false);
                    }

                    break;                
                }
            case GoblinState.Chasing: 
                {         

                                                                     

                    agent.SetDestination(player.position);
                    agent.speed = _movementSpeed * speedMultiplier;
                    anim.SetBool("isChasing", true);

                    print("PLAYER FOUND PLAYER FOUND");
                    

                    





                    //if player comes within vision then chase 
                    if (distanceFromPlayer <= meleeRange)
                    {
                        //attack the player

                        _currentState = GoblinState.Attacking;
                        anim.SetInteger("AttackValue", 1);
                        



                    }
                    else if (distanceFromPlayer > minRangedRange && distanceFromPlayer <= maxRangedRange)
                    {
                        //throw spear at player
                       

                    } 

                    if (vision.isInVision == false)
                    {
                        anim.SetBool("isChasing", false);

                        _currentState = GoblinState.OntheLookOut;
                        currentRateUntilScanning = startRateUntilScanning;
                    } 
                    break;                    
                }
            case GoblinState.Attacking: 
                {

                    anim.SetInteger("AttackValue", 1);
                       
                        
                    
                    if (distanceFromPlayer >= meleeRange)
                    {
                        _currentState = GoblinState.Chasing;
                        anim.SetInteger("AttackValue", 0);
                    }

                    break;
                
                }

            

        } 
    
    }

    void SetAttackRate() 
    {
        TimeTillNextAttackRate = 0f;
    
    }

    Vector3 newRandomPosition()
    {
        float x = Random.Range(-20f, 20f);
        float z = Random.Range(-20f, 20f);

        Vector3 position = new Vector3(x, 0, z);
        return position;

    }


    IEnumerator ChangeDirection()
    {
        coroutineIsWorking = true;

        // Get the new path and set it to navmesh agent as destination 
        GetNewPathCoordinates();
        yield return new WaitForSeconds(FindNewPathTime);
        pathIsValid = agent.CalculatePath(Target, path);
        anim.SetBool("isChasing", false);
        if (!pathIsValid) Debug.Log("Could not Find Path, will recalculate");

        while (!pathIsValid)
        {
            yield return new WaitForSeconds(0.07f);
            GetNewPathCoordinates();
            pathIsValid = agent.CalculatePath(Target, path);
        }


        coroutineIsWorking = false;
    }

    void GetNewPathCoordinates()
    {
        Target = newRandomPosition();

        agent.SetDestination(Target);
    }

    public void ThrowSpear()
    {

        Instantiate(spearPref, ThrowingPosition.transform.position, ThrowingPosition.transform.rotation);


    }
}

public enum GoblinState
{ 
    OntheLookOut,
    Scanning,
    Chasing,
    Charging,
    Attacking,
    ThrowingSpear

}

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
    public float movementSpeed;
    bool coroutineIsWorking;
    bool pathIsValid;
    Vector3 Target;
    public Transform player;

    // Chasing Variables
    public float meleeRange;
    public float minRangedRange;
    public float maxRangedRange;
    float distanceFromPlayer;

    // Chargin Variables
    public float rageSpeed;

    // Attacking Variables
    bool isAttacking = true;

    // ThrowingSpear Variables
    public GameObject spearPref;
    public Transform ThrowingPosition;
    private float throwingRate;
    public float startThrowingRate;

    // Goblin FSM instance
    private GoblinState _currentState;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        agent.speed = movementSpeed;
        anim = GetComponent<Animator>();
        vision = visionCone.GetComponent<Vision>();

        throwingRate = startThrowingRate;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_currentState) {
            case GoblinState.OntheLookOut:
                {
                    break;
                }
            case GoblinState.Scanning:
                {
                    break;
                }
            case GoblinState.Chasing: 
                {
                    break;
                }
            case GoblinState.Attacking: 
                {
                    break;                
                }
            case GoblinState.Charging: 
                {
                    break;
                }
            case GoblinState.ThrowingSpear:
                {
                    break; 
                }
                
                

        } 
    
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

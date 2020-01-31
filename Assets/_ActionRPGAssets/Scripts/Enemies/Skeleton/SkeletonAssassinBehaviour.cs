using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SkeletonAssassinBehaviour : MonoBehaviour
{
    private SkeletonState _currentState;

    // Scouting variables
    NavMeshAgent agent;
    NavMeshPath path;

    public Material visible;
    public Material invisible;
    public Renderer stateOfVision;
    public float movementSpeed;
    public float FindNewPathTime;
    public Transform Player;
    private Animator anim;

    private GameObject Ally;

    public float helpAllyRadius;

    Vector3 Target;

    bool coroutineIsWorking;
    bool pathIsValid;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        stateOfVision.material = visible;
        agent.speed = movementSpeed;
        anim = GetComponent<Animator>();
        Ally = GameObject.FindGameObjectWithTag("Ally");
    }

    private void Update()
    {
        switch (_currentState) 
        {
            case SkeletonState.Scouting: 
                {
                    break;
                
                
                
                }
            case SkeletonState.Stalking:
                {
                    break;



                }
            case SkeletonState.Engaging:
                {
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



}
public enum SkeletonState
{
    Scouting,
    Stalking,
    Engaging,
    Assisting,
    RunningAway
}

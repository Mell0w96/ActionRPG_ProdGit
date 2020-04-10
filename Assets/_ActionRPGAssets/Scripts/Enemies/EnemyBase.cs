using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{

    public float FindNewPathTime;
    public NavMeshAgent agent;
    public NavMeshPath path;
    public bool coroutineIsWorking;
    public bool pathIsValid;
    public Vector3 Target;
    public Animator anim;
    public float movementSpeed;
    public CharacterControls player;


    // Start is called before the first frame update
    public virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();
        agent.speed = movementSpeed;
        anim = GetComponent<Animator>();
        player = FindObjectOfType<CharacterControls>();
        NavMeshHit closestHit;

        if (NavMesh.SamplePosition(gameObject.transform.position, out closestHit, 500f, 1))
            gameObject.transform.position = closestHit.position;
        else
            Debug.LogError("Could not find position on NavMesh!");
    }

    // Update is called once per frame
    public virtual void Update()
    {
        
    }

    public virtual IEnumerator ChangeDirection()
    {
        coroutineIsWorking = true;

        // Get the new path and set it to navmesh agent as destination 
        GetNewPathCoordinates();
        yield return new WaitForSeconds(FindNewPathTime); // time to wait before finding new path
        pathIsValid = agent.CalculatePath(Target, path); // set path to direction going to target
        //anim.SetBool("isChasing", false); // make sure regular walking animation is playing
                                          // if (!pathIsValid) Debug.Log("Could not Find Path, will recalculate");

        while (!pathIsValid) // if path is not valid, reepeat the process
        {
            yield return new WaitForSeconds(0.07f);
            GetNewPathCoordinates();
            pathIsValid = agent.CalculatePath(Target, path);
        }


        coroutineIsWorking = false;
    }

    public void GetNewPathCoordinates() // sets agent's destination to the new random position 
    {
        float GoPosX = this.transform.position.x;
        float GoPosZ = this.transform.position.z;

        float x = GoPosX + Random.Range(-20f, 20f);
        float z = GoPosZ + Random.Range(-20f, 20f);

        Vector3 position = new Vector3(x, this.transform.position.y, z);
        Target = position;

        agent.SetDestination(Target);

        //print("position" + position + gameObject.name);
    }

}

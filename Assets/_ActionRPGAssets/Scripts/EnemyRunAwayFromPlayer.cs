using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRunAwayFromPlayer : MonoBehaviour
{
    float followSpeed = 1f;
    Transform target;

    // Start is called before the first frame update
    // Enemy locates the player
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    // Enemy follows the player
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, -followSpeed * Time.deltaTime);
    }
}

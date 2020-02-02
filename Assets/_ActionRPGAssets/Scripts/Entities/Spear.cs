using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spear : MonoBehaviour
{
    public float spearSpeed;
    private float spearLifeTime = 20f;
    private float timer = 0;

    private GameObject player;

    private void Start()
    {
        if(player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * spearSpeed * Time.deltaTime);

        if (timer >= spearLifeTime)
        {
            Destroy(gameObject);
            timer = spearLifeTime;
        }
        else {
            timer += Time.deltaTime;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") 
        {
            Destroy(this.gameObject);
        
        }
    }
}

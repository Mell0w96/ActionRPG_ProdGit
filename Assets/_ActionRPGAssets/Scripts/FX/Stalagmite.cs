﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalagmite : FXMesh
{

    public float growthSpeedInSeconds;

    public float deathTimeInSeconds;

    Transform myTransform;

    public float scale = 1;

    float timeActive;

    public GameObject DestroyEffect;

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        myTransform = this.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        timeActive += Time.deltaTime;
        
        myTransform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one * scale, timeActive / growthSpeedInSeconds);
        

        if(timeActive>= deathTimeInSeconds)
        {
            Die();
        }
    }

    public void Die()
    {
        if(DestroyEffect != null){
            GameObject.Instantiate(DestroyEffect,myTransform.position + Vector3.up * Random.Range(0,1),Quaternion.identity);
        }
        Destroy(this.gameObject,0f);
    }
}

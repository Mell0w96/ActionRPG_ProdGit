using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestoy : MonoBehaviour
{


    public float lifespan;
    float TimeSinceBirth = 0;

    // Update is called once per frame
    void Update()
    {
        TimeSinceBirth += Time.deltaTime;
        if(TimeSinceBirth>= lifespan)
        {
            Destroy(this.gameObject, 0f);
        }
    }
}

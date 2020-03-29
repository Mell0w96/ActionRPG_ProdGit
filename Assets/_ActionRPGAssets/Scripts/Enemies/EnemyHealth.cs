using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, Idamageable
{
    private float damagedHealth;
    public float OriginalHealth;
    [SerializeField]
    private float currentHealth;

    public GameObject healthDrop;

    void Start()
    {
        currentHealth = OriginalHealth;
    }

    public void TakeDamage(float damageAmmount)
    {
        damagedHealth = currentHealth - damageAmmount;
        currentHealth = damagedHealth;
        // print("CURRENT HEALTH" + currentHealth);

    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Instantiate(healthDrop, this.transform.position, transform.rotation* Quaternion.Euler(0f,180f,0f) );
            Destroy(this.gameObject);
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour, Idamageable
{
    private float damagedHealth;
    public float OriginalHealth;
    [SerializeField]
    private float currentHealth;
    public UIBars enemyHealthUI;

    public GameObject healthDrop;

    void Start()
    {
        currentHealth = OriginalHealth;
        enemyHealthUI.SetMaxValue(OriginalHealth);
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
            Instantiate(healthDrop, this.transform.position + new Vector3(0f, 0.41f, 0f), transform.rotation* Quaternion.Euler(-90f, 0f, 0f) );
            Destroy(this.gameObject);
            
        }

        enemyHealthUI.SetValue(currentHealth);
    }
}

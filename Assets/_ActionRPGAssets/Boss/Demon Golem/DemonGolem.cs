using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GolemBossStates
{
    WAITING,
    APPROACKING,
    ATTACKING,
}

public enum BossAttackType
{
    LIGHT,
    HEAVY,
    NULL
}

public class DemonGolem : BossBehaviour, Idamageable
{
    Animator animatorComponent;
    [Range(1,50)] public float WalkSpeedPerSecond;
    [Range(10, 200)]
    public float StartingHealth;
    float CurrentHealth;


    

    public float LightAttackDamage;

    public float HeavyAttackDamage;

    BossAttackType AttackType = BossAttackType.NULL;

    List<Idamageable> RecentlyHit;

    GolemBossStates CurrentState = GolemBossStates.WAITING;

    // Start is called before the first frame update
    void Start()
    {
        animatorComponent = this.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BossActive)
        {
            if(CurrentState == GolemBossStates.WAITING)
            {
                if (CheckForPlayer())
                {
                    animatorComponent.SetBool("Walking", true);
                    CurrentState = GolemBossStates.APPROACKING;
                }
            } else if(CurrentState == GolemBossStates.APPROACKING)
            {
                if (!CheckForPlayer())
                {
                    animatorComponent.SetBool("Walking", false);
                    CurrentState = GolemBossStates.WAITING;
                }
            } else if (CurrentState == GolemBossStates.ATTACKING)
            {

            }
        } 
    }

   protected void OnTriggerEnter(Collider other)
    {
        if (BossActive && other.GetComponent<Idamageable>() != null)
        {
            if (AttackType == BossAttackType.NULL)
            {
            }
            else if (AttackType == BossAttackType.LIGHT)
            {
                //do light damage
                DealDamage(LightAttackDamage, other.GetComponent<Idamageable>());
            } else if (AttackType == BossAttackType.HEAVY)
            {
                DealDamage(HeavyAttackDamage,other.GetComponent<Idamageable>());
            }
        }
    }

    bool CheckForPlayer()
    {
        if(playerTarget == null)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void Die()
    {

    }

    public void DealDamage(float damageAmmount,Idamageable target)
    {
        if(RecentlyHit.Contains(target) == false)
        {
            target.TakeDamage(damageAmmount);
            RecentlyHit.Add(target);
            StartCoroutine(RecentHit(target));
        }
    }

    IEnumerator RecentHit(Idamageable removalTarget)
    {
        yield return new WaitForSeconds(0.5f);
        RecentlyHit.Remove(removalTarget);

        yield return null;
    }

    public void TakeDamage(float damageAmmount)
    {
        CurrentHealth -= damageAmmount;

        if(CurrentHealth <= 0)
        {
            Die();
        }
    }
}

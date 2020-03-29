using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


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
    NavMeshAgent navMeshAgentComponent;
    [Range(1,50)] public float WalkSpeed;
    [Range(100, 2000)]
    public float StartingHealth;
    float CurrentHealth;

    [Range(5, 15)]
    public float AttackRange;
    
    [Range(5,10)]
    public float LightAttackDamage;
    [Range(11, 35)]
    public float HeavyAttackDamage;

    [Range(1, 4)]
    public float LightAttackDuration = 2;

    [Range(1, 6)]
    public float HeavyAttackDuration = 3;

    Coroutine PlayerAttack;

    bool AttackComplete = true;


    BossAttackType AttackType = BossAttackType.NULL;

    List<Idamageable> RecentlyHit;

    GolemBossStates CurrentState = GolemBossStates.WAITING;








    // Start is called before the first frame update
    void Start()
    {
        animatorComponent = this.GetComponent<Animator>();
        navMeshAgentComponent = this.GetComponent<NavMeshAgent>();
        CurrentHealth = StartingHealth;

        RecentlyHit = new List<Idamageable>();

    }

    // Update is called once per frame
    void Update()
    {
        if (BossActive)
        {
            if(CurrentState == GolemBossStates.WAITING)
            {

                animatorComponent.SetBool("Walking", false);
                if (CheckForPlayer())
                {
                    
                    if (CheckDistanceToPlayer() <= AttackRange)
                    {
                        CurrentState = GolemBossStates.ATTACKING;
                    }
                    else
                    {
                        animatorComponent.SetBool("Walking", true);
                        CurrentState = GolemBossStates.APPROACKING;
                        navMeshAgentComponent.speed = WalkSpeed;
                    }
                }
            } else if(CurrentState == GolemBossStates.APPROACKING)
            {
                if (!CheckForPlayer())
                {
                    animatorComponent.SetBool("Walking", false);
                    CurrentState = GolemBossStates.WAITING;
                } else
                {                
                    ApproackPlayer();
                    if(CheckDistanceToPlayer() <= AttackRange)
                    {
                        navMeshAgentComponent.SetDestination(this.transform.position);
                        animatorComponent.SetBool("Walking", false);
                        CurrentState = GolemBossStates.ATTACKING;
                       
                    }
                }
            } else if (CurrentState == GolemBossStates.ATTACKING)
            {
                
                if (AttackComplete)
                {
                    AttackComplete = false;
                    PlayerAttack = StartCoroutine(AttackPlayer());
                }
            }
        } 
    }

    
    IEnumerator AttackPlayer()
    {
        if(Random.Range(0,100) <= 50)
        {
            //do light attack
            animatorComponent.SetTrigger("LightAttack");
            AttackType = BossAttackType.LIGHT;
            yield return new WaitForSecondsRealtime(LightAttackDuration);
        }
        else
        {
            //do heavy attack
            animatorComponent.SetTrigger("HeavyAttack");
            AttackType = BossAttackType.HEAVY;
            yield return new WaitForSecondsRealtime(HeavyAttackDuration);
        }

        
           


        CurrentState = GolemBossStates.WAITING;
        AttackType = BossAttackType.NULL;
        AttackComplete = true;

        StopCoroutine(PlayerAttack);
        yield return null;
    }


    void ApproackPlayer()
    {
        navMeshAgentComponent.SetDestination(playerTarget.transform.position);
        animatorComponent.SetFloat("WalkSpeed", navMeshAgentComponent.speed);
    }

   /*private void OnTriggerEnter(Collider other)
    {

        Debug.Log(other + "Entered The TriggerCollider");

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
    }*/

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

    float CheckDistanceToPlayer()
    {
        if (!CheckForPlayer())
        {
            return 0;
        }
        else
        {
            float dist = 0;

            dist = (playerTarget.transform.position - this.transform.position).magnitude;

            return dist;
        }
    }

    void Die()
    {
        Destroy(this.gameObject,2f);
    }

    public void DealDamage(float damageAmmount,Idamageable target)
    {

        Debug.Log("Attacking " + target);

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

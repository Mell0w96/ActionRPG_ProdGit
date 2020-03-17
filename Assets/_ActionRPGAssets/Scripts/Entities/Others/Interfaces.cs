using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IdamageDealer 
{
    void DealDamage(float damageAmmount);
}

public interface Idamageable 
{
    void TakeDamage(float damageAmmount);

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int _health;
    public int health => _health;

    public void IncreaseHealth(int increasedValue)
    {
        _health += increasedValue;
        Debug.Log("Health has increased on " + gameObject.name);
    }

    public void TakeDamage(int DamageValue)
    {
        _health -= DamageValue;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Animator anim = GetComponent<Animator>();

        if (anim)
        {
            //play die animation for all objects that have an animation
        }
    }
}

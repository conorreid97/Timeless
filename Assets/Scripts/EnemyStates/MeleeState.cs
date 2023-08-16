using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeState : IEnemyState
{
    /// <summary>
    /// sets up the attack timer and a cooldown 
    /// sets a bool if the enemy can attack or not
    /// </summary>
    private float attackTimer;
    private float attackCoolDown = 2;
    private bool canAttack = true;

    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    /// <summary>
    /// executes the attack function
    /// if the enemy isnt in melee range switch to rangedstate
    /// if enemy doesnt have a target then switch to idle state
    /// </summary>
    public void Execute()
    {
        Debug.Log("attacking");
        Attack();
        if (!enemy.InMeleeRange)
        {
            enemy.ChangeState(new RangedState());
        }
        else if (enemy.Target == null)
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {
       
    }

    public void OnTriggerEnter(Collider2D other)
    {
        
    }

    /// <summary>
    /// handles the attack function
    /// </summary>
    private void Attack()
    {
        attackTimer += Time.deltaTime;
        // if the attack timer is > attack cooldown 
        //set can attack to true and resets timer
        if (attackTimer >= attackCoolDown)
        {
            canAttack = true;
            attackTimer = 0;
        }
        // sets canattack bool back to false
        // triggers the attack parameter
        if (canAttack)
        {
            canAttack = false;
            
            enemy.MyAnim.SetTrigger("attack");

        }
    }
}

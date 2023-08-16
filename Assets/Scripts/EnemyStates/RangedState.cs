using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedState : IEnemyState {

    private Enemy enemy;

    public void Enter(Enemy enemy)
    {
        this.enemy = enemy;
    }

    /// <summary>
    /// changes to melee state when the enemy gets close enough to the player
    /// if the enemy has a target then move, if not then switch to idle state
    /// </summary>
    public void Execute()
    { 
        if (enemy.InMeleeRange)
        {
            enemy.ChangeState(new MeleeState());
        }
        else if (enemy.Target != null)
        {
            enemy.Move();
        }
        else
        {
            enemy.ChangeState(new IdleState());
        }
    }

    public void Exit()
    {

    }

    /// <summary>
    /// when the enemy hits the edge collider change direction
    /// switch to patrol state
    /// </summary>
    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Edge")
        {
            enemy.ChangeDirection();
            enemy.ChangeState(new PatrolState());
        }
    }

}

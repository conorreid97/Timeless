using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IEnemyState
{
    private Enemy enemy;

    private float idleTimer;

    private float idleDuration;

    /// <summary>
    /// This code executes each time enemy goes into idle state
    /// </summary>
    public void Enter(Enemy enemy)
    {
        // gets random number between 1 and 10 to set as idle duration
        idleDuration = UnityEngine.Random.Range(1, 10);
        this.enemy = enemy;
    }

    /// <summary>
    /// start idling 
    /// if the enemy has a target then go into patrol state
    /// </summary>
    public void Execute()
    {
        Idle();

        if (enemy.Target != null)
        {
            enemy.ChangeState(new PatrolState());
        }
    }

    public void Exit()
    {

    }

    // this checks if the enemy is getting hit but player is not in sight 
    // then the enemy will start running towards the player 
    public void OnTriggerEnter(Collider2D other)
    {
        if (other.tag == "Bullet")
        {
            enemy.Target = Player.Instance.gameObject;
        }
        if (other.tag == "Player")
        {
            enemy.Target = Player.Instance.gameObject;
        }
    }

    /// <summary>
    /// this handles what will happen when the enemy is idling
    /// it sets the speed parameter to 0
    /// then if the idle timer gets higher than the duration start, patrolling 
    /// </summary>
    private void Idle()
    {
        enemy.MyAnim.SetFloat("speed", 0);

        idleTimer += Time.deltaTime;

        if (idleTimer >= idleDuration)
        {
            enemy.ChangeState(new PatrolState());
        }
    }
}

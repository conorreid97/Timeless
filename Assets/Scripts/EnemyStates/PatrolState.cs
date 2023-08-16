using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatrolState : IEnemyState
{
    private Enemy enemy;
    private float patrolTimer;
    private float patrolDuration;

    /// <summary>
    /// Executes when enemy enters patrol state
    /// </summary>
    /// <param name="enemy"></param>
    public void Enter(Enemy enemy)
    {
        // gets random number between 1 and 10 to set as patrol duration
        patrolDuration = UnityEngine.Random.Range(1, 10);
        this.enemy = enemy;
    }

    /// <summary>
    /// executes the patrol and move function
    /// </summary>
    public void Execute()
    {
        Debug.Log("Patrolling");
        Patrol();
        enemy.Move();

        // switches from patrol state to ranged state
        if (enemy.Target != null)
        {
            enemy.ChangeState(new RangedState());
        }
    }

    public void Exit()
    {

    }

    public void OnTriggerEnter(Collider2D other)
    {

        // this checks if the enemy is getting hit but player is not in sight //
        // then the enemy will start running towards the player //
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
    /// handles patrol function
    /// when the patrol timer > duration switch to idle state
    /// </summary>
    private void Patrol()
    {
        patrolTimer += Time.deltaTime;
        Debug.Log("counting down");
        if (patrolTimer >= patrolDuration)
        {
            enemy.ChangeState(new IdleState());
        }
    }
}

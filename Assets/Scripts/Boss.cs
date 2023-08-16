using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Character
{

    /// <summary>
    /// The enemys current state 
    /// changing this will change the enemys behaviour
    /// </summary>
    private IEnemyState currentState;

    /// <summary>
    /// the enemys target
    /// </summary>
    public GameObject Target { get; set; }

    /// <summary>
    /// the enemys melee range, at what range does the enemy need to use the sword
    /// </summary>
    [SerializeField]
    private float meleeRange;

    private Vector2 startPos;

    [SerializeField]
    private Transform leftEdge;

    [SerializeField]
    private Transform rightEdge;

    private Canvas healthCanvas;

    /// <summary>
    /// Indicates if the enemy is in melee range
    /// </summary>
    public bool InMeleeRange
    {
        get
        {
            if (Target != null)
            {
                return Vector2.Distance(transform.position, Target.transform.position) <= meleeRange;
            }
            return false;
        }
    }
    /// <summary>
    /// Indicates if the enemy is dead
    /// </summary>
    public override bool IsDead
    {
        get
        {
            return healthStat.CurrentValue <= 0;
        }
    }

    public override void Start()
    {
        this.startPos = transform.position;

        //Calls the base start
        base.Start();

        //Makes the RemoveTarget function listen to the players Dead event
        Player.Instance.Dead += new DeadEventHandler(RemoveTarget);

        // sets the enemy in idle state
        ChangeState(new IdleState());

        healthCanvas = transform.GetComponentInChildren<Canvas>();

    }
    /// <summary>
    /// Update is called once per frame
    /// </summary>
    void Update()
    {
        if (!IsDead) // if enemy is alive
        {
            if (!TakingDamage) // if we arent taking damage
            {
                // execute the current state, this can make the enemy move or attack etc
                currentState.Execute();
            }

            // makes enemy look at his target
            LookAtTarget();
        }
    }

    /// <summary>
    /// Removes the enemys target
    /// </summary>
    public void RemoveTarget()
    {
        // removes the target 
        Target = null;

        // changes the state to a patrol state
        ChangeState(new PatrolState());
    }

    /// <summary>
    /// Makes the enemy look at the target
    /// </summary>
    private void LookAtTarget()
    {
        // if we have a target
        if (Target != null)
        {
            // calculate the direction
            float xDir = Target.transform.position.x - transform.position.x;
            // if we are turning the wrong direction
            if (xDir < 0 && facingRight || xDir > 0 && !facingRight)
            {
                // look in the right direction
                ChangeDirection();
            }
        }
    }

    /// <summary>
    /// Changes the enemy state
    /// </summary>
    /// <param name="newState"></param>
    public void ChangeState(IEnemyState newState)
    {
        // if we have a current state
        if (currentState != null)
        {
            // call the exit function on the state
            currentState.Exit();
        }
        // sets the current state as the new state
        currentState = newState;
        // calles the enter function on the current state
       // currentState.Enter(this);//////////////////////////////////////////
    }

    /// <summary>
    /// Moves the enemy
    /// </summary>
    public void Move()
    {
        // if the enemy is not attacking
        if (!Attack)
        {
            if ((GetDirection().x > 0 && transform.position.x < rightEdge.position.x) || (GetDirection().x < 0 && transform.position.x > leftEdge.position.x))
            {
                // sets the player move animation speed to 1 
                MyAnim.SetFloat("speed", 1);
                // moves the enemy in the right direction
                transform.Translate(GetDirection() * (movementSpeed * Time.deltaTime));
            }
            else if (currentState is PatrolState)
            {
                ChangeDirection();
            }
        }
    }

    /// <summary>
    /// Gets the current direction
    /// </summary>
    /// <returns></returns>
    public Vector3 GetDirection()
    {
        return facingRight ? Vector2.right : Vector2.left;
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        currentState.OnTriggerEnter(other);
        EdgeCollider2D damagearea = GameObject.Find("Player").GetComponent<Player>().SwordCollider;
        if (other == damagearea)
        {
            StartCoroutine(TakeDamage());
        }
        if (other.tag == "Bullet")
        {
            StartCoroutine(TakeDamage());
        }
    }

    public override IEnumerator TakeDamage()
    {
        if (!healthCanvas.isActiveAndEnabled)
        {
            healthCanvas.enabled = true;
        }
        healthStat.CurrentValue -= 10;
        Debug.Log("enemy loosing health");
        if (!IsDead)
        {
            MyAnim.SetTrigger("damage");
        }
        else
        {
            // this spawns a coin when the enemy respawns
            GameObject coin = (GameObject)Instantiate(GameManager.Instance.CoinPrefab, new Vector3(transform.position.x, transform.position.y + 2), Quaternion.identity);
            // ignores collision between coin and enemy
            Physics2D.IgnoreCollision(coin.GetComponent<Collider2D>(), GetComponent<Collider2D>());
            MyAnim.SetTrigger("die");
            yield return null;
        }
    }

    // Resets things for when the enemy dies
    public override void Death()
    {
        MyAnim.ResetTrigger("die");
        MyAnim.SetTrigger("idle");
        healthStat.CurrentValue = healthStat.MaxVal;
        Destroy(gameObject);
        healthCanvas.enabled = false;

    }

    public override void ChangeDirection()
    {
        // Makes  a reference to the enemys canvas
        var v = transform.Find("EnemyHealthBarCanvas");
        if (v != null)
        {
            Debug.Log("found " + transform.ToString());
            Transform tmp = transform.Find("EnemyHealthBarCanvas").transform;

            // Stores the position, so that we know where to move it after we have flipped the enemy
            Vector3 pos = tmp.position;

            // Removes the canvas from the enemy, so that the health bar doesn't flip with it
            tmp.SetParent(null);

            // Changes the enemys direction
            base.ChangeDirection();

            // Puts the health bar back on the enemy
            tmp.SetParent(transform);

            // Puts the health bar back in the correct position
            tmp.position = pos;
        }
    }
}


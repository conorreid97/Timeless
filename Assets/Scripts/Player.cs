using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// an event to let enemy know that the player is dead
public delegate void DeadEventHandler();

// serialized field lets you change the value in the inspector
public class Player : Character {

    public Transform bloodEffect;
    public Transform coinEffect;
    public Transform heartEffect;
    public Transform grenadeEffect;
    public Transform portalgunEffect;

    private static Player instance;
    public static Player Instance
    {
        get
        {
            if (instance == null)
            {
                instance = GameObject.FindObjectOfType<Player>();
            }
            return instance;
        }
    }
    

    public bool isDying = false;
    public float timeToDeath = 0;

    public event DeadEventHandler Dead;

    [SerializeField]
    private Transform gunfirePos;

    [SerializeField]
    private Transform bulletPos;

    [SerializeField]
    private Transform grenadePos;

    private int numberOfGrenades = 1;

    public int numberOfPortalGun = 0;

    [SerializeField]
    private Transform[] groundPoints;

    [SerializeField]
    private float groundRadius;

    [SerializeField]
    private LayerMask whatIsGround;

    [SerializeField]
    private bool airControl;
 
    [SerializeField]
    public float jumpforce;

    public bool autoFire = false;
    private int autoFireDelayMax = 100;
    private int autoFireDelayCountDown = 0;

    [SerializeField]
    private GameObject gunfirePrefab;

    [SerializeField]
    private GameObject bulletPrefab;

    [SerializeField]
    private GameObject grenadePrefab;

    [SerializeField]
    private float immortalTime;
    private bool immortal = false;
    private SpriteRenderer spriteRenderer;
    public float speedforce = 0.1f;
    public Animation animation;

    //checkpoint
    public GameObject currentCheckpoint;

    private Vector2 startPos;

    public Rigidbody2D MyRigidBody { get; set; }
    public bool Jump { get; set; }
    public bool Slide { get; set; }
    public bool OnGround { get; set; }

    // checks if player is dead
    public override bool IsDead
    {
        get
        {
            if (healthStat.CurrentValue <= 0)
            {
                OnDead();
            }

            return healthStat.CurrentValue <= 0;
        }
    }

    public bool IsFalling
    {
        get
        {
            return MyRigidBody.velocity.y < 0;
        }
    }

    public override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //startPos = transform.position;
        //HeartControl.health -= 1;
        MyRigidBody = GetComponent<Rigidbody2D>();
        healthStat.Initialize();
        SwordCollider.enabled = false;
    }

    void Update()
    {
        // kills and respawns the player when fall off
        if (!TakingDamage && !IsDead)
        {
            if (transform.position.y <= -14f)
            {
                Death();
            }
        }
        HandleInput();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!TakingDamage && !IsDead)
        {
            float horizontal = Input.GetAxis("Horizontal");

            // makes the variable = to the function
            OnGround = IsGrounded();

            if (isDying)
            {
                timeToDeath -= Time.deltaTime;
                if (timeToDeath <= 0)
                {
                    Death();
                    isDying = false;
                }
            }
            else
            {

                HandleMovement(horizontal);
            }

            flip(horizontal);

            HandleLayers();
        }
    }
   
    public void OnDead()
    {
        if (Dead != null)
        {
            Dead();
        }
    }

    private void HandleMovement(float horizontal)
    {
        if (IsFalling)
        {
            // changes it to falling layer
            gameObject.layer = 11;
            MyAnim.SetBool("land", true);
        }

        if (!Attack && !Slide && (OnGround || airControl))
        {
            MyRigidBody.velocity = new Vector2(horizontal * movementSpeed, MyRigidBody.velocity.y);
        }

        if (Jump && MyRigidBody.velocity.y == 0)
        {
            MyRigidBody.AddForce(new Vector2(0, jumpforce));
        }

        MyAnim.SetFloat("speed", Mathf.Abs(horizontal));
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {  
            //attack = true;
            MyAnim.SetTrigger("attack");
            FindObjectOfType<AudioManager>().Play("Melee");

        }
        if (Input.GetKeyDown(KeyCode.W) && !IsFalling)
        {
            //jump = true;
            MyAnim.SetTrigger("jump");
            FindObjectOfType<AudioManager>().Play("Jump");

        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            //slide = true;
            MyAnim.SetTrigger("slide");
            FindObjectOfType<AudioManager>().Play("Slide");
        }
        if (Input.GetKeyDown(KeyCode.Space) && !autoFire)
        {
            //shoot = true;
            MyAnim.SetTrigger("shoot");
            FindObjectOfType<AudioManager>().Play("Shoot");
            GunFire(0);
            Bullet(0);
        }
        if(Input.GetKey(KeyCode.Space) && autoFire)
        {
            if (autoFireDelayCountDown <= 0)
            {               
                MyAnim.SetTrigger("shoot");
                FindObjectOfType<AudioManager>().Play("Shoot");
                GunFire(0);
                Bullet(0);
                autoFireDelayCountDown = autoFireDelayMax;
            }
            autoFireDelayCountDown -= (int)(Time.deltaTime*1000);
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            //shoot = true;
            if (numberOfGrenades > 1)
            {
                MyAnim.SetTrigger("throw");
                FindObjectOfType<AudioManager>().Play("Throw");
                GameManager.Instance.CollectedGrenades--;
            }
        }
    }

    // flips the player depending in what direction and speed it is going
    private void flip(float horiontal)
    {
        if (horiontal > 0 && !facingRight || horiontal < 0 && facingRight)
        {
            ChangeDirection();
        }
    }

    // this checks if the player is on the ground using the ground points at the players feet
    // return true if the player is colliding with something with his feet if not return false
    private bool IsGrounded()
    {
        if (MyRigidBody.velocity.y <= 0)
        {
            foreach (Transform point in groundPoints)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(point.position, groundRadius, whatIsGround);

                for (int i = 0; 1 < colliders.Length; i++)
                {
                    if (colliders[i].gameObject != gameObject)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    // sets up the layers so that there is a layer for in the air and on the ground
    private void HandleLayers()
    {
        if (!OnGround)
        {
            MyAnim.SetLayerWeight(1, 1);
        }
        else
        {
            MyAnim.SetLayerWeight(1, 0);
        }
    }

    // controls which direction the gunfire should face depending on player position
    public void GunFire(int value)
    {
        if (!OnGround && value == 1 || OnGround && value == 0)
        {
            if (facingRight)
            {
                GameObject tmp = (GameObject)Instantiate(gunfirePrefab, gunfirePos.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                tmp.GetComponent<GunFire>().Initialize(Vector2Int.right);
            }
            else
            {
                GameObject tmp = (GameObject)Instantiate(gunfirePrefab, gunfirePos.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                tmp.GetComponent<GunFire>().Initialize(Vector2Int.left);
            }
        }
         
    }

    // controls which direction the bullet should face depending on player position
    public void Bullet(int value)
    {
        if (!OnGround && value == 1 || OnGround && value == 0)
        {
            if (facingRight)
            {
                GameObject tmp = (GameObject)Instantiate(bulletPrefab, bulletPos.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                tmp.GetComponent<Bullet>().Initialize(Vector2Int.right);
            }
            else
            {
                GameObject tmp = (GameObject)Instantiate(bulletPrefab, bulletPos.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                tmp.GetComponent<Bullet>().Initialize(Vector2Int.left);
            }
        }
    }

    // controls which direction the grenade should face depending on player position
    public void Throw(int value)
    {
        if (numberOfGrenades > 0)
        {
            numberOfGrenades--;
                if (!OnGround && value == 1 || OnGround && value == 0)
                {
                    if (facingRight)
                    {
                        GameObject tmp = (GameObject)Instantiate(grenadePrefab, grenadePos.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                        tmp.GetComponent<Grenade>().Initialize(Vector2Int.right);
                    }
                    else
                    {
                        GameObject tmp = (GameObject)Instantiate(grenadePrefab, grenadePos.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                        tmp.GetComponent<Grenade>().Initialize(Vector2Int.left);
                    }
                }   
        } 
    }

    // flashes the player to indicate immortality
    private IEnumerator IndicateImmortal()
    {
        while (immortal)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(.1f);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(.1f);
        }
    }

    // take damage function where the health bar is updated / immortal is triggered on & off / triggers animation
    public override IEnumerator TakeDamage()
    {
        Debug.Log("dmg function entered");
        if (!immortal)
        {
            Debug.Log("not immortal");
            healthStat.CurrentValue -= 10;

            if (!IsDead)
            {
                MyAnim.SetTrigger("damage");
                immortal = true;
                Debug.Log("player taking damage");
                StartCoroutine(IndicateImmortal());
                yield return new WaitForSeconds(immortalTime);

                immortal = false;
            }
            else
            {
                MyAnim.SetLayerWeight(1, 0);
                MyAnim.SetTrigger("die");
            }
        }
    }

    // plays sound & stops player when they die / sets the player back to idle / 
    // resets health bar / takes life off / sets player back to latest checkpoint
    public override void Death()
    {
        FindObjectOfType<AudioManager>().Play("PlayerDeath");
        MyRigidBody.velocity = Vector2.zero;
        MyAnim.SetTrigger("idle");
        healthStat.CurrentValue = healthStat.MaxVal;       
        transform.position = currentCheckpoint.transform.position;
        HeartControl.health -= 1;
        GetComponent<Rigidbody2D>().drag = 0;

    }

    // sets everything up that the player might hit and what will happen when they collide
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Acid")
        {
            FindObjectOfType<AudioManager>().Play("AcidSplash");
            GetComponent<Rigidbody2D>().drag = 20;
            isDying = true;
            timeToDeath = 2.0f;
        }

        if (collision.gameObject.tag == "Spike")
        {
            GetComponent<Rigidbody2D>().drag = 5;
            isDying = true;
            timeToDeath = 0.5f;
            Instantiate(bloodEffect, transform.position, transform.rotation);
        }

        if (collision.gameObject.tag == "Saw")
        {
            GetComponent<Rigidbody2D>().drag = 5;
            isDying = true;
            timeToDeath = 0.5f;
            Instantiate(bloodEffect, transform.position, transform.rotation);
        }

        if (collision.gameObject.tag == "PickUp_Grenade")
        {
            FindObjectOfType<AudioManager>().Play("Pickup");
            numberOfGrenades++;
            GameManager.Instance.CollectedGrenades++;
            Destroy(collision.gameObject);
            Instantiate(grenadeEffect, transform.position, transform.rotation);
        }

        if (collision.gameObject.tag == "Coin")
        {
            Debug.Log("coin");
            FindObjectOfType<AudioManager>().Play("Pickup");
            if (GameManager.Instance != null)
            {
                GameManager.Instance.CollectedCoins++;
            }
            Destroy(collision.gameObject);
            Instantiate(coinEffect, transform.position, transform.rotation);
            
        }

        if (collision.gameObject.tag == "Heart")
        {
            FindObjectOfType<AudioManager>().Play("LifePickup");
            Destroy(collision.gameObject);
            HeartControl.health++;
            Instantiate(heartEffect, transform.position, transform.rotation);
        }

        if (collision.gameObject.tag == "PortalGun")
        {
            numberOfPortalGun++;
            GameManager.Instance.CollectedPortalGun++;
            Instantiate(portalgunEffect, transform.position, transform.rotation);
            collision.gameObject.GetComponent<PortalLink>().LinkedPortal.gameObject.active = true;
          
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "Water")
        {
            Debug.Log("water!!!!!!!");
            FindObjectOfType<AudioManager>().Play("WaterSplash");
            GetComponent<Rigidbody2D>().drag = 10;
            isDying = true;
            timeToDeath = 1f;
        }

        //this code runs the virtual ontriggerenter from character class
        base.OnTriggerEnter2D(collision);
    }
}

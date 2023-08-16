using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Character : MonoBehaviour {


    [SerializeField]
    protected Stats healthStat;

    [SerializeField]
    public float movementSpeed;

    [SerializeField]
    private EdgeCollider2D swordCollider;

    [SerializeField]
    private List<string> damageSources;

    public abstract bool IsDead { get; }

    protected bool facingRight;

    public bool Attack { get; set; }

    public bool TakingDamage { get; set; }

    public Animator MyAnim { get; private set; }

    public EdgeCollider2D SwordCollider
    {
        get
        {
            return swordCollider;
        }
    }

    // Use this for initialization
    public virtual void Start() {
        facingRight = true;
        MyAnim = GetComponent<Animator>();
        healthStat.Initialize();
    }

    // Update is called once per frame
    void Update() {

    }

    public abstract IEnumerator TakeDamage();

    // this implements the way each character dies
    public abstract void Death();

    // changes the direction that the character is looking in 
    public virtual void ChangeDirection()
    {
        facingRight = !facingRight;
        transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
    }

    // enables the collider for attacking which will cause damage 
    public void MeleeAttack()
    {
        SwordCollider.enabled = true;
    }

    //starts the take damage coroutine when a damage source triggers the collider
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (damageSources.Contains(other.tag))
        {
            StartCoroutine(TakeDamage());
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

// this automatically puts a rigidbody2d on the object you attach the script to
[RequireComponent(typeof(Rigidbody2D))]
public class Grenade : MonoBehaviour
{
    public float delay = 3f;

    float countdown;
    public GameObject explosionEffect;

    bool hasExploded = false;

    public Vector3 ExplodePosition { get; private set; }

    [SerializeField]
    private float speed;

    private Rigidbody2D myRigidbody;

    private Vector2 direction;

    [SerializeField]
    private CircleCollider2D damageCollider;

    public CircleCollider2D DamageCollider
    {
        get
        {
            return damageCollider;
        }
    }

    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        countdown = delay;
    }

    // this takes the direction that was set and makes it the gun fires direction
    private void FixedUpdate()
    {
        myRigidbody.velocity = direction * speed;
        countdown -= Time.deltaTime;
        if (countdown <= 0f && !hasExploded)
        {
            Debug.Log("EXPLODE");
            Explode();
            hasExploded = true;
            ExplodePosition = transform.position;
        }
        if (countdown <= 0)
        {
            GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
            transform.position = ExplodePosition;
        }
        if(countdown<= -2f)
        {
            Destroy(gameObject);
        }
    }

    // this lets the player access this script to set the direction the same as the players direction
    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    // this enables the damage collider / starts explosion effect / shakes the camera / makes the grenade invisible
    void Explode ()
    {
        Debug.Log("EXPLODED!!!!!!");
        DamageCollider.enabled = true;
        Instantiate(explosionEffect, transform.position, transform.rotation);
        CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
        transform.GetComponent<SpriteRenderer>().enabled = false;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this automatically puts a rigidbody2d on the object you attach the script to
[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    public Bullet bullet;

    [SerializeField]
    private float speed;

    private Rigidbody2D myRigidbody;

    private Vector2 direction;

    // Use this for initialization
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }

    // this takes the direction that was set and makes it the gun fires direction
    private void FixedUpdate()
    {
        myRigidbody.velocity = direction * speed;
    }

    // this lets the player access this script to set the direction the same as the players direction
    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    private void OnBecameInvisible()
    {
//        Debug.Log("bullet destroy");
        Destroy(gameObject);
    }
}


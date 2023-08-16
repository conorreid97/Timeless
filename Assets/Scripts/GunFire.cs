using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this automatically puts a rigidbody2d on the object you attach the script to
[RequireComponent(typeof(Rigidbody2D))]
public class GunFire : MonoBehaviour {

    [SerializeField]
    private float speed;

    //private float respawnTime = 5;
    private float timetoDestroy;

    private Rigidbody2D myRigidbody;

    private Vector2 direction;

	// Use this for initialization
	void Start () {
        myRigidbody = GetComponent<Rigidbody2D>();
        timetoDestroy = 0.3f;
    }

    // this takes the direction that was set and makes it the gun fires direction
    private void FixedUpdate()
    {
        myRigidbody.velocity = direction * speed;
        timetoDestroy -= Time.deltaTime;
        if (timetoDestroy <= 0)
        {
            DestroyObject();
        }
    }

    // this lets the player access this script to set the direction the same as the players direction
    public void Initialize(Vector2 direction)
    {
        this.direction = direction;
    }

    private void DestroyObject()
    {
        
        Destroy(gameObject);
    }
}

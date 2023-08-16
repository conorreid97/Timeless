using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the platform collision
/// </summary>
public class CollisionTrigger : MonoBehaviour {

    /// <summary>
    /// the platform collider
    /// </summary>
    [SerializeField]
    private BoxCollider2D platformCollider;
    
    /// <summary>
    /// platform trigger
    /// </summary>
    [SerializeField]
    private BoxCollider2D platformTrigger;

	// Use this for initialization
	void Start () {
        // ignores the collision between the two box colliders
        Physics2D.IgnoreCollision(platformCollider, platformTrigger, true);
	}

    /// <summary>
    /// if player enters the trigger ignore the other box collider
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player" || other.gameObject.tag == "Enemy")
        {
            Physics2D.IgnoreCollision(platformCollider, other, true);
            Debug.Log("executing1111");
        }
    }

    /// <summary>
    /// When a trigger collision stops 
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerExit2D(Collider2D other)
    {
        // if the player stop colliding
        if (other.gameObject.tag == "player" || other.gameObject.tag == "Enemy")
        {
            // stop the collision from ignoring the player
            Physics2D.IgnoreCollision(platformCollider, other, false);
            Debug.Log("executing");
        }
    }
}

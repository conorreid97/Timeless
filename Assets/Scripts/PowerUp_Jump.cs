using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Jump : MonoBehaviour {

    public GameObject pickupEffect;
    public GameObject pickupEffectPlayer;
    public float multiplier = 1.4f;
    public float duration = 8f;

    // when the player collides with powerup then start the coroutine
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));

        }
    }

    // coroutine that controls the way the powerup works
    // start effect / change the jumpforce of player / make powerup invisible / change jumpforce back / destroy powerup
    IEnumerator Pickup(Collider2D player)
    {
        Instantiate(pickupEffect, transform.position, transform.rotation);

        var ps = Instantiate(pickupEffectPlayer, player.transform.position, player.transform.rotation);
        ps.transform.SetParent(player.transform);

        Player stats = player.GetComponent<Player>();
        stats.jumpforce *= multiplier;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(duration);

        stats.jumpforce /= multiplier;

        Destroy(gameObject);
    }
}

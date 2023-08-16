using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp_Run : MonoBehaviour
{

    public GameObject pickupEffect;
    public GameObject pickupEffectPlayer;
    public float multiplier = 1.4f;
    public float duration = 8f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Pickup(other));
            
        }
    }

    IEnumerator Pickup(Collider2D player)
    {
        Instantiate(pickupEffect, transform.position, transform.rotation);

        var ps = Instantiate(pickupEffectPlayer, player.transform.position, player.transform.rotation);
        ps.transform.SetParent(player.transform);

        Character stats = player.GetComponent<Character>();
        stats.movementSpeed *= multiplier;

        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        

        yield return new WaitForSeconds(duration);

        stats.movementSpeed /= multiplier;

        Destroy(gameObject);
    }
}

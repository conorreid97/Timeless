using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZCameraShake;

public class Barrel : MonoBehaviour
{

    public GameObject barrelExplosion;

    // if the bullet hits the barrel then start the explosion code
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            StartCoroutine(Explosion(other));

        }
    }

    // this plays the explosion particle effect / shake the camera / play sound from audio manager / destroy the barrel
    IEnumerator Explosion(Collider2D player)
    {
        Instantiate(barrelExplosion, transform.position, transform.rotation);
        CameraShaker.Instance.ShakeOnce(4f, 4f, 0.1f, 1f);
        FindObjectOfType<AudioManager>().Play("Blast");
        yield return null;

        Destroy(gameObject);
    }
}

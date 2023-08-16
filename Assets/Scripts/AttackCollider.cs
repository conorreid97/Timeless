using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackCollider : MonoBehaviour {

    [SerializeField]
    private List<string> targetTag;

    /// <summary>
    /// if the tagged target enters trigger
    /// set the collider to false again
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter2D(Collider2D other)
    {
       if (targetTag.Contains(other.tag))
        {
            GetComponent<Collider2D>().enabled = false;
        }
    }
}

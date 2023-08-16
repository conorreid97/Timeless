using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heart : MonoBehaviour {

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("life+1");
        HeartControl.health += 1;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreCollision : MonoBehaviour {

    [SerializeField]
    private Collider2D other;

    [SerializeField]
    private Collider2D other2;

    [SerializeField]
    private Collider2D other3;

    [SerializeField]
    private Collider2D other4;

    // Use this for initialization
    private void Awake () {
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other, true);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other2, true);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other3, true);
        Physics2D.IgnoreCollision(GetComponent<Collider2D>(), other4, true);
    }

    // Update is called once per frame
    void Update () {
		
	}
}

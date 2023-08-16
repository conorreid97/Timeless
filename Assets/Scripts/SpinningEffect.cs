using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinningEffect : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        if (gameObject.tag == "Heart")
        {
            transform.Rotate(3, 0, 0);
        }
        if (gameObject.tag == "Saw")
        {
            transform.Rotate(0, 0, 7);
        }
        if (gameObject.tag == "Grenade")
        {
            transform.Rotate(0, 0, -7);
        }
    }
}

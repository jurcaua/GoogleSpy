using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotater : MonoBehaviour {

    public Vector3 rotationMag = Vector3.one;
    public float speed = 5f;
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(rotationMag * speed * Time.deltaTime);
	}
}

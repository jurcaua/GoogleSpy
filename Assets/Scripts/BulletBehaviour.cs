using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

	public int speed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * speed / 100;
	}

	void OnTriggerEnter(Collider coll) {
		
		Debug.Log (coll.gameObject.name);
		if (coll.gameObject.tag == "Enemy") {
			coll.gameObject.GetComponent<Position>().Die ();
			Destroy(gameObject);
		}
	}
}

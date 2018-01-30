using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour {

	public int speed;
	
	void Update () {
		transform.position += transform.forward * speed / 100;
	}

	void OnTriggerEnter(Collider coll) {
		
		Debug.Log (coll.gameObject.name);
		if (coll.gameObject.tag == "Enemy" && coll.isTrigger == false) {
			coll.transform.tag = "Untagged";                                // no longer an enemy
			coll.transform.GetChild (0).gameObject.SetActive (false);       // deactivate the dynamic sight
			Destroy (gameObject);                                           // destory
		}
	}
}

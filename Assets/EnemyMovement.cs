using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

	public List<Transform> itinerary = new List<Transform> ();
	public int index;
	public Coroutine routine;
	public bool ascending;

	public List<Position> nearbyPositions = new List<Position> ();
	// Use this for initialization
	void Start () {
		routine = StartCoroutine (FollowItirenary ());
		index = 0;
	}
	
	// Update is called once per frame
	void Update () {

	}

	public IEnumerator FollowItirenary() {
		while (transform.tag == "Enemy") {

			//go to a point
			float distance = Vector3.Distance (transform.position, itinerary[index].position);
			Vector3 direction = (itinerary[index].position - transform.position) / 50;
			for (int i = 0; i < 50; i++) {
				transform.position += direction;
				yield return new WaitForSeconds (0.005f * distance * 2);
			}

			if (index < itinerary.Count - 1 && ascending) {
				index++;
			} else {
				index--;
			}

			if (index == itinerary.Count - 1) {
				ascending = false;
			} else if (index == 0) {
				ascending = true;
			}

			yield return new WaitForSeconds (1f);

		}
	}

	void OnTriggerEnter(Collider c) {
		Debug.Log ("entered");
		Position p;
		if ((p = c.GetComponent<Position>()) != null) {
			nearbyPositions.Add(p);
		}
	}

	void OnTriggerExit(Collider c) {
		Position p;
		if ((p = c.GetComponent<Position>()) != null) {
			nearbyPositions.Remove(p);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	public Position pos;

	public bool isMoving;
	public bool queueable;
	public float speed;

	public Position stoppedMovement;
	public bool stopped;

	public GameObject bullet;

	//public float delayBeforeAction;
	public float delayBetweenActions;

	public List<string> futureActions = new List<string> ();

	// Use this for initialization
	void Start () {
		transform.position = pos.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		foreach (Position p in pos.neighbourPositions) {
			if (p.tag == "Enemy") {
				Debug.DrawLine (transform.position, p.transform.position, Color.blue);
			} else {
				Debug.DrawLine (transform.position, p.transform.position, Color.red);
			}
		}
	}

	public IEnumerator moveTowards(Position p, int delayBeforeAction, string parameter) {
		isMoving = true;
		stoppedMovement = p;

		if (pos.tag == "Enemy") {
			pos.tag = "Untagged";
			foreach (Position _pos in pos.neighbourPositions) {
				_pos.neighbourPositions.Remove (pos);
				_pos.UpdateDirections ();
			}

		}

		float speed = 1f;
		if (parameter == "quickly") {
			speed = 5f;
		} else if (parameter == "slowly") {
			speed = 0.3f;
		}

		yield return new WaitForSeconds (delayBeforeAction);

		float distance = Vector3.Distance (transform.position, p.transform.position);
		Vector3 direction = (p.transform.position - transform.position) / 50;
		for (int i = 0; i < 50; i++) {
			transform.position += direction;
			yield return new WaitForSeconds (0.005f * distance / speed);
		}

		pos = p;
		yield return new WaitForSeconds (delayBetweenActions);

		isMoving = false;
	}

	public IEnumerator Action(Position p, string action, string parameter) {
		yield return new WaitForSeconds (0f);
		if (p != null) {
			if (action == "shoot") {
				Instantiate (bullet, transform.position, Quaternion.identity).transform.LookAt (p.transform);
			}

			if (action == "sneak") {
				float speed = 1f;
				if (parameter == "quickly") {
					speed = 2.5f;
				} else if (parameter == "slowly") {
					speed = 0.3f;
				}
				isMoving = true;

				float distance = Vector3.Distance (transform.position, p.transform.position);
				Vector3 direction = (p.transform.position - transform.position) / 50;
				for (int i = 0; i < 50; i++) {

					if (distance != Vector3.Distance (transform.position, p.transform.position)) {
						direction = (p.transform.position - transform.position) / (50 - i);
					}

					transform.position += direction;
					yield return new WaitForSeconds (0.005f * distance / speed);
				}

				pos = p;
				p.GetComponent<MeshRenderer> ().enabled = false;
				p.UpdateDirections ();

				StopCoroutine(p.GetComponent<EnemyMovement>().routine);
				//p.GetComponent<EnemyMovement> ().routine = null;

				yield return new WaitForSeconds (delayBetweenActions);

				isMoving = false;
			}
		}
	}
}

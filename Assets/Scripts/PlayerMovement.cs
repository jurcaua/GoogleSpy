﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour {

	//public Position pos;
	public NewPosition pos;

	public bool isMoving;
	public bool queueable;
	public float speed;

	public NewPosition stoppedMovement;
	public bool stopped;

	public GameObject bullet;
	public TextToMovement ttm;

	//public float delayBeforeAction;
	public float delayBetweenActions;

	public List<string> futureActions = new List<string> ();
	static public List<NewPosition> lookingAt = new List<NewPosition> ();

	public Animator a;
	public bool canShoot;
	// Use this for initialization
	void Start () {
		transform.position = pos.transform.position;
		pos.playPresent = true;
		StartCoroutine (pos.ShowAvailable (this));
		isMoving = false;
		a = GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {




		//foreach (Position p in pos.neighbourPositions) {
//			if (p.tag == "Enemy") {
//				Debug.DrawLine (transform.position, p.transform.position, Color.blue);
//			} else {
//				Debug.DrawLine (transform.position, p.transform.position, Color.red);
//			}
		//}
	}
	public IEnumerator Move(NewPosition p, int delay, string speed) {
		Debug.Log ("Moving Towards: " + p + " with delay: " + delay + " and speed " + speed);



		isMoving = true;
		pos.playPresent = false;

		stoppedMovement = p;
		if (pos.tag == "Enemy") {
			pos.transform.position = new Vector3 (pos.transform.position.x, -0.5f, pos.transform.position.z);
			pos.tag = "Position";
		}
		float s = getSpeed (speed);
		yield return new WaitForSeconds (delay);
		a.SetBool ("isWalking", true);

		float distance = Vector3.Distance (transform.position, p.transform.position);
		Vector3 direction = (p.transform.position - transform.position) / 100;

		transform.rotation = Quaternion.LookRotation (-direction);
		for (int i = 0; i < 100; i++) {
			transform.position += direction;
			yield return new WaitForSeconds (0.0005f * distance / s);
		}
			
		pos = p;
		a.SetBool ("isWalking", false);
		lookingAt.Clear ();
		yield return new WaitForSeconds (delayBetweenActions);
		pos.playPresent = true;
		isMoving = false;
		StartCoroutine(pos.ShowAvailable (this));
	}

//	public IEnumerator moveTowards(Position p, int delayBeforeAction, string parameter) {
//
//
//		Debug.Log ("Moving Towards: " + p.name + " with delay: " + delayBeforeAction + " and speed " + parameter);
//		isMoving = true;
//		stoppedMovement = p;
//
//		if (pos.tag == "Untagged") {
//			foreach (Position _pos in pos.neighbourPositions) {
//				_pos.neighbourPositions.Remove (pos);
//				_pos.UpdateDirections ();
//			}
//
//		}
//
//		float speed = 1f;
//		if (parameter == "quickly") {
//			speed = 2.5f;
//		} else if (parameter == "slowly") {
//			speed = 0.3f;
//		}
//
//		yield return new WaitForSeconds (delayBeforeAction);
//
//		float distance = Vector3.Distance (transform.position, p.transform.position);
//		Vector3 direction = (p.transform.position - transform.position) / 50;
//		for (int i = 0; i < 50; i++) {
//			transform.position += direction;
//			yield return new WaitForSeconds (0.005f * distance / speed);
//		}
//
//		pos = p;
//		pos.UpdateDirections ();
//
//
//		yield return new WaitForSeconds (delayBetweenActions);
//
//		isMoving = false;
//		StartCoroutine(pos.ShowAvailable (this));
//	}
//

	public IEnumerator Shoot(NewPosition p, int delay, string speed) {
		if (canShoot) {
			yield return new WaitForSeconds (delay);
			Debug.Log ("Shoot: " + p + " with delay: " + delay + " and speed " + speed);
			Transform b = Instantiate (bullet, transform.position, Quaternion.identity).transform;
			b.transform.LookAt (p.transform);
		}
	}

	public IEnumerator Sneak(NewPosition p, int delay, string speed) {
		Debug.Log ("Sneak: " + p + " with delay: " + delay + " and speed " + speed);

		float s = getSpeed (speed);
		pos.playPresent = false;

		yield return new WaitForSeconds (delay);

		a.SetBool ("isWalking", true);
		isMoving = true;
		stoppedMovement = p;
		float distance = Vector3.Distance (transform.position, p.transform.position);
		Vector3 direction = (p.transform.position - transform.position) / 100;
		for (int i = 0; i < 100; i++) {
		
			if (distance != Vector3.Distance (transform.position, p.transform.position)) {
				direction = (p.transform.position - transform.position) / (100 - i);
			}
		
			transform.position += direction;
			yield return new WaitForSeconds (0.0005f * distance / s);
		}


		p.transform.GetChild (0).gameObject.SetActive (false);
		pos = p;


		//p.GetComponent<MeshRenderer> ().enabled = false;

		//p.UpdateDirections ();
		
		//p.tag = "Untagged";
		//StopCoroutine(p.GetComponent<EnemyMovement>().routine);
		//p.GetComponent<EnemyMovement> ().routine = null;
		a.SetBool ("isWalking", false);
		yield return new WaitForSeconds (delayBetweenActions);

		pos.playPresent = true;
		isMoving = false;
		StartCoroutine(pos.ShowAvailable (this));
		
	}

//	public IEnumerator Action(Position p, string action, string parameter) {
////		yield return new WaitForSeconds (0f);
////		if (p != null) {
////
////			if (action == "shoot") {
////				Transform b = Instantiate (bullet, transform.position, Quaternion.identity).transform;
////				b.transform.LookAt (p.transform);
////				//b.GetComponent<BulletBehaviour> ().speed = Mathf.RoundToInt(b.GetComponent<BulletBehaviour> ().speed * speed);
////			}
////
////			if (action == "sneak") {
////				float speed = 1f;
////				if (parameter == "quickly") {
////					speed = 2.5f;
////				} else if (parameter == "slowly") {
////					speed = 0.3f;
////				}
////				isMoving = true;
////
////				float distance = Vector3.Distance (transform.position, p.transform.position);
////				Vector3 direction = (p.transform.position - transform.position) / 50;
////				for (int i = 0; i < 50; i++) {
////
////					if (distance != Vector3.Distance (transform.position, p.transform.position)) {
////						direction = (p.transform.position - transform.position) / (50 - i);
////					}
////
////					transform.position += direction;
////					yield return new WaitForSeconds (0.005f * distance / speed);
////				}
////
////				pos = p;
////				p.GetComponent<MeshRenderer> ().enabled = false;
////				p.UpdateDirections ();
////
////				p.tag = "Untagged";
////				//StopCoroutine(p.GetComponent<EnemyMovement>().routine);
////				//p.GetComponent<EnemyMovement> ().routine = null;
////
//				yield return new WaitForSeconds (delayBetweenActions);
////
////				isMoving = false;
////
////				StartCoroutine(pos.ShowAvailable (this));
////			}
////		}
//	}

	public float getSpeed(string speed) {
		float s = 1f;
		if (speed == "quickly") {
			s = 2f;
		} else if (speed == "slowly") {
			s = 0.3f;
		}
		return s;
	}


	public IEnumerator GameOver(Vector3 enemy) {

		StopCoroutine (ttm.routine);

		yield return new WaitForSeconds (1f);

		Vector3 direction = (enemy - transform.position);

		transform.rotation = Quaternion.LookRotation (-direction);


		yield return new WaitForSeconds (2f);

		SceneManager.LoadScene (SceneManager.GetActiveScene ().buildIndex);
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToMovement : MonoBehaviour {

	public enum direction {right, left, forwards, backwards}

	public GameObject player;
	PlayerMovement pm;
	Coroutine routine;

	// Use this for initialization
	void Start () {
		pm = player.GetComponent<PlayerMovement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.LeftArrow)) {
			Translate ("left", 0, "");
		}

		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			Translate ("right", 0, "");
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			Translate ("forwards", 0, "");
		}

		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			Translate ("backwards", 0, "");
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			//Translate ("wait", 0);
			TranslateEnemy ("shoot", "white", "");
		}

		if (Input.GetKeyDown(KeyCode.LeftAlt)) {
			TranslateEnemy ("sneak", "", "");
		}

		if (!pm.isMoving && pm.futureActions.Count > 0) {
			Translate (pm.futureActions [0], 0, "");
			pm.futureActions.RemoveAt (0);
			//pm.isMoving = true;
		}
	}

	public void Translate(string str, int delayBeforeAction, string parameter) {
		if (str == "wait") {
			//pm.Wait (routine, delayBeforeAction);
			if (routine != null && !pm.stopped) {
				StopCoroutine (routine);
				pm.stopped = true;
				Debug.Log ("Stopping");
			} else {
				Debug.Log ("Can't Stop Right Now");
			}
			return;
		} 

		if (str == "go") {
			if (pm.stoppedMovement != null && pm.stopped) {
				routine = StartCoroutine (pm.moveTowards (pm.stoppedMovement, delayBeforeAction, parameter));
				pm.stopped = false;
				Debug.Log ("Resuming");
			} else {
				Debug.Log ("Already Resumed");
			}
			return;
		}

		if (!pm.isMoving) {
			Position p = pm.pos.isAvailable (str);

			if (p == null) {
				Debug.Log ("I Can't Go This Way !");
			} else if (p.tag == "Enemy") {
				Debug.Log ("Target is an Enemy");
			} else {
				//player.transform.position = p.transform.position;
				//pm.pos = p;
				Debug.Log ("Going to " + p.name + " in " + delayBeforeAction + " seconds");
				routine = StartCoroutine (pm.moveTowards (p, delayBeforeAction, parameter));
			}
		} else if (pm.queueable) {
			Debug.Log ("Wait A Second, I'm Still Moving");
			pm.futureActions.Add (str);
		}

	}

	public void TranslateEnemy(string action, string description , string parameter) {
		if (!pm.isMoving) {
			Position p = pm.pos.isEnemyAvailable (description);
			StartCoroutine (pm.Action (p, action, parameter));
		} else {
			Debug.Log ("Wait A Second, I'm Still Moving");
		}
	}
}

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
			//Translate ("left", 0, "");
			NewTranslate("position", "go to", "left", null, "normal",0);
		}

		if (Input.GetKeyDown(KeyCode.RightArrow)) {
			//Translate ("right", 0, "");
			NewTranslate("position", "go to", "right", null, "normal",0);
		}

		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			//Translate ("forwards", 0, "");
			NewTranslate("position", "go to", "forwards", null, "normal",0);
		}

		if (Input.GetKeyDown(KeyCode.DownArrow)) {
			//Translate ("backwards", 0, "");
			NewTranslate("position", "go to", "backwards", null, "normal",0);
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			//Translate ("wait", 0);
			//TranslateEnemy ("shoot", "white", "");
			//Debug.Log(pm.pos.getEnemy("left", null));
			NewTranslate("enemy", "sneak", null, null, "normal",0);
		}

		if (Input.GetKeyDown(KeyCode.LeftAlt)) {
			//TranslateEnemy ("sneak", null, "");
		}

		if (!pm.isMoving && pm.futureActions.Count > 0) {
			//Translate (pm.futureActions [0], 0, "");
			//pm.futureActions.RemoveAt (0);
			//pm.isMoving = true;
		}
	}

//	public void Translate(string str, int delayBeforeAction, string parameter) {
//		if (str == "wait") {
//			//pm.Wait (routine, delayBeforeAction);
//			if (routine != null && !pm.stopped) {
//				StopCoroutine (routine);
//				pm.stopped = true;
//				Debug.Log ("Stopping");
//			} else {
//				Debug.Log ("Can't Stop Right Now");
//			}
//			return;
//		} 
//
//		if (str == "resume") {
//			if (pm.stoppedMovement != null && pm.stopped) {
//				routine = StartCoroutine (pm.moveTowards (pm.stoppedMovement, delayBeforeAction, parameter));
//				pm.stopped = false;
//				Debug.Log ("Resuming");
//			} else {
//				Debug.Log ("Already Resumed");
//			}
//			return;
//		}
//
//		if (!pm.isMoving) {
//			Position p = pm.pos.isAvailable (str);
//
//			if (p == null) {
//				Debug.Log ("I Can't Go This Way !");
//			} else if (p.tag == "Enemy") {
//				Debug.Log ("Target is an Enemy");
//			} else {
//				//player.transform.position = p.transform.position;
//				//pm.pos = p;
//				Debug.Log ("Going to " + p.name + " in " + delayBeforeAction + " seconds");
//				if (str == "run to") {
//					routine = StartCoroutine (pm.moveTowards (p, delayBeforeAction, "quickly"));
//				} else {
//					routine = StartCoroutine (pm.moveTowards (p, delayBeforeAction, parameter));
//				}
//			}
//		} else if (pm.queueable) {
//			Debug.Log ("Wait A Second, I'm Still Moving");
//			pm.futureActions.Add (str);
//		}
//
//	}
//
//	public void TranslateEnemy(string action, string description , string parameter) {
//		if (!pm.isMoving) {
//			Position p = pm.pos.isEnemyAvailable (description);
//			Debug.Log (p);
//			StartCoroutine (pm.Action (p, action, parameter));
//		} else {
//			Debug.Log ("Wait A Second, I'm Still Moving");
//		}
//	}

	public void NewTranslate (string tag, string action, string direction, string name, string speed, int delay)
	{
		if (action == "wait") {
			return;
		} else if (action == "resume") {
			return;
		}

		if (tag == "position") {
			//movement
			if ((direction == "right" || direction == "left" || direction == "forwards" || direction == "backwards") && name != null) {
				Debug.Log ("GOOD IN");
				NewPosition np = pm.pos.getFixedDirection (direction);
				if (np != null) {
					if (action == "run to") {
						StartCoroutine (pm.Move (np, delay, "quickly"));
					} else {
						StartCoroutine (pm.Move (np, delay, speed));
					}
				}
			} else {
				Debug.Log ("INHERE");
				NewPosition np = pm.pos.getPosition (direction, name);
				if (np != null) {
					if (action == "run to") {
						StartCoroutine (pm.Move (np, delay, "quickly"));
					} else {
						StartCoroutine (pm.Move (np, delay, speed));
					}
				} else {
					//No Position Found
					Debug.Log ("ERROR IN FINDING POSITION");
				}
			
			}
				
				//NewPosition = 
			} else {
				//enemy action
				NewPosition np = pm.pos.getEnemy (direction, name);
				if (np != null) {
					if (action == "shoot") {
						StartCoroutine (pm.Shoot (np, delay, speed));
					} else if (action == "sneak") {
						StartCoroutine (pm.Sneak (np, delay, speed));
					}
				} else {
					//No Position Found
					Debug.Log ("ERROR IN FINDING ENEMY");
				}
			}
		}

}


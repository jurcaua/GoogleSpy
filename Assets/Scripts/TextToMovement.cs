using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextToMovement : MonoBehaviour {

    // the only directions the player can go towards explictly
    public enum direction {right, left, forwards, backwards} 

    // object references
	public GameObject player;
	public Coroutine routine;

    private PlayerMovement pm;

    // Use this for initialization
    void Start () {
		pm = player.GetComponent<PlayerMovement> ();
	}

    /////////////////////////////////////////
    // KEYBOARD CONTROLS FOR TESTING ONLY
    /////////////////////////////////////////
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

	public void NewTranslate (string tag, string action, string direction, string name, string speed, int delay)
	{
		if (action == "wait" && routine != null) { // wait command just pauses the current movement
			StopCoroutine (routine);
			return;
		} else if (action == "resume") { // resume command continues movement from current position
			routine = StartCoroutine (pm.Move (pm.stoppedMovement, delay, speed));
			return;
		}

        // movement command to move in a certain direction or to an object
        if (tag == "position") {
            NewPosition np = pm.pos.getPosition (direction, name);
			if (np != null) {
				if (action == "run to") {
					routine = StartCoroutine (pm.Move (np, delay, "quickly"));
				} else {
					routine = StartCoroutine (pm.Move (np, delay, speed));
				}
			} else { 
				Debug.Log ("ERROR IN FINDING POSITION");
			}
		}
        // enemy command to attack a described enemy in different ways
        else { 
            NewPosition np = pm.pos.getEnemy (direction, name);
			if (np != null) {
				if (action == "shoot") {
					routine = StartCoroutine (pm.Shoot (np, delay, speed));
				} else if (action == "sneak") {
					routine = StartCoroutine (pm.Sneak (np, delay, speed));
				}
			} else { // no position Found
                Debug.Log ("ERROR IN FINDING ENEMY");
			}
		}
	}
}


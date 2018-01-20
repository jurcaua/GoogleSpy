﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NewPosition : MonoBehaviour {

	public string _name;

	public NewPosition Left;
	public NewPosition Right;
	public NewPosition Forwards;
	public NewPosition Backwards;

	public bool playPresent;

	public List<NewPosition> positionsInRange = new List<NewPosition>();
	public List<NewPosition> possiblePositions = new List<NewPosition>();


	void Update() {
		if (playPresent) {
			foreach (NewPosition np in positionsInRange) {
				if (np.tag == "Enemy") {
					Debug.DrawLine (transform.position, np.transform.position, Color.blue);
				} else if (np.tag == "Position") { 
					Debug.DrawLine (transform.position, np.transform.position, Color.red);
				}
			}
		}
	}

	void Start() {
	}

	public float GetX() {
		return transform.position.x;
	}

	public float GetZ() {
		return transform.position.z;
	}

	public string getDirectionTo(NewPosition np) {
		if (np != null) {
			if (GetX () < np.GetX ()) {
				//right heavy
				if (Mathf.Abs (GetZ () - np.GetZ ()) < Mathf.Abs (GetX () - np.GetX ())) {
					//right heavy
					return "right";
				} else if (GetZ () < np.GetZ ()) {
					//top heavy
					return "forwards";
				} else {
					//bottom heavy
					return "backwards";
				}
			} else {
				//left heavy
				if (Mathf.Abs (GetZ () - np.GetZ ()) < Mathf.Abs (GetX () - np.GetX ())) {
					//left heavy
					return "left";
				} else if (GetZ () > np.GetZ ()) {
					//bottom heavy
					return "backwards";
				} else {
					//top heavy
					return "forwards";
				}
			}
		}
		return null;
	}

	void OnTriggerEnter(Collider coll) {
		//positionsInRange.Clear ();
		if (coll.tag == "Enemy" || coll.tag == "Position" && possiblePositions.Contains(coll.GetComponent<NewPosition> ())) {
			positionsInRange.Add (coll.GetComponent<NewPosition> ());
		}
	}

	void OnTriggerExit(Collider coll) {
		if (coll.tag == "Enemy" || coll.tag == "Position" && possiblePositions.Contains(coll.GetComponent<NewPosition> ())) {
			positionsInRange.Remove (coll.GetComponent<NewPosition> ());
		}
	}

//	public Position isAvailable(string str) {
//		return null;
//	}
//
//	public Position isEnemyAvailable(string str) {
//		return null;
//	}

	public NewPosition getFixedDirection(string direction) {
		if (direction == "right") {
			return goRight ();
		} if (direction == "left") {
			return goLeft ();
		} if (direction == "forwards") {
			return goForwards ();
		} else {
			return goBackwards ();
		}
	}

	public NewPosition goRight() {
		return Right;
	}
	public NewPosition goLeft() {
		return Left;
	}
	public NewPosition goForwards() {
		return Forwards;
	}
	public NewPosition goBackwards() {
		return Backwards;
	}

	public NewPosition getEnemy(string direction, string color) {
		foreach (NewPosition np in positionsInRange) {
			if (np.tag == "Enemy") {
				if (direction == null && color == null) {
					return np;
				} else if (direction != null && color == null) {
					if (getDirectionTo (np) == direction) {
						return np;
					}
				} else if (direction == null && color != null) {
					if (np._name == color) {
						return np;
					}
				} else if (getDirectionTo (np) == direction && np._name == color) {
					return np;
				}
			}
		}
		return null;
	}

	public NewPosition getPosition(string direction, string obj) {
		foreach (NewPosition np in positionsInRange) {
			Debug.Log (np._name + " >< " + obj);
			if (np.tag == "Position") {
				if (direction == null && obj == null) {
					return np;
				} else if (direction != null && obj == null) {
					if (getDirectionTo (np) == direction) {
						return np;
					}
				} else if (direction == null && obj != null) {
					Debug.Log (np._name + " " + obj);
					if (np._name == obj) {
						return np;
					}
				} else if (direction.Equals(getDirectionTo (np)) && np._name == obj) {
					return np;
				} 
			}
		}
		return null;
	}
}

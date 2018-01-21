using System.Collections;
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

    public bool victoryPosition = false;

	public List<NewPosition> positionsInRange = new List<NewPosition>();
	public List<NewPosition> possiblePositions = new List<NewPosition>();

	public bool lookedAt;

	public GameObject arrow;
	public List<GameObject> arrows = new List<GameObject> ();

	void Update() {
		if (playPresent) {

            if (victoryPosition) {
                // winning condition
            }

			foreach (NewPosition np in positionsInRange) {

				if (np.tag == "Enemy") {
					Debug.DrawLine (transform.position, np.transform.position, Color.blue);
				} else if (np.tag == "Position") { 
					//np.transform.GetChild (0).gameObject.SetActive (true);
					Debug.DrawLine (transform.position, np.transform.position, Color.red);
					if (!PlayerMovement.lookingAt.Contains(np)) {
						PlayerMovement.lookingAt.Add (np);
					}
				} else {
					//positionsInRange.Remove (np);
				}
			}
		}
		if (transform.tag == "Position" && transform.childCount > 0) {
			if (PlayerMovement.lookingAt.Contains (this)) {
				transform.GetChild (0).gameObject.SetActive (true);
			} else {
				transform.GetChild (0).gameObject.SetActive (false);
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
		//if// ((transform.tag == "Enemy" && (coll.tag == "Enemy" || coll.tag == "Position")) || (possiblePositions.Contains(coll.GetComponent<NewPosition> ())  && (coll.tag == "Enemy" || coll.tag == "Position"))) {
		if ((possiblePositions.Contains(coll.GetComponent<NewPosition> ())  && (coll.tag == "Enemy" || coll.tag == "Position"))) {
			positionsInRange.Add (coll.GetComponent<NewPosition> ());
		}
	}

	void OnTriggerExit(Collider coll) {
	//	if ( (transform.tag == "Enemy" && (coll.tag == "Enemy" || coll.tag == "Position")) || (possiblePositions.Contains(coll.GetComponent<NewPosition> ())  && (coll.tag == "Enemy" || coll.tag == "Position"))) {
		if ((possiblePositions.Contains(coll.GetComponent<NewPosition> ())  && (coll.tag == "Enemy" || coll.tag == "Position"))) {
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

//		if (color == null) {
//			if (getFixedDirection (direction) != null) {
//				return getFixedDirection (direction);
//			}
//		}

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

	public IEnumerator ShowAvailable(PlayerMovement pm) {


		yield return new WaitForSeconds (0.1f);
		foreach (NewPosition p in positionsInRange) {
			GameObject a = Instantiate (arrow, (transform.position + (p.transform.position - transform.position).normalized * 1.5f), Quaternion.identity);
			if (p._name != null) {
				a.GetComponentInChildren<TextMeshProUGUI> ().text = p._name;
			} else {
				a.GetComponentInChildren<TextMeshProUGUI> ().text = getDirectionTo(p);
			}
			arrows.Add (a);
			a.transform.LookAt (p.transform);
		}

		pm.isMoving = false;

		while (!pm.isMoving) {
			int i = 0;
			foreach (NewPosition p in positionsInRange) {
				arrows[i].transform.position = (transform.position + (p.transform.position - transform.position).normalized * 1.5f);
				//arrows [i].transform.position -= new Vector3 (0, arrows [i].transform.position.y, 0);
				arrows[i].transform.LookAt (p.transform);
				i++;
			}
			yield return new WaitForSeconds (0.01f);
		}

		foreach (GameObject a in arrows) {
			Destroy (a);
		}
		arrows.Clear();
	}
}

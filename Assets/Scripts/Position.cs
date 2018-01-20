using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Position : MonoBehaviour {

	public class Direction {
		public Position position;
		public direction dir;
		public string name;
	}

	public List<Direction> availableDirections = new List<Direction>();
	public List<Position> neighbourPositions = new List<Position>();

	public string special;

	public enum direction {right, left, forwards, backwards}

	public GameObject arrow;
	public List<GameObject> arrows = new List<GameObject>();


	// Use this for initialization
	void Start () {
		UpdateDirections ();
	}

	public void UpdateDirections() {
		availableDirections.Clear ();
		foreach (Position _pos in neighbourPositions) {
			Direction _dir = new Direction ();
			_dir.position = _pos;
			if (Mathf.Abs (transform.position.z - _pos.transform.position.z) < Mathf.Abs (transform.position.x - _pos.transform.position.x)) {
				if (transform.position.x > _pos.transform.position.x) {
					_dir.dir = direction.left;
				} else {
					_dir.dir = direction.right;
				}
			} else {
				if (transform.position.z > _pos.transform.position.z) {
					_dir.dir = direction.backwards;
				} else {
					_dir.dir = direction.forwards;
				}
			}

			if (_pos.special != null) {
				_dir.name = _pos.special;
			} else {
				_dir.name = "NULL";
			}

			availableDirections.Add (_dir);
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public Position isAvailable(string s) {
		foreach (Direction _dir  in availableDirections) {
			if (_dir.dir.ToString() == s || _dir.name == s) {
				return _dir.position;
			}
		}

		return null;
	}

	public Position isEnemyAvailable(string desc) {
		foreach (Direction _dir in availableDirections) {
			if (_dir.position.tag == "Enemy") {
				if (desc != null) {
					if (_dir.name == desc) {
						//returns enemy with desired description/color
						return _dir.position;
					}
				} else {
					//returns enemy if no description given
					return _dir.position;
				}
			}
		}
		return null;
	}

	public IEnumerator ShowAvailable(PlayerMovement pm) {


		yield return new WaitForSeconds (0.1f);
		foreach (Position p in neighbourPositions) {
			GameObject a = Instantiate (arrow, (transform.position + (p.transform.position - transform.position).normalized * 1.2f), Quaternion.identity);
			if (availableDirections [neighbourPositions.IndexOf (p)].name != "") {
				a.GetComponentInChildren<TextMeshProUGUI> ().text = availableDirections [neighbourPositions.IndexOf (p)].name;
			} else {
				a.GetComponentInChildren<TextMeshProUGUI> ().text = availableDirections [neighbourPositions.IndexOf (p)].dir.ToString ();
			}
			arrows.Add (a);
			a.transform.LookAt (p.transform);
		}

		pm.isMoving = false;

		while (!pm.isMoving) {
			int i = 0;
			foreach (Position p in neighbourPositions) {
				arrows[i].transform.position = (transform.position + (p.transform.position - transform.position).normalized * 1.2f);
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

	public void Die() {
		foreach (Position _pos in neighbourPositions) {
			_pos.neighbourPositions.Remove (this);
			_pos.UpdateDirections ();
		}

		Destroy (gameObject);
	}
}

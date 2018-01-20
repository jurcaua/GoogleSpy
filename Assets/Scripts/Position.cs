using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}

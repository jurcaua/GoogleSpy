using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSight : MonoBehaviour {

	public float range;
	[Range(0,360)]
	public float viewAngle;
	public float sightAngle;
	public float angle;


	public LayerMask targetMask;
	public LayerMask obstacleMask;
	public LayerMask terrainMask;

	public Vector3 wayFacing;
	public Transform target;


	[Header("Mesh")]
	public int numVertex;
	public Mesh viewMesh;
	public MeshFilter MeshF;

	public Material normal;
	public Material found;
	public float test;

	public List<Transform> visibleTargets = new List<Transform> ();
	// Use this for initialization
	void Start () {
		//StartCoroutine (FindTargets ());
		viewMesh = new Mesh ();
		GetComponent<MeshFilter> ().mesh = viewMesh;
	}

	// Update is called once per frame
	void Update () {

		foreach (Transform t in visibleTargets) {
			Debug.DrawLine (transform.position, t.position, Color.red);
		}

		if (visibleTargets.Count > 0) {
			GetComponent<Renderer> ().material = found;
		} else {
			GetComponent<Renderer> ().material = normal;
		}
	}

	void LateUpdate() {
		DrawMesh ();
	}

	IEnumerator FindTargets() {
		while (true) {
			//FindVisibleTargets ();
			yield return new WaitForSeconds (0.1f);
		}
	}

	public Vector3 DirFromAngle(float angle, bool isGlobal) {
		if (!isGlobal) {
			angle += transform.eulerAngles.y;
		}
		return new Vector3 (Mathf.Sin (angle * Mathf.Deg2Rad), Mathf.Cos (angle * Mathf.Deg2Rad), 0);
	}

	void FindVisibleTargets() {
		visibleTargets.Clear ();
		Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll (transform.position, range, targetMask);
		//Debug.Log (targetsInViewRadius.Length);
		for (int i = 0; i < targetsInViewRadius.Length; i++) {
			Transform target = targetsInViewRadius [i].transform;
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle (wayFacing, dirToTarget) < (viewAngle) / 2) {
				float distToTarget = Vector3.Distance (transform.position, target.position);

				if (!Physics2D.Raycast (transform.position, dirToTarget, distToTarget, terrainMask)) {
					if (target.name == "Player") {
						visibleTargets.Add (target);
					}
				}
			}
		}
	}

//	void DrawMesh() {
//		int stepcount = Mathf.RoundToInt (viewAngle * meshresolution);
//		float stepAngleSize = viewAngle / stepcount;
//
//		List<Vector3> viewPoints = new List<Vector3> ();
//		ViewCastInfo oldViewCast = new ViewCastInfo ();
//
//		for (int i = 0; i <= stepcount; i++) {
//			float angle = (transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i); //TODO * wayFacing.x;
//			ViewCastInfo newViewCast = ViewCast (angle);
//
//			//
//
//			viewPoints.Add (newViewCast.point);
//			oldViewCast = newViewCast;
//		}
//		int vertexCount = viewPoints.Count + 1;
//		Vector3[] vertices = new Vector3[vertexCount];
//		int[] triangles = new int[(vertexCount - 2) * 3];
//
//		vertices [0] = Vector3.zero;
//		for (int i = 0; i < vertexCount - 1; i++) {
//			vertices [i + 1] = transform.InverseTransformPoint (viewPoints [i]);
//
//			if (i < vertexCount - 2) {
//				triangles [i * 3] = 0;
//				triangles [i * 3 + 1] = i + 1;
//				triangles [i * 3 + 2] = i + 2;
//			}
//		}
//		viewMesh.Clear ();
//		viewMesh.vertices = vertices;
//		viewMesh.triangles = triangles;
//		viewMesh.RecalculateNormals ();
//	}
//		

	void DrawMesh() {
		//int stepcount = Mathf.RoundToInt (viewAngle * meshresolution);
		visibleTargets.Clear ();
		float stepAngleSize = viewAngle / (numVertex + 1);
		List<Vector3> viewPoints = new List<Vector3> ();

		for (int i = 0; i < numVertex; i++) {
			RaycastHit2D hit;
			Vector3 dir = DirFromAngle (angle + 90+(viewAngle - (i + 1) * stepAngleSize * 2) / -2, false) * range * wayFacing.x;

			if (!(hit = Physics2D.Raycast (transform.position, dir, range, obstacleMask))) {
			//	Debug.DrawRay (transform.position, dir, Color.red);
				viewPoints.Add (dir);
			} else {
			//	Debug.DrawRay (transform.position, DirFromAngle (angle + 90+(viewAngle - (i + 1) * stepAngleSize * 2) / -2, false) * wayFacing.x * hit.distance, Color.red);
				viewPoints.Add (DirFromAngle (angle + 90+(viewAngle - (i + 1) * stepAngleSize * 2) /- 2, false) * wayFacing.x * hit.distance);
				if (hit.transform.name == "Player") {
					visibleTargets.Add (hit.transform);
				}
			}
		}

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices [0] = Vector3.zero;
		for (int i = 0; i < vertexCount - 1; i++) {


			vertices [i + 1] = new Vector3 (viewPoints[i].x, viewPoints [i].y, 0);

			if (i < vertexCount - 2) {
				triangles [i * 3] = 0;
				triangles [i * 3 + 1] = i + 1;
				triangles [i * 3 + 2] = i + 2;
			}
		}

		viewMesh.Clear ();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateBounds ();
		viewMesh.RecalculateNormals ();
}

	ViewCastInfo ViewCast(float angle) {
		Vector3 dir = DirFromAngle (angle, true);
		RaycastHit2D hit;

		if ((hit = Physics2D.Raycast (transform.position, dir, range, obstacleMask))) {
			return new ViewCastInfo (true, hit.point, hit.distance, angle);
		} else {
			return new ViewCastInfo (false, transform.position + dir * range, range, angle);
		}
	}
		

	public struct ViewCastInfo {
		public bool hit;
		public Vector3 point;
		public float dist;
		public float angle;

		public ViewCastInfo(bool _hit, Vector3 _point, float _dist, float _angle) {
			hit = _hit;
			point = _point;
			dist = _dist;
			angle = _angle;
		}

	}
}
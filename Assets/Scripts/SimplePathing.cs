using UnityEngine;
using System.Collections;

[RequireComponent (typeof (CharacterController))]
[RequireComponent (typeof (Seeker))]

public class SimplePathing : MonoBehaviour {

	public CharacterController controller;
	public Pathfinding.Path path;
	public Vector3 targetPosition;
	public float speed;
	public float nextWaypointDistance;
	public int currentWaypoint;
	public Seeker seeker;
	public bool endReached;
	public Vector3 curPos;

	//A function to initialize values;
	public void initVarsWithDefaultValues(){
		speed = 1000;

		nextWaypointDistance = 3f;
		currentWaypoint = 0;

		endReached = false;
	}

	//Called before Start
	public void Awake(){
		initVarsWithDefaultValues();
	}

	// Use this for initialization
	public void Start () {
		controller = (GetComponent(typeof(CharacterController)) as CharacterController);
		seeker = (GetComponent(typeof(Seeker)) as Seeker);
		//seeker.StartPath(transform.position, targetPosition, OnPathComplete);
	}

	public void OnPathComplete(Pathfinding.Path p){
		Debug.Log("Yay, we got a path back. Did it have an error? " +p.error);
		if (!p.error) {
			path = p;
			//Reset the waypoint counter
			currentWaypoint = 0;
			endReached = false;
		}
	}

	// Update is called once per frame
	public void Update () {
		
	}
	
	public void FixedUpdate(){
		if (path == null) {
			//We have no path to move after yet
			return;
		}
		
		
		if (currentWaypoint >= path.vectorPath.Count) {
			if(endReached == false){
				Debug.Log ("End Of Path Reached");
				endReached = true;
			}
			return;
		}
		
		//Direction to the next waypoint
		Vector3 dir = (path.vectorPath[currentWaypoint]-transform.position).normalized;
		dir *= (speed * Time.fixedDeltaTime);
		controller.SimpleMove(dir);
		
		//Check if we are close enough to the next waypoint
		//If we are, proceed to follow the next waypoint
		if (Vector3.Distance (transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
			currentWaypoint++;
			return;
		}
	}
	
	public void PathTo(Vector3 targetPos){
		seeker.StartPath(transform.position, targetPos, OnPathComplete); 
		targetPosition = targetPos;
	}
}

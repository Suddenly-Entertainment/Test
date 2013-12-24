#pragma strict
@script RequireComponent(CharacterController)
@script RequireComponent(Seeker)

var controller: CharacterController;
var path: Pathfinding.Path;
var targetPosition: Vector3;
var speed: float = 1000;
var nextWaypointDistance: float = 3;
var currentWaypoint: int = 0;
var seeker: Seeker;
var endReached: boolean = false;
var curPos: Vector3;

function Start () {
	controller = GetComponent(CharacterController);
	seeker = GetComponent(Seeker);
	//seeker.StartPath(transform.position, targetPosition, OnPathComplete);
}

function OnPathComplete(p : Pathfinding.Path){
	Debug.Log("Yay, we got a path back. Did it have an error? " +p.error);
	    if (!p.error) {
            path = p;
            //Reset the waypoint counter
            currentWaypoint = 0;
           	endReached = false;
        }
}
function Update () {

}

function FixedUpdate(){
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
        var  dir: Vector3 = (path.vectorPath[currentWaypoint]-transform.position).normalized;
        dir *= speed * Time.fixedDeltaTime;
        controller.SimpleMove (dir);
        
        //Check if we are close enough to the next waypoint
        //If we are, proceed to follow the next waypoint
        if (Vector3.Distance (transform.position, path.vectorPath[currentWaypoint]) < nextWaypointDistance) {
            currentWaypoint++;
            return;
        }
}

function PathTo(targetPos: Vector3){
	seeker.StartPath(transform.position, targetPos, OnPathComplete); 
	targetPosition = targetPos;
}
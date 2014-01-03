using UnityEngine;
using System.Collections;

public class NexusDefault : UnitBase {

	// Use this for initialization
	public override void Start () {
		base.Start();
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	[RPC]
	public override void Die(){
		TEAMS Winner = TEAMS.NA;

		if(Team == TEAMS.BLUE)Winner = TEAMS.ORANGE;
		else if(Team == TEAMS.ORANGE)Winner = TEAMS.BLUE;

		GameObject.Find ("GameManager").GetComponent<GameManager>().MatchEnd(Winner);

		Debug.LogError ("The rest of this function has not been implemented yet!");
	}
}

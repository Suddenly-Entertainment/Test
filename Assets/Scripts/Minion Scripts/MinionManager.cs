using UnityEngine;
using System.Collections;
[RequireComponent (typeof (SphereCollider))]

public class MinionManager : UnitBase {
	//@script RequireComponent(GenericCreature);
	//@script RequireComponent(SphereCollider);
	SphereCollider SpCo;

	// Use this for initialization
	public override void Start () {
		base.Start ();
		SpCo = (GetComponent(typeof(SphereCollider)) as SphereCollider);

		objHeight = SpCo.radius*2;
		objWidth = SpCo.radius*2;
	}
	
	// Update is called once per frame
	public override void Update () {
		base.Update();
	}

	[RPC]
	public override IEnumerator Die(){
		if(Network.isServer){
			Network.Destroy(networkView.viewID);
			//TODO: Add the rest :P
		}

		yield return new WaitForEndOfFrame();
	}
}

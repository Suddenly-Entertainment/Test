using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace SuddenlyEntertainment
{
	public class YasuoWScript: MonoBehaviour
	{
		float endTime;

		void Start(){
			endTime = Time.time + 5;
		}
		void Update(){
			if(Time.time >= endTime){
				Network.Destroy(gameObject);
			}
		}
		void OnTriggerEnter(Collider c){
			if(c.tag == "Projectile"){
				Network.Destroy(c.gameObject);
			}
		}
	}

}
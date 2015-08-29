﻿using UnityEngine;
using System.Collections.Generic;


namespace KoboldAI {
	public class Kobold : Actor{

		public float FieldOfView = 110f;
		public float SightDistance = 10f;
		public float HearDistance = 30f;

		public bool IsAlerted = false;

		private SphereCollider senseCollider = null;

		public override void Start()
		{
			base.Start();
			senseCollider = this.gameObject.AddComponent<SphereCollider>();
			senseCollider.radius = Mathf.Max(SightDistance,HearDistance);
			senseCollider.isTrigger = true;
		}



		#region SENSES

		public void OnTriggerStay(Collider other)
		{
			Actor actor = other.GetComponent<Unit>();
			if (actor != null)
			if(actor.Team != this.Team)
			{

				Vector3 dir = other.transform.position - this.transform.position;
				float angle = Vector3.Angle(dir,this.transform.forward);

				// Vision
				if(angle < FieldOfView * 0.5f)
				{
					RaycastResult rRes = navGraph.RayCast(this.transform.position,dir.normalized,SightDistance,AccessType.Fly);
					if (rRes.doesHit)
					{
						Debug.Log("Kobold: I has visual to " + rRes.actor.gameObject.name + " from team " + rRes.actor.Team);
					}
				}

				// Hearing
				if (!IsAlerted)
					if (actor.isMoving)
					{
						if(navGraph.GetShortestPath(this.transform.position,actor.transform.position,AccessType.Fly).Count <= Mathf.FloorToInt(HearDistance))
						{
							Debug.Log("Kobold: I hear someone moving nearby!");
							IsAlerted = true;
						}
					}
			}

		}

		public void OnTriggerExit(Collider other)
		{
			IsAlerted = false;
			Debug.Log("Kobold: Must be the wind...");
		}

		#endregion
	}
}

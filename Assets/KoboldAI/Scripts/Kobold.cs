using UnityEngine;
using System.Collections.Generic;


namespace KoboldAI {
	public class Kobold : Actor{

		public float FieldOfView = 90f;
		public float SightDistance = 10f;
		public float HearDistance = 30f;

		public bool IsAlerted = false;

		public List<GameObject> hearables = new List<GameObject>();
		public List<GameObject> visibles = new List<GameObject>();

		public override void Start()
		{
			base.Start();
		}

		void FixedUpdate()
		{
			if(Manager.Instance.LevelGenerator.Ready && Manager.Instance.NavGraph != null)
			{
				visibles.Clear();
				hearables.Clear();
				List<RaycastResult> visionResults = CastVisionRays();
				List<RaycastResult> hearingResults = CastHearingRays();
				foreach(RaycastResult r in visionResults)
				{
					if (r.doesHit)
					{
						if(!visibles.Contains(r.gameObject))
						{
							Debug.Log("Kobold: I see "+r.gameObject.name+" @ "+r.position.ToString());
							visibles.Add(r.gameObject);
						}

					}
				}
				foreach(RaycastResult r in hearingResults)
				{
					if (r.doesHit)
					{
						if(!hearables.Contains(r.gameObject))
						{
							Debug.Log("Kobold: I hear "+r.gameObject.name+" @ "+r.position.ToString());
							hearables.Add(r.gameObject);
						}
						
					}
				}
				if (hearables.Count > 0 || visibles.Count > 0)
					IsAlerted = true;
				else
					IsAlerted = false;
			}
		}

		#region SENSES

		private List<RaycastResult> CastHearingRays()
		{
			List<RaycastResult> results = new List<RaycastResult>();
			if (navGraph != null)
			{
				for (float angle = 0; angle <= 360; angle += 5)
				{
					float radians = Mathf.Deg2Rad * angle;
					Vector3 direction = new Vector3(Mathf.Sin(radians),0,Mathf.Cos(radians));
					results.Add(navGraph.RayCast(this.transform.position,direction,HearDistance,AccessType.Fly));
				}
			}
			return results;
		}

		private List<RaycastResult> CastVisionRays()
		{
			List<RaycastResult> results = new List<RaycastResult>();
			if (navGraph != null)
			{
				for (float angle = -FieldOfView/2; angle <= FieldOfView/2; angle += 5)
				{
					float radians = Mathf.Deg2Rad * (angle+transform.eulerAngles.y);
					Vector3 direction = new Vector3(Mathf.Sin(radians),0,Mathf.Cos(radians));
					results.Add(navGraph.RayCast(this.transform.position,direction,SightDistance,AccessType.Fly));
				}
			}
			return results;
		}

		#endregion
	}
}

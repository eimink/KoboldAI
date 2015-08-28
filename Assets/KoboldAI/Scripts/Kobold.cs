using UnityEngine;
using System.Collections.Generic;


namespace KoboldAI {
	public class Kobold : MonoBehaviour {

		Graph navGraph = null;

		List<Node> currentPath = null;

		public int Speed = 2;
		public AccessType TravelMethod = AccessType.Walk;

		private Vector3 targetPosition = Vector3.zero;
		private bool isMoving = false;

		// Use this for initialization
		void Start () {
			targetPosition = this.transform.position;
			navGraph = GameObject.Find("Map").GetComponent<LevelGenerator>().NodeGraph;
		}

		void Update () {
			if (navGraph == null)
			{
				navGraph = GameObject.Find("Map").GetComponent<LevelGenerator>().NodeGraph;
			}
			if (currentPath != null)
			{
				Debug.DrawLine(this.transform.position,navGraph.GraphPosToWorld(currentPath[0].Position),Color.red);
				for(int i = 0; i < currentPath.Count -1;i++)
				{
					Debug.DrawLine(navGraph.GraphPosToWorld(currentPath[i].Position),navGraph.GraphPosToWorld(currentPath[i+1].Position),Color.red);
				}
			}
			if (this.transform.position != targetPosition)
			{
				isMoving = true;
				this.transform.position = targetPosition;
			}
			else
				isMoving = false;
		}

		public void MoveTo(Vector3 position)
		{
			currentPath = null;
			currentPath = navGraph.GetShortestPathDijkstra(this.transform.position,position,TravelMethod);
		}

		public void TravelAlongPath()
		{
			if (currentPath != null && !isMoving)
			{
				int movementLeft = Speed;
				while (movementLeft > 0){
					targetPosition = navGraph.GraphPosToWorld(currentPath[1].Position);
					movementLeft -= Mathf.FloorToInt(currentPath[1].CostToEnter(TravelMethod));
					currentPath.RemoveAt(0);
					if (currentPath.Count == 1)
					{
						currentPath = null;
						break;
					}
				}
			}
		}
	}
}

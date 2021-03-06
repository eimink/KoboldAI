﻿using UnityEngine;
using System.Collections.Generic;

namespace KoboldAI {
	public class Actor : MonoBehaviour {

		public int Speed = 2;
		public AccessType TravelMethod = AccessType.Walk;
		public int Team = 1;
		
		private Vector3 targetPosition = Vector3.zero;
		public bool isMoving = false;
		protected Graph navGraph = null;
		private List<Node> currentPath = null;
		private bool initOccupy = false;

		// Use this for initialization
		public virtual void Start () {
			targetPosition = this.transform.position;
			navGraph = Manager.Instance.NavGraph;
			if (navGraph != null)
				navGraph.OccupyNode(navGraph.WorldPosToGraph(transform.position),this);
		}
		
		void Update () {
			if (navGraph == null)
			{
				navGraph = Manager.Instance.NavGraph;

			}
			if (!initOccupy && navGraph != null && Manager.Instance.LevelGenerator.Ready)
			{
				navGraph.OccupyNode(navGraph.WorldPosToGraph(transform.position),this);
				initOccupy = true;
			}
			if (currentPath != null)
			{
				DebugDrawPath();
				if (this.transform.position != targetPosition)
				{
					isMoving = true;
					this.transform.position = targetPosition;
				}
				else
					isMoving = false;
			}
			else
				isMoving = false;
		}
		
		private void DebugDrawPath()
		{
			Debug.DrawLine(this.transform.position,navGraph.GraphPosToWorld(currentPath[0].Position),Color.red);
			if (currentPath.Count > 1)
			for(int i = 0; i < currentPath.Count -1;i++)
			{
				Debug.DrawLine(navGraph.GraphPosToWorld(currentPath[i].Position),navGraph.GraphPosToWorld(currentPath[i+1].Position),Color.red);
			}
		}
		
		#region MOVEMENT
		
		public void MoveTo(Vector3 position)
		{
			currentPath = null;
			currentPath = navGraph.GetShortestPath(this.transform.position,position,TravelMethod);
		}
		
		public void TravelAlongPath()
		{
			if (currentPath != null && currentPath.Count > 1 && !isMoving)
			{
				int movementLeft = Speed;
				while (movementLeft > 0){
					navGraph.OccupyNode(currentPath[0].Position,null);
					currentPath.RemoveAt(0);
					movementLeft -= Mathf.FloorToInt(currentPath[0].CostToEnter(TravelMethod));
					targetPosition = navGraph.GraphPosToWorld(currentPath[0].Position);
					navGraph.OccupyNode(currentPath[0].Position,this);
					if (currentPath.Count <= 1)
					{
						movementLeft = 0;
						break;
					}
				}
			}
		}
		
		#endregion
	}
}

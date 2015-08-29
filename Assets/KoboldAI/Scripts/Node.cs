using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace KoboldAI {

	public class Node {

		public Vector2 Position {get{return m_position;} set{m_position = value;}}
		public Dictionary<Node,float> Neighbors {get{return m_neighbors;} set{m_neighbors = value;}}
		public bool IsTraversable(AccessType travelMethod = AccessType.Walk) {return FlagsHelper.IsSet(m_tile.AccessibleBy,travelMethod);}
		public float CostToEnter(AccessType travelMethod = AccessType.Walk) {return m_tile.GetAccessCost(travelMethod);}
		public Actor OccupiedBy = null;
		private Dictionary<Node,float> m_neighbors = new Dictionary<Node,float>();
		private Vector3 m_position = Vector3.zero;
		private Tile m_tile;

		public Node(Vector2 position, Tile tile)
		{
			this.m_position = position;
			this.m_tile = tile;
		}

	}

}

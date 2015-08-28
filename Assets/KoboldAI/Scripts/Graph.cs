using UnityEngine;
using System.Collections.Generic;

namespace KoboldAI {

	public class Graph {

		private Vector3 m_WorldOrigin = Vector3.zero;

		public void SetWorldOrigin(Vector3 origin) { m_WorldOrigin = origin; }

		public Node[,] Nodes;

		public void AddNode(Vector2 pos, Tile tile)
		{
			Nodes[Mathf.FloorToInt(pos.x),Mathf.FloorToInt(pos.y)] = new Node(pos,tile);
		}

		public void SetEightWayNeighbors(int sizeX, int sizeY)
		{
			for(int x = 0; x < sizeX; x++)
			{
				for(int y = 0; y < sizeY; y++)
				{
					if (x > 0)
					{
						Nodes[x,y].Neighbors.Add(Nodes[x-1,y],1f);
						if (y > 0)
						{
							Nodes[x,y].Neighbors.Add(Nodes[x-1,y-1],1.0001f);
						}
						if (y < sizeY-1)
						{
							Nodes[x,y].Neighbors.Add(Nodes[x-1,y+1],1.0001f);
						}
					}
					if (x < sizeX-1)
					{
						Nodes[x,y].Neighbors.Add(Nodes[x+1,y],1f);
						if (y > 0)
						{
							Nodes[x,y].Neighbors.Add(Nodes[x+1,y-1],1.0001f);
						}
						if (y < sizeY-1)
						{
							Nodes[x,y].Neighbors.Add(Nodes[x+1,y+1],1.0001f);
						}
					}
					if (y > 0)
					{
						Nodes[x,y].Neighbors.Add(Nodes[x,y-1],1f);
					}
					if (y < sizeY-1)
					{
						Nodes[x,y].Neighbors.Add(Nodes[x,y+1],1f);
					}
				}
			}
		}

		public Vector3 GraphPosToWorld(Vector2 pos)
		{
			return m_WorldOrigin + new Vector3(pos.x,0,pos.y);
		}

		public Vector2 WorldPosToGraph(Vector3 pos)
		{
			Vector3 p = pos - m_WorldOrigin;
			return new Vector2(p.x,p.z);
		}

		public Graph(int sizeX = 10, int sizeY = 10)
		{
			Nodes = new Node[sizeX,sizeY];
			m_WorldOrigin = Vector3.zero;

		}

		public Graph(Vector3 origin,int sizeX = 10, int sizeY = 10)
		{
			Nodes = new Node[sizeX,sizeY];
			m_WorldOrigin = origin;
		}

		/// <summary>
		/// Gets the shortest path with dijkstra. Converts automatically from world coordinates to graph.
		/// </summary>
		/// <returns>The shortest path as list of nodes.</returns>
		/// <param name="start">Start coordinates.</param>
		/// <param name="end">End coordinates.</param>
		public List<Node> GetShortestPathDijkstra(Vector3 start, Vector3 end, AccessType travelMethod = AccessType.Walk)
		{
			return GetShortestPathDijkstra(WorldPosToGraph(start),WorldPosToGraph(end), travelMethod);
		}

		/// <summary>
		/// Gets the shortest path with dijkstra.
		/// </summary>
		/// <returns>The shortest path as list of nodes.</returns>
		/// <param name="start">Start coordinates.</param>
		/// <param name="end">End coordinates.</param>
		public List<Node> GetShortestPathDijkstra(Vector2 start, Vector2 end, AccessType travelMethod = AccessType.Walk)
		{
			Debug.Log(start.ToString()+" "+end.ToString());
			return GetShortestPathDijkstra(Nodes[Mathf.FloorToInt(start.x),Mathf.FloorToInt(start.y)],Nodes[Mathf.FloorToInt(end.x),Mathf.FloorToInt(end.y)],travelMethod);
		}

		/// <summary>
		/// Gets the shortest path with dijkstra.
		/// </summary>
		/// <returns>The shortest path as list of nodes</returns>
		/// <param name="start">Start node.</param>
		/// <param name="end">End node.</param>
		public List<Node> GetShortestPathDijkstra(Node start, Node end, AccessType travelMethod = AccessType.Walk)
		{
			var previous = new Dictionary<Node, Node>();
			var distances = new Dictionary<Node, float>();
			var nodes = new List<Node>();
			
			List<Node> path = null;
			
			foreach (var vertex in Nodes)
			{

				if (vertex == start)
				{
					distances[vertex] = 0;
				}
				else
				{
					distances[vertex] = Mathf.Infinity;
				}
				nodes.Add(vertex);
			}

			while (nodes.Count != 0)
			{
				Node smallest = null;
				foreach(Node n in nodes)
				{
					if ( smallest == null || distances[n] < distances[smallest] )
						smallest = n;
				}
				nodes.Remove(smallest);

				if (smallest == end)
				{
					path = new List<Node>();
					while (previous.ContainsKey(smallest))
					{
						path.Add(smallest);
						smallest = previous[smallest];
					}
					Debug.Log("path ready @ "+smallest.Position+" with count "+path.Count);
					break;
				}
				
				if (float.IsInfinity(distances[smallest]))
				{
					Debug.Log("infinity break @ "+smallest.Position);
					break;
				}
				
				foreach (var neighbor in smallest.Neighbors)
				{
					var alt = distances[smallest] + (neighbor.Key.CostToEnter(travelMethod)*neighbor.Value);
					if (alt < distances[neighbor.Key])
					{
						distances[neighbor.Key] = alt;
						previous[neighbor.Key] = smallest;
					}
				}
			}
			if (path != null)
				path.Reverse();
			return path;
		}

		private int DistanceComparator(float arg1, float arg2)
		{
			if (arg1 == arg2)
				return 0;
			if (!float.IsInfinity(arg1) && !float.IsInfinity(arg2))
			{
				if(arg1 < arg2) return -1;
				else return 1;
			}
			else if(!float.IsInfinity(arg1))
				return -1;
			else
				return 1;
		}
	}

}

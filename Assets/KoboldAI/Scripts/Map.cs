using UnityEngine;
using System.Collections.Generic;

namespace KoboldAI {

	public class Map : MonoBehaviour {

		public int SizeX = 16;
		public int SizeY = 16;
		
		public Object[] tilePrefabs;

		public Graph NodeGraph;

		// Use this for initialization
		void Awake() {
			NodeGraph = new Graph(this.transform.position,SizeX,SizeY);
			for(int x = 0; x < SizeX; x++)
			{
				for(int y = 0; y < SizeY; y++)
				{
					Vector3 pos = new Vector3(x,0,y);
					GameObject g = (GameObject)Instantiate(tilePrefabs[0],pos,Quaternion.identity);
					g.transform.parent = this.gameObject.transform;
					Tile t = g.GetComponent<Tile>();
					NodeGraph.AddNode(new Vector2(x,y),t);
				}
			}
			NodeGraph.SetEightWayNeighbors(SizeX,SizeY);

		}
		
		// Update is called once per frame
		void Update () {

		}
	}

}

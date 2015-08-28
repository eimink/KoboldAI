using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

namespace KoboldAI {

	public class LevelGenerator : MonoBehaviour {

		[Serializable]
		public class GenBlock {
			public Tile LevelTile;
			public Color IdentifierColor;
		}

		public GenBlock[] blocks;
		public GameObject floorTile;
		public Color floorColor;
		public Graph NodeGraph;
		public bool Ready {get{return m_ready;}}

		GameObject m_levelParent;
		bool m_ready = false;

		// Use this for initialization
		void Start () {
			Init();
		}

		protected void Init() {
			m_ready = false;
			if (m_levelParent != null)
				Destroy (m_levelParent);
			if (GameObject.Find("GeneratedLevel") != null)
				Destroy (GameObject.Find("GeneratedLevel"));
			m_levelParent = new GameObject ();
			m_levelParent.name = "GeneratedLevel";
		}

		protected void GenerateLevelFromBitmap(Texture2D bitmap)
		{
			Color[] pixels = bitmap.GetPixels();
			int width = bitmap.width;
			int height = bitmap.height;
			NodeGraph = new Graph(this.transform.position,width,height);
			for (int i = 0; i < width; i++)
			{
				for (int j = 0; j < height; j++)
				{
					GameObject o = null;
					int idx = FindBlockIndex(pixels[i*width+j]);
					if (idx >= 0)
					{
						o = (GameObject)Instantiate(blocks[idx].LevelTile.gameObject,new Vector3(i,0,j),Quaternion.identity);
					}
					else
					{
						o = (GameObject)Instantiate(floorTile,new Vector3(i,0,j),Quaternion.identity);
					}
					o.transform.parent = m_levelParent.transform;
					Tile t = o.GetComponent<Tile>();
					NodeGraph.AddNode(new Vector2(i,j),t);
				}
			}
			NodeGraph.SetEightWayNeighbors(width,height);
			m_ready = true;
		}

		protected int FindBlockIndex(Color c)
		{
			for (int i = 0; i < blocks.Length; i++) 
			{
				if (blocks [i].IdentifierColor == c)
				{
					return i;
				}
			}
			return -1;
		}
	}
}

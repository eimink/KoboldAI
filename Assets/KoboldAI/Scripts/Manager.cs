using UnityEngine;
using System.Collections;

namespace KoboldAI {
	public class Manager : Singleton<KoboldAI.Manager> {

		protected Manager () {}

		public LevelGenerator LevelGenerator;
		public Graph NavGraph {get{return LevelGenerator.NodeGraph;}}

	}
}

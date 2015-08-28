using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace KoboldAI {
	public class BitmapLevelGenerator : LevelGenerator {

		public Texture2D testBitmap;

		void Start () {
			Init();
			GenerateLevelFromBitmap (testBitmap);
		}
	}
}

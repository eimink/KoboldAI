using UnityEngine;
using System;
using System.Collections;

namespace KoboldAI {

	public class RandomLevelGenerator : LevelGenerator {

		public int spawnpoints = 4;
		public int width = 64; 
		public int height = 64;
		public int xOrg = 0;
		public int yOrg = 0;
		public float scale = 1.0f;
		public float darkTreshold = 0.3f;
		public float midTreshold = 0.5f;
		public float lightTreshold = 0.7f;

		bool m_started = false;

		// Use this for initialization
		void Start () {
			Init();
			RunGenerator();
		}

		void RunGenerator()
		{
			m_started = true;
		}

		int TextToInteger(string param)
		{
			int rval = 0;
			for (int i=0; i < param.Length; i++)
			{
				rval += (int)param[i];
			}
			return rval;
		}

		void GenerateWithSeed(string seed)
		{
			Init();
			UnityEngine.Random.seed = (TextToInteger(seed));
			xOrg = UnityEngine.Random.Range(-64,64);
			yOrg = UnityEngine.Random.Range(-64,64);
			scale = UnityEngine.Random.Range (2, 16);
			darkTreshold = UnityEngine.Random.Range (1f, 40f) / 100f;
			midTreshold = UnityEngine.Random.Range(41f,70f) / 100f;
			lightTreshold = UnityEngine.Random.Range (71f, 100f) / 100f;
			RunGenerator();
		}

		void Update()
		{
			if (!Ready && m_started)
			{
				Texture2D perlin = PerlinGenerator.CreatePerlinTexture(width, height, xOrg, yOrg, scale);
				GenerateLevelFromBitmap(GenerateDebris(ApplyTresholds(perlin)));
			}
		}
		 // 102, 153
		Texture2D ApplyTresholds(Texture2D tex)
		{
			Color[] pixels = tex.GetPixels ();
			for (int i = 0; i < pixels.Length; i++)
			{
				Color c = pixels[i];
				c.r = c.r <= darkTreshold ? 0f : c.r >= lightTreshold ? 1f : c.r <= midTreshold ? 0.4f : 0.6f;
				c.g = c.g <= darkTreshold ? 0f : c.g >= lightTreshold ? 1f : c.g <= midTreshold ? 0.4f : 0.6f;
				c.b = c.b <= darkTreshold ? 0f : c.b >= lightTreshold ? 1f : c.b <= midTreshold ? 0.4f : 0.6f;
				pixels[i] = c;
			}
			tex.SetPixels(pixels);
			return tex;
		}

		Texture2D GenerateDebris(Texture2D t)
		{
			Texture2D tex = t;
			Color debrisColor = new Color (0, 1, 0);
			int debrisAmount = UnityEngine.Random.Range (16, 64);
			for (int i = 0; i < debrisAmount; i++)
			{
				Vector2 point = GetPoint();
				Color pixel = tex.GetPixel((int)point.x,(int)point.y);
				if(pixel != floorColor)
				{
					i++;
				}
				else
					tex.SetPixel((int)point.x,(int)point.y,debrisColor);
			}
			return tex;
		}

		Vector2 GetPoint()
		{
			return new Vector2 (UnityEngine.Random.Range (0, width),UnityEngine.Random.Range (0, height));
		}

		Vector2 GetPoint(int quadrant)
		{
			if (quadrant <= 1)
				return new Vector2 (UnityEngine.Random.Range (width/4, width/2), UnityEngine.Random.Range (height/4, height/2));
			else if (quadrant == 2)
				return new Vector2 (UnityEngine.Random.Range (width/2, 3*width/4), UnityEngine.Random.Range (height/4, height/2));
			else if (quadrant == 3)
				return new Vector2 (UnityEngine.Random.Range (width/4, width/2),UnityEngine.Random.Range (height/2, 3*height/4));
			else
				return new Vector2 (UnityEngine.Random.Range (width/2, 3*width/4),UnityEngine.Random.Range (height/2, 3*height/4));
		}
	}

}

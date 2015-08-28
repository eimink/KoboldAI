using UnityEngine;
using System.Collections;

public static class PerlinGenerator {

	public static Texture2D CreatePerlinTexture(int width, int height, int xOrg = 0, int yOrg = 0, float scale = 1.0f)
	{
		Texture2D noiseTex = new Texture2D (width, height);
		Color[] pix = new Color[noiseTex.width * noiseTex.height];
		float y = 0.0F;
		while (y < noiseTex.height) {
			float x = 0.0F;
			while (x < noiseTex.width) {
				float xCoord = xOrg + x / noiseTex.width * scale;
				float yCoord = yOrg + y / noiseTex.height * scale;
				float sample = Mathf.PerlinNoise(xCoord, yCoord);
				pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
				x++;
			}
			y++;
		}
		noiseTex.SetPixels(pix);
		return noiseTex;
	}
}

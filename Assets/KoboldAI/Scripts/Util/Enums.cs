using UnityEngine;
using System.Collections;

namespace KoboldAI {

	public enum Teams
	{
		Players = 0,
		Enemies
	}

	public enum TileType 
	{
		Grass = 0,
		Swamp,
		Water,
		Rock
	}
	
	public enum AccessType
	{
		None = 0,
		Swim,
		Walk,
		WalkAndSwim,
		Fly,
		FlyAndSwim,
		FlyAndWalk,
		All
	}
}

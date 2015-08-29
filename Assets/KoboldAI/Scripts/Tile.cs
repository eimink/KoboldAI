using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KoboldAI
{

	[Serializable]
	public class AccessCost
	{
		public AccessType Type = AccessType.All;
		public float Cost = 1.0f;
	}

	[Serializable]
	public class Tile : MonoBehaviour{
		public TileType Type = TileType.Grass;
		public AccessType AccessibleBy = AccessType.All;
		public List<AccessCost> AccessCosts;
		public float GetAccessCost(AccessType accessMethod)
		{
			AccessCost ac = AccessCosts.Find(p => FlagsHelper.IsSet(p.Type,accessMethod));
			if (ac != null)
				return ac.Cost;
			else return Mathf.Infinity;
		}

	}

}

using UnityEngine;
using System.Collections;

namespace KoboldAI{
	public class Clicker : MonoBehaviour {

		public void OnMouseUp()
		{
			Debug.Log("click @ " + this.transform.position.ToString());
			GameObject.Find("Unit").GetComponent<Unit>().MoveTo(this.transform.position);
		}
	}
}

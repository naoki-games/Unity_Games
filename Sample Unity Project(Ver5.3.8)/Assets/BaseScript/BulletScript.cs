using UnityEngine;
using System.Collections;

public class BulletScript : MonoBehaviour {
	public float limit = 5.0f;
	// Use this for initialization
	void Start () {
		GameObject.Destroy(this.gameObject,limit);
	
	}
	void Update () {
		
		
	}
}
using UnityEngine;
using System.Collections;

public class ExplodeDamage : MonoBehaviour {

	private Player2 pscript;
	private Transform SourcePos;

	int layerMask;
	float radius = 2.0f;

	// Use this for initialization
	void Start () {
		pscript = GameObject.FindGameObjectWithTag ("Player").GetComponent<Player2>();
		SourcePos = this.gameObject.transform;

		layerMask = 1 << LayerMask.NameToLayer("Player");
		#if UNITY_EDITOR
		// helper to visualise the ground check ray in the scene view
		#endif
		//Debug.DrawRay (ray.origin, ray.direction, Color.red, 1.0f, false);

		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (Physics.CheckSphere (transform.position, radius, layerMask)) {
			Debug.Log("Damage");
			if (pscript.damage == false) {
				pscript.Damage (SourcePos);
			}
		}
	}

	void OnDrawGizmos(){
		Gizmos.DrawWireSphere(transform.position,radius);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

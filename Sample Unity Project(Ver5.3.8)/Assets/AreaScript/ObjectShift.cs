using UnityEngine;
using System.Collections;

public class ObjectShift : MonoBehaviour {
	public int high = 20;
	public int low = 10;
	private GameObject player;
	public float reDistance = 550;
	private int n ;
	public float speed = 1.2f;
	private float yvalue = 0f;
	private int plusminus = 1; //low=+,high=-

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
	}

	void Update () {
		float sqrDistanceToPlayer = Vector3.SqrMagnitude(transform.position - player.transform.position);
		if (sqrDistanceToPlayer <  reDistance){
			yvalue += speed * plusminus * Time.deltaTime;
			Vector3 p = new Vector3 (transform.position.x, yvalue+low,transform.position.z);
			transform.position = p;
			
			if(transform.position.y <= low){
				n = 0;
				plusminus = 1;
			}
			
			else if(transform.position.y >= high){
				n = 1;
				plusminus = -1;
			}
			else{
				if(n == 0){
					plusminus = 1;
				}
				else{
					plusminus = -1;
				}
			}
		}

	}

	void OnColliderEnter(Collider col){
		if (col.gameObject == player) {
			int Count = gameObject.transform.childCount;
			Debug.Log (Count);
			col.transform.parent = transform;
		}
	}

	void OnColliderExit(Collider col){
		if (col.gameObject == player) {
			col.transform.parent = null;
		}
	}
}

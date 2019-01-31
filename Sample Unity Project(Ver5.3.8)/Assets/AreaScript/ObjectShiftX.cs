using UnityEngine;
using System.Collections;

public class ObjectShiftX : MonoBehaviour {
	public int right = 20;
	public int left = 10;
	private GameObject player;
	public float reDistance = 550;
	private int n ;
	public float speed = 1.2f;
	private float xvalue = 0f;
	private int plusminus = 1; //low=+,high=-

	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
	}
	void Update () {
		float sqrDistanceToPlayer = Vector3.SqrMagnitude(transform.position - player.transform.position);
		if (sqrDistanceToPlayer <  reDistance){
			xvalue += speed * plusminus * Time.deltaTime;
			Vector3 p = new Vector3 (xvalue+left, transform.position.y, transform.position.z);
			transform.position = p;

			if(transform.position.x <= left){
				n = 0;
				plusminus = 1;
			}

			else if(transform.position.x >= right){
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
}

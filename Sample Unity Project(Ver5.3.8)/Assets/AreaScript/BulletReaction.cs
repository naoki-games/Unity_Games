using UnityEngine;
using System.Collections;

public class BulletReaction : MonoBehaviour {

	public int limit = 15;
	public GameObject projectile1;

	// Update is called once per frame
	void Update () {
		
		Destroy(gameObject,limit);
	
	}

	void OnTriggerStay(Collider other){
		if(this.gameObject.tag != "EnemyBullet"){
			if(other.tag == "Breaking"){
				Instantiate(projectile1, transform.position, transform.rotation);
				Destroy(gameObject);
			}
		}
		else{
			if(other.tag == "Player"){
				Instantiate(projectile1, transform.position, transform.rotation);
				Destroy(gameObject);
			}
		}
		if(other.tag == "Ground"){
			Instantiate(projectile1, transform.position, transform.rotation);
			Destroy(gameObject);
		}

	}
}

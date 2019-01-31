using UnityEngine;
using System.Collections;

public class FixationEnemy : MonoBehaviour {
	
	public Rigidbody bulletPrefab;
	private GameObject player;
	private bool shotFlag = false;
	public GameObject gun;
	public float kakudo = 10.0f;
	public Transform turret;
	public float power = 850f;
	public float reDistance = 550f;
	private float timer;
	// Use this for initialization
	void Start () {
		player = GameObject.FindWithTag ("Player");
	}
	// Update is called once per frame
	void Update () {
		float sqrDistanceToPlayer = Vector3.SqrMagnitude(transform.position - player.transform.position);
		if (sqrDistanceToPlayer <  reDistance)
		{ 
			Rigidbody FlameBallInstance;
			
			timer += Time.deltaTime;

			float rotateY = (turret.localEulerAngles.y > 180) ? turret.localEulerAngles.y - 360 : turret.localEulerAngles.y;
			Quaternion targetRotation = Quaternion.LookRotation(player.transform.position - turret.position);
			if(rotateY < kakudo){
				if(rotateY > -kakudo){
					turret.rotation = Quaternion.Slerp(turret.rotation, targetRotation, Time.deltaTime * 1f);
				}
				else{
					turret.localEulerAngles = new Vector3(turret.localEulerAngles.x, turret.localEulerAngles.y+1f, turret.localEulerAngles.z);
				}
			}
			else{
				turret.localEulerAngles = new Vector3(turret.localEulerAngles.x, turret.localEulerAngles.y-1f, turret.localEulerAngles.z);
			}

			if (timer > 5) {
				shotFlag = true;
				timer = 0;
			}else{
				if(shotFlag){
					FlameBallInstance = Instantiate(bulletPrefab, 
					                                gun.transform.position,turret.transform.rotation)as Rigidbody;
					
					FlameBallInstance.AddForce(turret.transform.forward * power );
				}
				shotFlag = false;
			}
		}
	}
	void randMove(){
		kakudo = Random.Range(kakudo, kakudo);
		
	}

}

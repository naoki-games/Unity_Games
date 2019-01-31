using UnityEngine;
using System.Collections;

namespace StateMachineSample{
	
	public class WeaponAttack : MonoBehaviour {

		private Player2 plscript;

		public GameObject explosionPrefab;

		void Start () {
			plscript = GameObject.FindGameObjectWithTag("Player").GetComponent<Player2>();
		}

		void OnCollisionStay(Collision col)
		{
			if (col.gameObject.tag == "Breaking")
			{
				Vector3 contactPoint = col.contacts[0].point;

				Quaternion targetRotation = Quaternion.LookRotation(transform.position - contactPoint);
				Instantiate(explosionPrefab, contactPoint, targetRotation);
				StartCoroutine("attackHitStop");
			}
			if (col.gameObject.tag == "Ground")
			{
				Vector3 contactPoint = col.contacts[0].point;
				
				Quaternion targetRotation = Quaternion.LookRotation(transform.position - contactPoint);
				Instantiate(explosionPrefab, contactPoint, targetRotation);
			}

			if (col.rigidbody != null)
			{
				col.rigidbody.AddForce(transform.forward * 3);
			}
			if (col.gameObject.tag == "Enemy")
			{
				col.gameObject.GetComponent<BossEnemy>().TakeDamage();
				StartCoroutine("attackHitStop");
			}
		}

		IEnumerator attackHitStop(){
			Debug.Log ("hit");
			plscript.anim.speed = 0.1f;
			yield return new WaitForSeconds(0.3f);
			plscript.anim.speed = 1.0f;
		}
	}
}

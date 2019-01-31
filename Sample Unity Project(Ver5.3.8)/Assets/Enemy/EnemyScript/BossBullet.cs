using UnityEngine;
using System.Collections;

namespace StateMachineSample
{
	public class BossBullet : MonoBehaviour
	{
		public GameObject gpSmokePrefab;

		private float speed = 5f;
		private float force = 150f;
		public ElementType elementType;

		public enum ElementType{
			normal,
			strength
		}

		private void Start()
		{
			if(elementType == ElementType.strength){
				Instantiate(gpSmokePrefab,transform.position,transform.rotation);
			}
			GetComponent<Rigidbody>().velocity = transform.forward * speed;
		}

		private void OnCollisionEnter(Collision collision)
		{
			if (collision.rigidbody != null)
			{
				collision.rigidbody.AddForce(transform.forward * force);
			}

			Destroy(gameObject);
		}
	}
}
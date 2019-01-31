using UnityEngine;
using System.Collections;

public class BreakableObject : MonoBehaviour {

	private VariableStatus Vstatus;
	private StateMachineSample.BossEnemy Benemy;

	public GameObject damageEf;
	public GameObject destroyEf;
	private bool destroyFlag = false;
	public int point = 0;
	public float HP = 50;
	private float damage;
	public ElementType elementType;

	public enum ElementType{
		Non,
		Big,
	}

	// Use this for initialization
	void Start () {
		Vstatus = GameObject.FindGameObjectWithTag("playerInformation").GetComponent<VariableStatus>();
		Benemy = this.GetComponent<StateMachineSample.BossEnemy>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	IEnumerator OnTriggerEnter(Collider other)
	{
		if (other.tag == "bullet"){
			damage = Vstatus.ShotATK;
			if(elementType == ElementType.Big){
				if(damage > 100){
					HP -= damage;
				}
			}
			else if(elementType == ElementType.Non){
				HP -= damage;
			}
			GameObject clone2 = 
				Instantiate(damageEf, other.transform.position,other.transform.rotation) as GameObject;
			clone2.transform.Rotate(0,180,0);

			if(HP <= 0){
				Vstatus.score += point;
				if(Benemy != null){
					Benemy.TakeDamage();
				}
				else{
					Destroy(gameObject);
					Destroy(other.gameObject.gameObject);
					
					GameObject clone = 
						Instantiate(destroyEf, transform.position, transform.rotation) as GameObject;
					yield return new WaitForSeconds(2);
					if(destroyFlag){
						point += 1;
					}
					Destroy(clone);
				}
			}else{
				GetComponent<AudioSource>().Play();
				yield return new WaitForSeconds(1);
				Destroy(clone2);
			}
		}
	}

	IEnumerator OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Attack"){
			damage = Vstatus.ATK;
			if(elementType != ElementType.Big){
				HP -= damage;
			}
			GameObject clone2 = 
				Instantiate(damageEf, col.transform.position,col.transform.rotation) as GameObject;
			clone2.transform.Rotate(0,180,0);

			if(HP <= 0){
				Vstatus.score += point;
				if(Benemy != null){
					Benemy.TakeDamage();
				}
				else{
					Destroy(gameObject);
					
					GameObject clone = 
						Instantiate(destroyEf, transform.position, transform.rotation) as GameObject;
					yield return new WaitForSeconds(2);
					if(destroyFlag){
						point += 1;
					}
					Destroy(clone);
				}
			}else{
				GetComponent<AudioSource>().Play();
				yield return new WaitForSeconds(1);
				Destroy(clone2);
			}
		}
	}

}

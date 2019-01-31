using UnityEngine;
using System.Collections;

public class DamageJudge : MonoBehaviour {

	private VariableStatus Vstatus;

	// Use this for initialization
	void Start(){
		Vstatus = GameObject.FindGameObjectWithTag ("playerInformation").GetComponent<VariableStatus>();
	}
	/*void OnTriggerEnter (Collider other) {
		if (other.tag == "EnemyBullet") {
			Vstatus.HP -= 50;
		}
	}*/
	void OnTriggerStay (Collider other) {
		if(Vstatus.endFlag == false){
			if(Vstatus.Mpoint < Vstatus.MaxMP){
				if (other.tag == "HealingPoint") {
					Vstatus.endFlag=true;
					Vstatus.Mpoint += 1;
					Vstatus.Invoke("DelayMethod",0.3f);
					Vstatus.endFlag=false;
				}
			}
		}
	}
}

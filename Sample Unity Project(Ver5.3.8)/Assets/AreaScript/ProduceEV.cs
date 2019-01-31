using UnityEngine;
using System.Collections;

public class ProduceEV : MonoBehaviour {

	private bool endflag = false;

	void Update () {
		if(!endflag){
			if(transform.rotation != Quaternion.Euler(0f,0f,0f)){
				CameraWorks.TargetChanger(this.transform);
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f,0f,0f), Time.deltaTime * 1f);
			}
			else{
				CameraWorks.TargetChanger(null);
				endflag = true;
			}
		}
	}
}

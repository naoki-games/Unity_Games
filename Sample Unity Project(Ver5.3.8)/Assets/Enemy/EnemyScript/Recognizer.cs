using UnityEngine;
using System.Collections;

public class Recognizer : MonoBehaviour {

	public bool endflag = false;

	public GameObject BossOb;
	public GameObject cameraOb;
	public EnemyType enemyType;
	private Animator anim;

	//private cameraMoveScript cmScript;
	private StateMachineSample.BossEnemy Benemy;
	private CameraChangeEV ccEV;
	
	public enum EnemyType{
		Normal,
		Boss,
	}
	// Use this for initialization
	void Start () {
		anim = cameraOb.GetComponent<Animator> ();
		//cmScript = anim.GetComponent<cameraMoveScript> ();
		Benemy = BossOb.GetComponent<StateMachineSample.BossEnemy>();
		ccEV = cameraOb.GetComponent<CameraChangeEV>();
	}

	void Update(){
	}
	
	IEnumerator OnTriggerStay(Collider other){
		if (other.tag == "Player"){
			if(enemyType == EnemyType.Boss){
				if(!endflag){
					endflag = true;
					ccEV.ShowProductionView();
					anim.Play ("cameraMove", -1, 0f);
					BossOb.SetActive(true);
					yield return new WaitForSeconds (2f);
					ccEV.ShowPlayerView ();
					yield return new WaitForSeconds (0.5f);
					Benemy.enabled = true;
					Destroy(gameObject);

				}
			}
		}
	}
}

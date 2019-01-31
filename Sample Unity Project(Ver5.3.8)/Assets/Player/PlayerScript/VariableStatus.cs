using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class VariableStatus : MonoBehaviour {

	public Rigidbody FlameBallPrefab;
	public GameObject HealingPrefab;
	public Transform player;  //set MuzzleObject

	private SoundDataBase database;
	private PlayerStatus PStatus;
	private Inventory inventory;
	public int MaxHP;
	[HideInInspector]public int HP;
	public int MaxMP;
	[HideInInspector]public int Mpoint;
	public float ShotATK = 50;
	public float ATK = 100;
	public int coMP = 1;
	public int coolTime = 2;
	private bool shotFlag = false;
	public float shotPower = 850;
	public bool endFlag = false;

	private bool gameoverflag = false;
	private GameObject source; 

	[HideInInspector]public int score = 0;

	public GUIStyle style;
	private int n = 0;
	// Use this for initialization
	void Start () {
		database = GameObject.FindGameObjectWithTag ("SoundDatabase").GetComponent<SoundDataBase> ();
		source = GameObject.FindGameObjectWithTag ("SoundDatabase");
		PStatus = this.GetComponent<PlayerStatus>();
		MaxHP = PStatus.status[0].charaLife;
		HP = PStatus.status[0].charaLife;
		MaxMP = PStatus.status[0].charaMP;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Rigidbody FlameBallInstance;
		
		if(!shotFlag){
			if(n < coolTime){
				n++;
			}
			else if(n >= coolTime){
				shotFlag = true;
			}
		}else{
			if (!Menu.drawMode) {
				if(Mpoint-coMP >= 0){
					if(Input.GetButton("BulletAttack")) {
						shotFlag = false;
						FlameBallInstance = (Rigidbody)Instantiate(FlameBallPrefab, 
						                                player.transform.position,player.transform.rotation);
						
						FlameBallInstance.AddForce(player.forward * shotPower );
						Mpoint -= coMP;
						n = 0;
					}
				}
			}
		}
		if (!gameoverflag) {
			if (HP <= 0) {
				gameoverflag = true;
				StartCoroutine (fade ());
			}
		}
		
	}

	IEnumerator fade(){
		FadeInOut.flgFade = true;
		FadeInOut.changeAlfa = 1f;
		AudioSource audio = source.GetComponent<AudioSource> ();
		audio.clip = database.sounds [3].audioClip;
		audio.Play ();
		yield return new WaitForSeconds(2);
		FadeInOut.flgFade = false;
		SceneManager.LoadScene("GameOver");
	}

	public void HealingPoint(int hp, int mp){
		if (hp > 0) {
			if (HP != MaxHP) {
				if (HP + hp < MaxHP) {
					HP += hp;
				} else if (HP < MaxHP) {
					hp = MaxHP - HP;
					HP += hp;
				}
			}
		}
		if (mp != 0) {
			if (Mpoint != MaxMP) {
				if (Mpoint + mp < MaxMP) {
					Mpoint += mp;
				} else if (Mpoint < MaxMP) {
					mp = MaxMP - Mpoint;
					Mpoint += mp;
				}
			}
		}
		GameObject HInstance = (GameObject)Instantiate(HealingPrefab,player.transform.position,Quaternion.identity);
		HInstance.transform.parent = player.transform;
	}

	void WeaponStatus(){
		ShotATK = 50;
		shotPower = 850;
		coolTime = 2;
	}

	void DelayMethod(){
		Debug.Log("Delay call");
	}

	void OnGUI () {
		GUI.Box (new Rect (45, 45, 130, 80), "");
		GUI.Label(new Rect(50,80,120,50),"MagicPoint " + Mpoint+"/"+MaxMP,style);
		GUI.Label(new Rect(50,50,120,50),"HP "+HP+"/"+MaxHP,style);
		GUI.Label(new Rect(450,70,100,50),"Score "+score,style);
	}
}

using UnityEngine;
using System.Collections;

public class Player2 : MonoBehaviour {
	
	public float speed = 1.5f; //move distance/second
	public float jumpSpeed = 20.0f;
	public float jumpPower = 10.0f;
	public float slopeLimit = 45f;
	[HideInInspector]public Animator anim;
	[HideInInspector]public Rigidbody rbody;
	public GameObject bladePrefab;
	public Transform camera;
	public Transform blade;
	public float rotateSpeed = 50;

	//private Value
	private float inputH;
	private float inputV;
	private float forward;

	private float specialAttackMove = 5;
	private bool action1 = false; //while motion
	private bool attackMode = false; //while damage motion
	private bool Inoperable = false; //located air
	private bool jump = false;
	private bool Run;
	private bool fall;
	public bool damage = false;

	private int nockbacktime = 0;

	public static bool airState;
	public static float airtime;
	
	private Vector3 move = Vector3.zero;
	Vector3 knockBackDirection;
	Vector3 targetRotation;
	private CharacterController chara;
	//private Rigidbody rig;
	private VariableStatus Vstatus;
	private GameObject bladeInstance;
	AnimatorStateInfo animInfo;

	//RayCast Value
	int layerMask;
	bool isSliding;

	public float rec;

	[SerializeField] float m_GroundCheckDistance = 0.3f;

	// Use this for initialization
	void Start () {
		chara = GetComponent<CharacterController>();
		//rig = GetComponent<Rigidbody>();
		Vstatus = GameObject.FindGameObjectWithTag ("playerInformation").GetComponent<VariableStatus>();
		anim = GetComponent<Animator>();
		rbody = GetComponent<Rigidbody>();
		Run = false;
	}
	
	// Update is called once per frame
	void Update () {
		rec = move.y;
		animInfo = this.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0);

		RaycastHit slideHit;
		layerMask = 1 << LayerMask.NameToLayer("Ground");
		//Debug.DrawLine(transform.position + (Vector3.up * 0.5f), transform.position + (Vector3.up * 0.5f) + transform.forward);
		/*f(airState){
			//レイヤーマスクは⇒で指定 int layerMask =1 << LayerMask.NameToLayer("レイヤー名");
			if (Physics.Raycast(transform.position + (Vector3.up * 0.5f), transform.forward, out slideHit,1.0f,layerMask)) {
				//衝突した際の面の角度とが滑らせたい角度以上かどうかを調べます。
				if (Vector3.Angle(slideHit.normal, Vector3.up) > chara.slopeLimit){
					isSliding = true;
				}else{
					isSliding = false;
				}
			}
			if (move.y <= 0 && !jump) {
				if(isSliding){//滑るフラグが立ってたら
					Vector3 hitNormal = slideHit.normal;
					chara.Move(hitNormal*0.1f);
				}
			}
		}*/
		CheckGroundStatus();

		if(Input.GetKey(KeyCode.LeftShift))
		{
			Run = true;
		}
		else
		{
			Run = false;
		}

		/*if (animInfo.IsName ("Damage")) {
			Inoperable = true;
		} else if (animInfo.IsName ("Lying")) {
			Inoperable = true;
		} else {
			Inoperable = false;
		}*/

		anim.SetFloat ("inputV",inputV);
		anim.SetFloat ("inputH",inputH);
		anim.SetFloat ("forward",forward);
		anim.SetFloat ("airPosition",move.y);
		anim.SetFloat ("airtime",airtime);
		anim.SetBool ("action1",action1);
		anim.SetBool ("attackMode",attackMode);
		anim.SetBool ("jump",jump);
		anim.SetBool ("Run",Run);
		anim.SetBool ("airState",airState);

		if (!Inoperable) {

			if (Input.GetButtonDown ("Avoidance")) { //avoid!
				if (action1 == false) {
					anim.Play ("Rolling", -1, 0f);
				}
			}

			if (Input.GetKeyDown ("space")) { //Jump!
				if (airState == false && action1 == false) {
					anim.Play ("Jump", -1, 0f);
					jump = true;
					airState = true;
				}
			}

			inputH = Input.GetAxis ("Horizontal");
			inputV = Input.GetAxis ("Vertical");
			float H = inputH;
			float V = inputV;
			move.x = inputH * speed * Time.deltaTime;
			move.z = inputV * speed * Time.deltaTime;
			if (H < 0)
				H *= -1;
			if (V < 0)
				V *= -1;
			forward = H + V;

			if (Run) {
				move.z *= 2.5f;
				move.x *= 2.5f;
			}

			//While Player stay in air
			if (airState == true) {
				airtime += 1.0f * Time.deltaTime;
				if (!Menu.drawMode) {
					if (!Run) {
						if (Input.GetButtonDown ("BladeAttack")) { //NormalAttack!
							if (action1 == false && !attackMode) {
								WeaponSummon ();
								anim.Play ("AirAttack", -1, 0f);
							}
						}
					}
				}
				if (airState == true && jump == false) {
					if (Run) {
						if (Input.GetButtonDown ("BladeAttack")) { //SpecialAttack!
							if (action1 == false && !attackMode) {
								WeaponSummon ();
								anim.Play ("Attack1", -1, 0f);
							}
						}
					}
					if (action1 == true) {
						chara.Move (transform.forward * specialAttackMove * Time.deltaTime);
						//rig.position += transform.forward * specialAttackMove * Time.deltaTime;
					}
				}
				//While Player stay in ground
			} else if (airState == false && jump == false) { //foot ground

				if (!Menu.drawMode) {
					if (Input.GetButton ("BulletAttack")) { //avoid!
						if (action1 == false && !attackMode) {
							anim.Play ("Magic", -1, 0f);
						}
					}
					if (Run) {
						if (Input.GetButtonDown ("BladeAttack")) { //SpecialAttack!
							if (action1 == false && !attackMode) {
								WeaponSummon ();
								anim.Play ("Attack1", -1, 0f);
							}
						}
					} else if (!Run) {
						if (Input.GetButtonDown ("BladeAttack")) { //NormalAttack!
							if (action1 == false && !attackMode) {
								WeaponSummon ();
								anim.Play ("Attack2", -1, 0f);
							}
						}
					}

				}
				move.y = 0;
				if (action1 == true) {
					chara.Move (transform.forward * specialAttackMove * Time.deltaTime);
					//rig.position += transform.forward * specialAttackMove * Time.deltaTime;
				} else if (action1 == false) {
					chara.Move (camera.transform.forward * move.z + camera.transform.right * move.x);
					//rig.position += camera.transform.forward * move.z + camera.transform.right * move.x;
				}
			}

			//Air behavior control
			if (airState == true || jump == true) {
				if (jump == true) {
					move.y += jumpSpeed * Time.deltaTime;
					if (move.y >= jumpPower) {
						jump = false;
					}
				} else {
					if (animInfo.IsName ("AirAttack")) {
						move.y += 3.0f * Time.deltaTime;
					} else {
						move.y += Physics.gravity.y * Time.deltaTime;
					}
				}
				chara.Move (camera.transform.forward * move.z + camera.transform.right * move.x + move * Time.deltaTime);
				//rig.position += camera.transform.forward * move.z + camera.transform.right * move.x + move * Time.deltaTime;
			}

			//chara rotation settings
			if (!action1) {
				Vector3 rotateV = camera.transform.forward;
				Vector3 rotateH = camera.transform.right;
				if (inputV != 0) {
					if (inputV < 0)
						rotateV *= -1;
					Quaternion rotation = Quaternion.LookRotation (rotateV);
					rotation.x = rotation.z = 0;
					transform.rotation = rotation;
				}
				if (inputH != 0) {
					if (inputH < 0)
						rotateH *= -1;
					Quaternion rotation = Quaternion.LookRotation (rotateH);
					rotation.x = rotation.z = 0;
					transform.rotation = rotation;
				}
				if (inputH != 0 && inputV != 0) {
					Quaternion rotation = Quaternion.LookRotation (rotateV + rotateH);
					rotation.x = rotation.z = 0;
					transform.rotation = rotation;
				}
			}
		} else {  //Inoperable = true
			if (damage == true) {
				if (nockbacktime >= 0) {
					//Debug.Log (nockbacktime);
					chara.Move ((knockBackDirection * 2.5f) * Time.deltaTime);
					airtime += 1.0f * Time.deltaTime;
					nockbacktime--;
				} else {
					damage = false;
					Inoperable = false;
				}
			}
		}

	}

	void NockBack(Transform pos, Transform targetObject){
		Inoperable = true;
		damage = true;
		airState = true;
		nockbacktime = 200;
		this.GetComponent<BoxCollider>().enabled = false;
		if(bladeInstance){
			GameObject.Destroy(bladeInstance.gameObject);
		}
		targetRotation = Quaternion.LookRotation(targetObject.transform.position - pos.transform.position).eulerAngles;
		knockBackDirection = (targetObject.transform.position - pos.transform.position).normalized;
		knockBackDirection.y += 1;
		knockBackDirection.x *= -1;
		knockBackDirection.z *= -1;
		targetRotation.x = targetRotation.z = 0;
		transform.rotation = Quaternion.Euler (targetRotation);
		action1 = false;
		attackMode = false;
	}

	void WeaponSummon(){
		bladeInstance = (GameObject)Instantiate(bladePrefab,blade.transform.position,blade.transform.rotation);
		bladeInstance.transform.parent = blade.transform;
	}

	public void Damage(Transform enemy){
		if (!damage) {
			Vstatus.HP -= 50;
			NockBack (transform, enemy);
			anim.Play ("Damage", -1, 0f);
			anim.speed = 1.0f;
		}
	}

	//When Player received from Enemy
	void OnTriggerStay(Collider other){
		if (other.tag == "EnemyBullet") {
			Damage (other.transform);
		}
	}

	//GroundJudgeScript Function called
	void CheckGroundStatus()
	{
		RaycastHit hitInfo;

		//Ray ray = new Ray (transform.position + (Vector3.up * 0.01f), Vector3.down);
		//Debug.DrawRay (ray.origin, ray.direction, Color.red, 1.0f, false);
		float radius = 0.2f;
		// 0.1f is a small offset to start the ray from inside the character
		// it is also good to note that the transform position in the sample assets is at the base of the character
		if (Physics.SphereCast (transform.position + (Vector3.up * 0.3f), radius,Vector3.down, out hitInfo, m_GroundCheckDistance, layerMask)) {
		//if (Physics.Raycast(ray, out hitInfo, m_GroundCheckDistance,layerMask)){
			//衝突した際の面の角度とが滑らせたい角度以上かどうかを調べます。
			if (Vector3.Angle(hitInfo.normal, Vector3.up) > chara.slopeLimit){
				isSliding = true;
			}else{
				isSliding = false;
			}
			if (airState) {
				if (airState && airtime > 0.5f && !action1 && !isSliding) {
					if (damage == true) {
						anim.Play ("Lying", -1, 0f);
					} else {
						anim.Play ("JumpDown", -1, 0f);
						Inoperable = false;
					}
					action1 = false;
					airtime = 0;
					airState = false;
					nockbacktime = 0;
					anim.speed = 1.0f;
					this.GetComponent<BoxCollider>().enabled = true;
				}
				if (move.y <= 0 && !jump) {
					if (isSliding) {//滑るフラグが立ってたら
						Vector3 hitNormal = hitInfo.normal;
						chara.Move (hitNormal * 0.1f);
					}
				}
				if (action1 && !isSliding) {
					airState = false;
					airtime = 0;
				}
			}
		}
		else{
			airState = true;
				
		}
	}

	//invincible time
	void InvincibleStart(){
		this.GetComponent<BoxCollider>().enabled = false;
	}
	
	void InvincibleEnd(){
		this.GetComponent<BoxCollider>().enabled = true;
	}


	//animation function
	void ActionStart(){
		action1 = true;
	}

	void InoperableStart(){
		Inoperable = true;
	}

	void InoperableEnd(){
		Inoperable = false;
	}

	void ActionEnd(){
		action1 = false;
		Inoperable = false;
	}
	void AttackStart(){
		attackMode = true;
	}

	void AttackEnd(){
		attackMode = false;
		if(bladeInstance){
			GameObject.Destroy(bladeInstance.gameObject);
		}
	}
		
}

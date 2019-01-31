using UnityEngine;
using System.Collections;

namespace StateMachineSample{
	
	public enum BossEnemyState{
		Wander,
		Pursuit,
		Attack,
		Escape,
		Explode,
	}

	public class BossEnemy : StatefulObjectBase<BossEnemy,BossEnemyState> {

		public GameObject GoalObj;
		public Transform turret;
		public Transform muzzle;
		public GameObject bulletPrefab;
		public GameObject subBulletPrefab;
		public GameObject dustEf;
		public GameObject destroyEf;

		private Transform player;

		public float speed = 5f;
		private float rotationSmooth = 1f;
		private float turretRotationSmooth = 0.8f;
		private float attackInterval = 2f;

		public float pursuitSqrDistance = 850f;
		public float escapeSqrDistance = 135f; //escape < pursuit
		public float attackSqrDistance = 120f;
		private float margin = 5f;

		private float changeTargetSqrDistance = 40f;

		private int overRangeTime = 20;

		private void Start(){
			Initialize();
		}

		public void Initialize (){
			player = GameObject.FindWithTag ("Player").transform;

			//StateMachine initialize
			stateList.Add(new StateWander(this));
			stateList.Add(new StatePursuit(this));
			stateList.Add(new StateAttack(this));
			stateList.Add(new StateEscape(this));
			stateList.Add(new StateExplode(this));

			stateMachine = new StateMachine<BossEnemy> ();

			ChangeState (BossEnemyState.Wander);
		}

		public void TakeDamage(){
			ChangeState (BossEnemyState.Explode);
		}

		#region State

		/// <summary>
		/// State:loitering(haikai)
		/// </summary>
		private class StateWander : State<BossEnemy>
		{
			private Vector3 targetPosition;

			public StateWander(BossEnemy owner) : base(owner){}

			// Use this for initialization
			public override void Enter () {
				targetPosition = GetRandomPositionOnLevel ();
			}
			
			// Update is called once per frame
			public override void Execute () {
				// when this is near target,retarget
				float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
				if (sqrDistanceToPlayer <  owner.pursuitSqrDistance - owner.margin)
				{ 
					owner.ChangeState(BossEnemyState.Pursuit);
				}

				// 
				float sqrDistanceToTarget = Vector3.SqrMagnitude(owner.transform.position - targetPosition);
				if (sqrDistanceToTarget < owner.changeTargetSqrDistance)
				{
					targetPosition = GetRandomPositionOnLevel();
				}

				// 
				Quaternion targetRotation = Quaternion.LookRotation(targetPosition - owner.transform.position);
				owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

				// forward
				owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
			
			}

			public override void Exit(){}

			public Vector3 GetRandomPositionOnLevel()
			{
				float levelSize = 55f;
				return new Vector3(Random.Range(-levelSize, levelSize), 0, Random.Range(-levelSize, levelSize));
			}
		}

		/// <summary>
		/// State: Pursuit
		/// </summary>
		private class StatePursuit : State<BossEnemy>
		{
			public StatePursuit(BossEnemy owner) : base(owner) {}

			public override void Enter() {}

			public override void Execute()
			{
				// 
				float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
				if (sqrDistanceToPlayer < owner.attackSqrDistance - owner.margin)
				{ 
					owner.ChangeState(BossEnemyState.Attack);
				}

				// 
				if (sqrDistanceToPlayer > owner.pursuitSqrDistance + owner.margin)
				{ 
					owner.ChangeState(BossEnemyState.Wander);
				}

				// 
				Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
				owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);

				// 
				owner.transform.Translate(Vector3.forward * owner.speed * Time.deltaTime);
			}

			public override void Exit() {}
		}

		/// <summary>
		/// State: Attack
		/// </summary>
		private class StateAttack : State<BossEnemy>
		{
			private float lastAttackTime;
			private int count = 0;

			public StateAttack(BossEnemy owner) : base(owner) { }

			public override void Enter() {}

			public override void Execute()
			{
				// 
				float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
				if (sqrDistanceToPlayer > owner.attackSqrDistance + owner.margin)
				{ 
					owner.ChangeState(BossEnemyState.Pursuit);
				}

				if (count > owner.overRangeTime)
				{ 
					count = 0;
					owner.ChangeState(BossEnemyState.Escape);
				}

				// 
				float rotateY = (owner.turret.localEulerAngles.y > 180) ? owner.turret.localEulerAngles.y - 360 : owner.turret.localEulerAngles.y;
				Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.turret.position);
				if(rotateY < 60){
					if(rotateY > -60){
						owner.turret.rotation = Quaternion.Slerp(owner.turret.rotation, targetRotation, Time.deltaTime * owner.turretRotationSmooth);
					}
					else{
						count++;
						owner.turret.localEulerAngles = new Vector3(owner.turret.localEulerAngles.x, owner.turret.localEulerAngles.y+1f, owner.turret.localEulerAngles.z);
					}
				}
				else{
					count++;
					owner.turret.localEulerAngles = new Vector3(owner.turret.localEulerAngles.x, owner.turret.localEulerAngles.y-1f, owner.turret.localEulerAngles.z);
				}

				// 
				if (Time.time > lastAttackTime + owner.attackInterval)
				{
					Instantiate(owner.bulletPrefab, owner.muzzle.position, owner.muzzle.rotation);
					lastAttackTime = Time.time;
				}
			}

			public override void Exit() {}
		}

		/// <summary>
		/// State: Escape
		/// </summary>
		private class StateEscape : State<BossEnemy>
		{
			public StateEscape(BossEnemy owner) : base(owner) {}
			
			public override void Enter() {
				Instantiate(owner.dustEf,owner.transform.position,Quaternion.identity);
				for(int i=0;i<5;i++){
					Instantiate(owner.subBulletPrefab,owner.transform.position,Quaternion.identity);
				}
			}
			
			public override void Execute()
			{
				// 
				float sqrDistanceToPlayer = Vector3.SqrMagnitude(owner.transform.position - owner.player.position);
				// 
				if (sqrDistanceToPlayer > owner.escapeSqrDistance + owner.margin)
				{ 
					owner.ChangeState(BossEnemyState.Pursuit);
				}
				
				// 
				Quaternion targetRotation = Quaternion.LookRotation(owner.player.position - owner.transform.position);
				owner.transform.rotation = Quaternion.Slerp(owner.transform.rotation, targetRotation, Time.deltaTime * owner.rotationSmooth);
				
				// 
				owner.transform.Translate(Vector3.back * owner.speed * Time.deltaTime);
			}
			
			public override void Exit() {}
		}

		/// <summary>
		/// State: Explode
		/// </summary>
		public class StateExplode : State<BossEnemy>
		{
			public StateExplode(BossEnemy owner) : base(owner) {}

			public override void Enter()
			{
				// 
				Vector3 force = Vector3.up * 1000f + Random.insideUnitSphere * 300f;
				owner.GetComponent<Rigidbody>().AddForce(force);

				// 
				Vector3 torque = new Vector3(Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f), Random.Range(-10000f, 10000f));
				owner.GetComponent<Rigidbody>().AddTorque(torque);

				owner.GoalObj.SetActive(true);
				// 1
				Destroy(owner.gameObject, 1.0f);
				
				GameObject clone = 
					Instantiate(owner.destroyEf, owner.transform.position, owner.transform.rotation) as GameObject;

			}

			public override void Execute() {}

			public override void Exit() {}
		}

		#endregion
	}
}

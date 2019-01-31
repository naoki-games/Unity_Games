using UnityEngine;
using System.Collections;

public class CameraWorks : MonoBehaviour {

	public Transform rotatetarget;
	public float xspeed = 100;
	public float yspeed = 100;
	public Transform targetObj;
	public Rigidbody rbody;
	public float distance = 10.0f;
	public float dy = 1.0f;


	static Transform EVtarget;
	private float x = 0.0f;
	private float y = 0.0f;
	private Vector3 position;
	private Quaternion rotation;
	private Vector3 pretarget;

	public float yMinLimit = -20f;
	public float yMaxLimit = 80f;

	// Use this for initialization
	void Start () {
		x = rotatetarget.rotation.y;   
		y = rotatetarget.rotation.x;

		pretarget = targetObj.position;
		// Make the rigid body not change rotation    
		if (rbody = GetComponent<Rigidbody>())      
			rbody.freezeRotation = true;
		//if there is Menu.cs
	}
	
	// Update is called once per frame
	void Update () {
		if (!Menu.drawMode) {
			if(EVtarget == null){
				x += Input.GetAxis("Mouse X") * xspeed * 0.1f;        
				y -= Input.GetAxis("Mouse Y") * yspeed * 0.1f;        

				y = ClampAngle(y, yMinLimit, yMaxLimit);

				rotation = Quaternion.Euler(y, x, 0f);
				position = rotation * new Vector3 (0.0f, dy, -distance) + targetObj.position;


				rotatetarget.rotation = rotation;
				rotatetarget.position = position;
			}
		}
		if(EVtarget != null){
			Quaternion targetRotation = Quaternion.LookRotation(EVtarget.position - rotatetarget.position);
			rotatetarget.rotation = Quaternion.Slerp(rotatetarget.rotation, targetRotation, Time.deltaTime * 1f);
		}

		if (Player2.airState) {
			if (pretarget.y > targetObj.position.y) {
				rotatetarget.position = Quaternion.Euler (y, x, 0f) * new Vector3 (0.0f, dy, -distance) + new Vector3(targetObj.position.x,pretarget.y,targetObj.position.z);
				pretarget.y -= 0.1f;
			}
		} else {
			if ((pretarget.y - targetObj.position.y + dy) <= 1f) {
				rotatetarget.position = Quaternion.Euler (y, x, 0f) * new Vector3 (0.0f, dy, -distance) + targetObj.position;
				pretarget = targetObj.position;
			} else {
				rotatetarget.position = Quaternion.Euler (y, x, 0f) * new Vector3 (0.0f, dy, -distance) + new Vector3(targetObj.position.x,pretarget.y,targetObj.position.z);
				pretarget.y -= 0.2f;
			}
		}
	}

	static float ClampAngle (float angle, float min, float max) {
		if (angle < -360)    
			angle += 360;    
		if (angle > 360)    
			angle -= 360;    
		return Mathf.Clamp (angle, min, max);
	}

	public static void TargetChanger(Transform target){
		EVtarget = target;
	}
}

using UnityEngine;
using System.Collections;

public class TimeCounter : TextSizeBase {

	public bool clear = false;
	public float second;
	public int minute;

	// Use this for initialization
	void Start () {
		second = 0;
		minute = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(clear == false){
			second += Time.deltaTime;
			if(second >= 60){
				second=0;
				minute+=1;
			}
		} 
	}

	void OnGUI(){
		Rect trect = new Rect (450, 20, 100, 50);
		GUI.Label(trect,"Time "+minute+":"+second,myStyle);
	}
}

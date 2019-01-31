using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GoalScript : TextSizeBase {

	public bool flgDisp; //diplay flag
	private AudioSource audioSource;
	private SoundDataBase database;

	public static bool SampleClear = false;
	GUIStyle style;
	public float reSecond;
	public int reMinute;
	public int reScore;
	int waittime = 10;

	private TimeCounter Tcounter;
	private VariableStatus Vstatus;

	// Use this for initialization
	void Start () {
		audioSource = gameObject.GetComponent<AudioSource>();
		database = GameObject.FindGameObjectWithTag ("SoundDatabase").GetComponent<SoundDataBase> ();
		Tcounter = GameObject.FindGameObjectWithTag ("TimeCounter").GetComponent<TimeCounter>();
		Vstatus = GameObject.FindGameObjectWithTag ("playerInformation").GetComponent<VariableStatus>();
	
	}
	IEnumerator OnTriggerEnter (Collider other) {
		Debug.Log("Hit");
		if (other.tag == "Player") {
			if(Tcounter.clear == false){
				Debug.Log("player");
				Tcounter.clear = true;
				reSecond = Tcounter.second;
				reMinute = Tcounter.minute;
				reScore = Vstatus.score;

				flgDisp = true;

				for(int i=0;i<5;i++){
					if(TitleMenu.AreaMinute[i] == 0){
						TitleMenu.AreaSecond[i] = reSecond;
						TitleMenu.AreaMinute[i] = reMinute;
						break;
					}
				}
				audioSource.clip = database.sounds[4].audioClip;
				audioSource.Play ();
				FadeInOut.flgFade = true;
				FadeInOut.changeAlfa = 0.5f;
				yield return new WaitForSeconds(waittime);
				FadeInOut.flgFade = false;
				SceneManager.LoadScene("GameTitle");
				SampleClear = true;
			}

		}
	}

	void OnGUI(){

		float factorSize = Screen.width / screenWidth;

		float msgwX;
		float msgwY;
		float msgwW = msgwWidth * factorSize;
		float msgwH = msgwHeight * factorSize;

		GUIStyle myStyle = new GUIStyle();
		//message shadow
		myStyle.normal.textColor = Color.yellow;
		myStyle.fontSize = (int)(40 * factorSize);
		msgwX = (msgwPosX + 62) * factorSize;

		if(flgDisp == true){
			msgwX = msgwPosX * factorSize;
			msgwY = msgwPosY * factorSize;
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0.5f);
			GUI.Label(new Rect(msgwX,msgwY,msgwW,msgwH),"GameClear!!",myStyle);

			//message shadow
			myStyle.normal.textColor = Color.white;

			myStyle.fontSize = (int)(32 * factorSize);
			msgwX = (msgwPosX - 22) * factorSize;

			msgwY = (msgwPosY + 50) * factorSize;
			GUI.Box(new Rect(msgwX,msgwY,msgwW,msgwH),"");
			GUI.Label(new Rect(msgwX,msgwY,msgwW,msgwH),"result",myStyle);

			myStyle.fontSize = (int)(25 * factorSize);
			msgwX = (msgwPosX - 5) * factorSize;
			msgwY = (msgwPosY + 100) * factorSize;
			GUI.Label(new Rect(msgwX,msgwY,msgwW,msgwH),"Score  "+reScore,myStyle);
			msgwY = (msgwPosY + 140) * factorSize;
			GUI.Label(new Rect(msgwX,msgwY,msgwW,msgwH),"Time  "+reMinute+":"+reSecond,myStyle);

		}
	}
}

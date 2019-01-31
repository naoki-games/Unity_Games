using UnityEngine;
using System.Collections;

public class GameCompleteText : TextSizeBase {

	public bool flgDisp; //diplay flag

	// Use this for initialization
	void Start () {
		
	}

	void OnGUI(){

		float factorSize = Screen.width / screenWidth;

		float msgwX;
		float msgwY;
		float msgwW = msgwWidth * factorSize;
		float msgwH = msgwHeight * factorSize;

		GUIStyle myStyle = new GUIStyle();
		myStyle.fontSize = (int)(40 * factorSize);
		msgwX = (msgwPosX + 62) * factorSize;

		if(flgDisp == true){
			Menu.drawMode = true;
			msgwX = msgwPosX * factorSize;
			msgwY = msgwPosY * factorSize;
			GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, 0.5f);
			GUI.Label(new Rect(msgwX,msgwY,msgwW,msgwH),"GameClear!!",myStyle);

			//message shadow
			myStyle.normal.textColor = Color.black;

			myStyle.fontSize = (int)(32 * factorSize);
			msgwX = (msgwPosX - 22) * factorSize;

			msgwY = (msgwPosY + 50) * factorSize;
			GUI.Box(new Rect(msgwX,msgwY,msgwW,msgwH),"");
			GUI.Label(new Rect(msgwX,msgwY,msgwW,msgwH),"result",myStyle);

			myStyle.fontSize = (int)(25 * factorSize);
			msgwX = (msgwPosX - 5) * factorSize;
			msgwY = (msgwPosY + 100) * factorSize;
			GUI.Label(new Rect(msgwX,msgwY,msgwW,msgwH),"Score",myStyle);
			msgwY = (msgwPosY + 140) * factorSize;
			GUI.Label(new Rect(msgwX,msgwY,msgwW,msgwH),"Time",myStyle);

			if(Input.GetMouseButtonUp(0)){
				flgDisp = false;
				Menu.drawMode = false;
			}
		}
	}
}

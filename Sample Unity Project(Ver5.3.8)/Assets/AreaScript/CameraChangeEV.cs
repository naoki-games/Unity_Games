using UnityEngine;
using System.Collections;

public class CameraChangeEV : MonoBehaviour {
	public Camera playerCamera;
	public Camera proCamera;
	
	public void ShowProductionView() {
		playerCamera.enabled = false;
		proCamera.enabled = true;
	}
	
	public void ShowPlayerView() {
		playerCamera.enabled = true;
		proCamera.enabled = false;
	}
}
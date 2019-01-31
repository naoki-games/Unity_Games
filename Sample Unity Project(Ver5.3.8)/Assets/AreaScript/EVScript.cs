using UnityEngine;
using System.Collections;

public class EVScript : MonoBehaviour {
	public float n;
	private CameraChangeEV ccEV;
	// Use this for initialization
	IEnumerator Start () {
		ccEV = this.GetComponent<CameraChangeEV>();
		ccEV.ShowProductionView();
		for(n=0;n<15;n++){
			transform.Translate (0.1f,0f,0f);
			yield return new WaitForSeconds(0.01f);
		}
		yield return new WaitForSeconds(3f);
		ccEV.ShowPlayerView();
		Destroy(gameObject);
	}

}

using UnityEngine;
using System.Collections;

public class switchScript : MonoBehaviour {
	
	public Transform EVOb;
	public int clipnum;
	public Transform source;

	private SoundDataBase database;

	void Start () {
		database = GameObject.FindGameObjectWithTag ("SoundDatabase").GetComponent<SoundDataBase> ();
	}
	void OnTriggerStay (Collider collider) {
		
		if (collider.gameObject.tag == "Player") {
			Debug.Log("Hit");
			AudioSource audio = source.GetComponent<AudioSource>();
			audio.clip = database.sounds[clipnum].audioClip;
			audio.Play();
			EVOb.gameObject.AddComponent<EVScript>();
			Destroy(gameObject);
		}
		if (collider.gameObject.tag == "bullet") {
			Debug.Log("Hit");
			AudioSource audio = source.GetComponent<AudioSource>();
			audio.clip = database.sounds[clipnum].audioClip;
			audio.Play();
			EVOb.gameObject.AddComponent<EVScript>();
			Destroy(gameObject);
		}
	}
}

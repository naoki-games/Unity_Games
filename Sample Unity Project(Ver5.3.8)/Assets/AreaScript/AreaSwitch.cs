using UnityEngine;
using System.Collections;

public class AreaSwitch : MonoBehaviour {

	public GameObject cube;
	public Transform targetpos;
	public Transform source;
	public int clipnum;

	private SoundDataBase database;

	void Start () {
		database = GameObject.FindGameObjectWithTag ("SoundDatabase").GetComponent<SoundDataBase> ();
	}

	void OnTriggerEnter (Collider other) {

		if (other.tag == "Player") {
			AudioSource audio = source.GetComponent<AudioSource>();
			audio.clip = database.sounds[clipnum].audioClip;
			audio.Play();
			Instantiate(cube,targetpos.position,targetpos.rotation);
			Destroy(gameObject);
		}
		if (other.tag == "bullet") {
			AudioSource audio = source.GetComponent<AudioSource>();
			audio.clip = database.sounds[clipnum].audioClip;
			audio.Play();
			Instantiate(cube,targetpos.position,targetpos.rotation);
			Destroy(gameObject);
		}
	}
}

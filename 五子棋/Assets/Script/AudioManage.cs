using UnityEngine;
using System.Collections;

/*音效管理类*/
public class AudioManage : MonoBehaviour {

	public static AudioManage instance;
	AudioClip ac=null;
	AudioSource al=null;

	void Start(){
		Application.runInBackground = true;
		instance = this;
		al = this.GetComponent<AudioSource> ();
	}

	//设置音效
	public void SetAudio(string  audioName){

		ac=Resources.Load (audioName) as AudioClip;

		al.PlayOneShot (ac);
	}
}
